Status: In-Progress

GitHub Actions Release Plan for Night Engine (Night.dll) - Compiled Version Info

This document outlines the implementation plan for a robust, manually triggerable GitHub Actions-based release process for the Night C# library, focusing on creating GitHub Releases. The version information will be compiled directly into the library.
1. Overview

The goal is to automate the versioning, building, testing, and packaging of the Night.dll library, culminating in a GitHub Release with the generated packages as assets. The process will be initiated manually via workflow_dispatch, allowing the user to specify the exact Semantic Version for the release. The library will contain a `VersionInfo.cs` file where the Semantic Version is updated by the GitHub Action, and a manually editable `CodeName` is stored.

Key Information from Repository Digest:

    Solution File: Night.sln (located at the repository root)
    Main Library Project: src/Night/Night.csproj (this is the project to be versioned and packaged as Night.dll)
    Test Project: tests/Night.Tests/Night.Tests.csproj
    Target Framework: net9.0
    .NET SDK Version: 9.0.x (aligning with existing CI)
    Default Branch: main
    Root Namespace for Library: Night

2. Prerequisites and Initial Setup

Before implementing the release workflow, ensure the following are in place:

    .NET SDK:
        Ensure your development environment and GitHub Actions runners have access to .NET SDK version 9.0.x. The workflow will use actions/setup-dotnet to configure this.
    GitHub CLI (gh):
        The GitHub CLI is used for creating GitHub Releases. It's typically available on GitHub-hosted runners.
    GitHub Actions Workflow Permissions:
        The workflow will need permissions to write to the repository (for committing .csproj and .cs changes, creating tags, and creating GitHub releases). The following permissions block should be included in the workflow file:

        permissions:
          contents: write

3. GitHub Actions Workflow File (`.github/workflows/release.yml`)

Create/update the file with the following content:

```yaml
name: Release Night Library (GitHub Release)

on:
  workflow_dispatch:
    inputs:
      version:
        description: 'Semantic Version for the release (e.g., 1.0.0, 1.0.0-beta.1). This is the pure SemVer.'
        required: true
        type: string

permissions:
  contents: write # To create commits, tags, and releases

jobs:
  release:
    name: Build and Create GitHub Release for Night Library
    runs-on: ubuntu-latest
    env:
      DOTNET_SKIP_FIRST_TIME_EXPERIENCE: true
      DOTNET_CLI_TELEMETRY_OPTOUT: true
      SOLUTION_FILE_PATH: Night.sln
      MAIN_PROJECT_FILE_PATH: src/Night/Night.csproj
      VERSION_INFO_FILE_PATH: src/Night/VersionInfo.cs # Path to the version C# file
      TEST_PROJECT_FILE_PATH: tests/Night.Tests/Night.Tests.csproj
      PACKAGE_OUTPUT_DIR: ./packages

    steps:
      - name: Checkout code
        uses: actions/checkout@v4
        with:
          fetch-depth: 0 # Required to analyze history

      - name: Setup .NET SDK
        uses: actions/setup-dotnet@v4
        with:
          dotnet-version: '9.0.x'

      - name: Validate Version Input (SemVer)
        run: |
          version_input="${{ github.event.inputs.version }}"
          semver_regex="^([0-9]+)\\.([0-9]+)\\.([0-9]+)(?:-([0-9A-Za-z-]+(?:\\.[0-9A-Za-z-]+)*))?(?:\\+([0-9A-Za-z-]+(?:\\.[0-9A-Za-z-]+)*))?$"
          if [[ ! "$version_input" =~ $semver_regex ]]; then
            echo "Error: Invalid version format. Input must be a pure Semantic Version (e.g., 1.0.0, 1.2.3-beta.1)."
            exit 1
          fi
          echo "SemVer input '$version_input' is valid."
        shell: bash

      - name: Update Version in .csproj
        id: update_version_csproj
        run: |
          $newSemVer = "${{ github.event.inputs.version }}"
          $projectFilePath = "${{ env.MAIN_PROJECT_FILE_PATH }}"
          Write-Host "Attempting to update <Version> in '$projectFilePath' to '$newSemVer'"
          [xml]$csproj = Get-Content -Path $projectFilePath -Raw
          $versionNode = $csproj.SelectSingleNode("//PropertyGroup/Version")
          if (-not $versionNode) {
              $propertyGroupNode = $csproj.SelectSingleNode("//PropertyGroup")
              if (-not $propertyGroupNode) {
                  $propertyGroupNode = $csproj.CreateElement("PropertyGroup")
                  $csproj.Project.AppendChild($propertyGroupNode) | Out-Null
              }
              $versionNode = $csproj.CreateElement("Version")
              $propertyGroupNode.AppendChild($versionNode) | Out-Null
          }
          $versionNode.'#text' = $newSemVer
          $csproj.Save($projectFilePath)
          Write-Host "Saved <Version> $newSemVer to '$projectFilePath'"
          echo "version_tag=v$newSemVer" >> $GITHUB_OUTPUT
        shell: pwsh

      - name: Update Version in VersionInfo.cs
        run: |
          $newSemVer = "${{ github.event.inputs.version }}"
          $versionInfoFilePath = "${{ env.VERSION_INFO_FILE_PATH }}"
          Write-Host "Attempting to update Version constant in '$versionInfoFilePath' to '$newSemVer'"
          $content = Get-Content $versionInfoFilePath -Raw
          # Regex to find 'public const string Version = ".*";' and replace the version string part
          $updatedContent = $content -replace '(?<=public const string Version = ")([^"]*)(?=";)', $newSemVer
          Set-Content -Path $versionInfoFilePath -Value $updatedContent
          Write-Host "Updated Version constant in '$versionInfoFilePath'"
        shell: pwsh

      - name: Commit Version Changes
        run: |
          git config --global user.name "${{ github.actor }}"
          git config --global user.email "${{ github.actor }}@users.noreply.github.com"
          git add "${{ env.MAIN_PROJECT_FILE_PATH }}" # .csproj
          git add "${{ env.VERSION_INFO_FILE_PATH }}" # VersionInfo.cs
          git commit -m "Update version to ${{ github.event.inputs.version }} [skip ci]"
          echo "Committed version updates for ${{ github.event.inputs.version }}"
        shell: bash

      - name: Create Git Tag
        run: |
          git tag "${{ steps.update_version_csproj.outputs.version_tag }}"
          echo "Created git tag ${{ steps.update_version_csproj.outputs.version_tag }}"
        shell: bash

      - name: Push Commit and Tag
        run: |
          git push origin HEAD:main --follow-tags
          echo "Pushed commit and tag to remote."
        shell: bash

      - name: Build Solution
        run: dotnet build "${{ env.SOLUTION_FILE_PATH }}" -c Release /p:Version="${{ github.event.inputs.version }}"

      - name: Run Tests
        run: dotnet test "${{ env.SOLUTION_FILE_PATH }}" --no-build -c Release

      - name: Create Package Output Directory
        run: mkdir -p "${{ env.PACKAGE_OUTPUT_DIR }}"

      - name: Package Library
        run: |
          dotnet pack "${{ env.MAIN_PROJECT_FILE_PATH }}" `
            --no-build `
            -c Release `
            -o "${{ env.PACKAGE_OUTPUT_DIR }}" `
            /p:Version="${{ github.event.inputs.version }}" `
            /p:IncludeSymbols=true `
            /p:SymbolPackageFormat=snupkg
        shell: pwsh

      - name: List Packaged Files
        run: ls -R "${{ env.PACKAGE_OUTPUT_DIR }}"
        shell: bash

      - name: Create GitHub Release
        env:
          GH_TOKEN: ${{ secrets.GITHUB_TOKEN }}
          VERSION_TAG: ${{ steps.update_version_csproj.outputs.version_tag }}
          RELEASE_VERSION: ${{ github.event.inputs.version }}
        run: |
          gh release create "$VERSION_TAG" \
            "${{ env.PACKAGE_OUTPUT_DIR }}"/*.nupkg \
            "${{ env.PACKAGE_OUTPUT_DIR }}"/*.snupkg \
            --title "Release $RELEASE_VERSION" \
            --notes "Night Engine Release $RELEASE_VERSION" \
            --draft=false \
            --prerelease=$([[ "$RELEASE_VERSION" == *-* ]] && echo true || echo false)
        shell: bash
```

4. C# Library Version Information (`src/Night/VersionInfo.cs`)

The library's version information will be stored and retrieved from a compiled C# file.

    Create `src/Night/VersionInfo.cs`:
    This file will contain the version information. The `Version` constant will be updated by the GitHub Actions workflow. The `CodeName` constant is for manual developer updates.

    ```csharp
    // In src/Night/VersionInfo.cs
    namespace Night
    {
        public static class VersionInfo
        {
            // This SemVer value is updated by the GitHub Action (e.g., "1.0.0", "1.2.3-beta.1")
            // It is used for AssemblyInformationalVersion and runtime GetVersion()
            public const string Version = "0.0.0-dev"; // Initial placeholder

            // This value is manually updated by the developer during development cycles.
            // It is not automatically used in the release title or notes by default.
            public const string CodeName = "Initial Development"; // Manual placeholder

            /// <summary>
            /// Gets the Semantic Version of the Night library.
            /// This version is set during the release process.
            /// </summary>
            /// <returns>The library's semantic version string.</returns>
            public static string GetVersion()
            {
                return Version;
            }

            // Example: If you want a way to get codename, you can add a method like this:
            // public static string GetCodeName()
            // {
            //     return CodeName;
            // }
            //
            // public static string GetFullVersionDisplay()
            // {
            //     return $"{Version} ('{CodeName}')";
            // }
        }
    }
    ```

    Update `.csproj` File (`src/Night/Night.csproj`):
    Ensure `VERSION.txt` is NO LONGER included if it was previously. The `VersionInfo.cs` file is compiled by default as a `.cs` file within the project. No specific `<Content>` tag is needed for it. The `<Version>` tag in the `.csproj` will still be updated by the workflow, which influences `AssemblyVersion`, `FileVersion`, and `AssemblyInformationalVersion` if not otherwise specified. The `AssemblyInformationalVersion` will effectively be the same as `VersionInfo.Version` after the workflow runs.

5. Step-by-Step Implementation Instructions for You (Agent)

    Update Workflow File:
        Create or update the `.github/workflows/release.yml` file with the YAML content from Section 3.

    Create `src/Night/VersionInfo.cs`:
        Create the `src/Night/VersionInfo.cs` file with the C# code from Section 4. Commit with the initial placeholders.

    Delete `src/Night/VERSION.txt` (if it exists):
        Ensure this file is removed from the `src/Night/` directory and from source control if previously committed.

    Update `src/Night/Night.csproj`:
        If the `<Content Include="VERSION.txt">...</Content>` item group exists from a previous plan, remove it. The workflow updates `<Version>` in the .csproj directly.

    Commit and Push Changes:
        Commit all changes:
        `git add .github/workflows/release.yml src/Night/VersionInfo.cs src/Night/Night.csproj`
        (If `VERSION.txt` was tracked, also `git rm src/Night/VERSION.txt`)
        `git commit -m "refactor: Implement compiled versioning with VersionInfo.cs and update release workflow"`
        `git push origin main`

6. User Guide for Running the Release Workflow

    Navigate to Actions in your GitHub repository.
    Select the "Release Night Library (GitHub Release)" workflow.
    Click `Run workflow`.
    Enter the pure Semantic Version (e.g., 1.0.0, 0.2.0-beta.1) in the input field. This version will be used for Git tagging, .csproj updates, `VersionInfo.cs` updates, and the GitHub release title.
    Click `Run workflow`.

7. Tailored Considerations and Best Practices

    `AssemblyInformationalVersion`: The `<Version>` tag set in the `.csproj` by the workflow directly influences the `AssemblyInformationalVersionAttribute` during the build. The `VersionInfo.GetVersion()` method will return the same SemVer string that is also effectively in `AssemblyInformationalVersionAttribute`.
    `CodeName` Usage: The `CodeName` constant in `VersionInfo.cs` is for your internal tracking and development. It is not automatically included in release artifacts or titles by this workflow. You can manually include it in `CHANGELOG.md` or release notes if desired.
    Branching Strategy: Unchanged. All operations target the `main` branch by default in the workflow.
    Testing the Workflow: Test with pre-release SemVer strings.

This revised plan focuses on a compiled-in version string, updated by the GitHub Action, and includes a manual codename field, removing the need for an external `VERSION.txt`.
