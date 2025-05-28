# Night Engine

A C# game engine built on top of SDL3.

This project aims to provide a "batteries-included," in code editor experience to game development. A near-parity Love2D-inspired API for C# developers serves as the framework base. On top of this framework, there is an (optional) engine that will provide opinionated solutions for the engine so game designers can focus on building games.

This project also intends to be AI friendly so that non-programmer game designers can build games faster.

## Features

### Project

- [ ] Near-parity Love2D-inspired API
- [ ] LLM friendly documentation automagically generated
- [ ] Game samples

### Engine-Specific Features

- [ ] Manager system (Assets, Scenes, Joystick, etc.)
- [ ] ECS

## Getting Started (Development)

1. Ensure [mise](https://mise.jdx.dev/) is installed.
2. Clone repository

### macOS Development Notes

#### VS Code and `mise` for .NET SDK Versioning

If you are using `mise` to manage your .NET SDK versions on macOS, you might encounter issues where VS Code (when launched via its `.app` bundle, e.g., from Finder, Spotlight, or Raycast) does not correctly pick up the `mise`-activated .NET SDK. This can lead to:

- Linter errors complaining about incorrect .NET versions or missing fundamental types.
- NuGet restore failures (e.g., `NETSDK1045` error) because VS Code's C# Dev Kit attempts to use a globally installed .NET SDK (like .NET 8) instead of the project-specified one (e.g., .NET 9).

This happens because GUI-launched applications on macOS do not typically inherit the full shell environment (like `PATH` modifications) that `mise` sets up in your terminal.

##### Solution: Wrapper Script for Launching VS Code

To ensure VS Code launches with the correct `mise`-managed environment, you can use a wrapper shell script. This script explicitly sets up the environment before launching VS Code.

1. **Create the script** (e.g., save as `~/vscode-launcher.sh` or `~/bin/vscode-launcher.sh`):

    ```zsh
    #!/bin/zsh

    # Wrapper script to launch VS Code with the mise-managed environment.

    # Add mise shims directory to PATH
    MISE_SHIMS_PATH="$HOME/.local/share/mise/shims"
    export PATH="$MISE_SHIMS_PATH:$PATH"

    # Optional: Navigate to your specific project directory if desired
    # cd "/path/to/your/project"

    # Launch VS Code, passing through any arguments
    exec code "$@"
    ```

2. **Make it executable:**

    ```bash
    chmod +x /path/to/your/vscode-launcher.sh
    ```

3. **Configure your launcher** (e.g., Raycast, Alfred, or even a custom Dock icon) to execute this script instead of `Visual Studio Code.app` directly.

This ensures that VS Code and its extensions (like the C# Dev Kit) inherit the correct PATH and use the .NET SDK version specified by `mise` for your project.

## License

This project is licensed under the zlib License. See [LICENSE](LICENSE) for details. Third-party library licences can be found in [NOTICE](docs/NOTICE.md).
