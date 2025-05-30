// <copyright file="ConfigurationManagerTests.cs" company="Night Circle">
// zlib license
//
// Copyright (c) 2025 Danny Solivan, Night Circle
//
// This software is provided 'as-is', without any express or implied
// warranty. In no event will the authors be held liable for any damages
// arising from the use of this software.
//
// Permission is granted to anyone to use this software for any purpose,
// including commercial applications, and to alter it and redistribute it
// freely, subject to the following restrictions:
//
// 1. The origin of this software must not be misrepresented; you must not
//    claim that you wrote the original software. If you use this software
//    in a product, an acknowledgment in the product documentation would be
//    appreciated but is not required.
// 2. Altered source versions must be plainly marked as such, and must not be
//    misrepresented as being the original software.
// 3. This notice may not be removed or altered from any source distribution.
// </copyright>

using System;
using System.IO;
using System.Reflection;
using System.Text.Json;

using Night;

using Xunit;

namespace Night.Tests
{
  /// <summary>
  /// Tests for the <see cref="ConfigurationManager"/> class.
  /// </summary>
  public class ConfigurationManagerTests : IDisposable
  {
    private const string TestDirName = "config_test_temp";
    private readonly string testDirectoryPath;
    private readonly string configFilePath;

    /// <summary>
    /// Initializes a new instance of the <see cref="ConfigurationManagerTests"/> class.
    /// Sets up a temporary directory for config files.
    /// </summary>
    public ConfigurationManagerTests()
    {
      ResetConfigurationManager(); // Ensure a clean state for each test

      var assemblyLocation = Path.GetDirectoryName(typeof(ConfigurationManagerTests).Assembly.Location);
      if (string.IsNullOrEmpty(assemblyLocation))
      {
        throw new InvalidOperationException("Could not determine the assembly location for test setup.");
      }

      this.testDirectoryPath = Path.Combine(assemblyLocation, TestDirName);
      _ = Directory.CreateDirectory(this.testDirectoryPath);
      this.configFilePath = Path.Combine(this.testDirectoryPath, "config.json");
    }

    /// <summary>
    /// Disposes of the test resources by deleting the temporary directory.
    /// </summary>
    public void Dispose()
    {
      // Attempt to reset again to ensure no lingering state affects other test classes
      // though xUnit typically isolates test classes.
      ResetConfigurationManager();
      if (Directory.Exists(this.testDirectoryPath))
      {
        Directory.Delete(this.testDirectoryPath, true);
      }
    }

    /// <summary>
    /// Tests that LoadConfig uses default settings and sets IsLoaded to true when config.json is not found.
    /// </summary>
    [Fact]
    public void LoadConfig_FileNotFound_UsesDefaultsAndIsLoaded()
    {
      // Arrange
      // Ensure config file does not exist (covered by test setup and Dispose)

      // Act
      ConfigurationManager.LoadConfig(this.testDirectoryPath);

      // Assert
      Assert.True(ConfigurationManager.IsLoaded);
      Assert.NotNull(ConfigurationManager.CurrentConfig);

      // Check a few default values from GameConfig and its nested objects
      Assert.Equal("Night Game", ConfigurationManager.CurrentConfig.Window.Title); // Default from WindowConfig
      Assert.Equal(800, ConfigurationManager.CurrentConfig.Window.Width); // Default from WindowConfig
      Assert.True(ConfigurationManager.CurrentConfig.Audio.MixWithSystem); // Default from AudioConfig
    }

    /// <summary>
    /// Tests that LoadConfig uses default settings and sets IsLoaded to true when config.json is empty.
    /// </summary>
    [Fact]
    public void LoadConfig_EmptyFile_UsesDefaultsAndIsLoaded()
    {
      // Arrange
      File.WriteAllText(this.configFilePath, string.Empty);

      // Act
      ConfigurationManager.LoadConfig(this.testDirectoryPath);

      // Assert
      Assert.True(ConfigurationManager.IsLoaded);
      Assert.NotNull(ConfigurationManager.CurrentConfig);
      Assert.Equal("Night Game", ConfigurationManager.CurrentConfig.Window.Title);
      Assert.Equal(800, ConfigurationManager.CurrentConfig.Window.Width);
    }

    /// <summary>
    /// Tests that LoadConfig uses default settings and sets IsLoaded to true when config.json contains invalid JSON.
    /// </summary>
    [Fact]
    public void LoadConfig_InvalidJson_UsesDefaultsAndIsLoaded()
    {
      // Arrange
      File.WriteAllText(this.configFilePath, "{ \"invalidJson\": "); // Incomplete JSON

      // Act
      ConfigurationManager.LoadConfig(this.testDirectoryPath);

      // Assert
      Assert.True(ConfigurationManager.IsLoaded);
      Assert.NotNull(ConfigurationManager.CurrentConfig);
      Assert.Equal("Night Game", ConfigurationManager.CurrentConfig.Window.Title);
      Assert.Equal(800, ConfigurationManager.CurrentConfig.Window.Width);
    }

    /// <summary>
    /// Tests that LoadConfig correctly loads settings from a valid config.json and sets IsLoaded to true.
    /// </summary>
    [Fact]
    public void LoadConfig_ValidJson_LoadsConfigAndIsLoaded()
    {
      // Arrange
      var expectedConfig = new GameConfig
      {
        Identity = "my-custom-game",
        Version = "1.0.0",
        Console = true,
        Window = new WindowConfig { Title = "My Custom Game", Width = 1920, Height = 1080, Fullscreen = true, VSync = false, Borderless = true },
        Audio = new AudioConfig { MixWithSystem = false },
        Modules = new ModulesConfig { Graphics = false, Audio = false },
      };
      string jsonContent = JsonSerializer.Serialize(expectedConfig, new JsonSerializerOptions { WriteIndented = true });
      File.WriteAllText(this.configFilePath, jsonContent);

      // Act
      ConfigurationManager.LoadConfig(this.testDirectoryPath);

      // Assert
      Assert.True(ConfigurationManager.IsLoaded);
      Assert.NotNull(ConfigurationManager.CurrentConfig);
      var actualConfig = ConfigurationManager.CurrentConfig;

      Assert.Equal(expectedConfig.Identity, actualConfig.Identity);
      Assert.Equal(expectedConfig.Version, actualConfig.Version);
      Assert.Equal(expectedConfig.Console, actualConfig.Console);

      Assert.Equal(expectedConfig.Window.Title, actualConfig.Window.Title);
      Assert.Equal(expectedConfig.Window.Width, actualConfig.Window.Width);
      Assert.Equal(expectedConfig.Window.Height, actualConfig.Window.Height);
      Assert.Equal(expectedConfig.Window.Fullscreen, actualConfig.Window.Fullscreen);
      Assert.Equal(expectedConfig.Window.VSync, actualConfig.Window.VSync);
      Assert.True(expectedConfig.Window.Borderless == actualConfig.Window.Borderless);

      Assert.Equal(expectedConfig.Audio.MixWithSystem, actualConfig.Audio.MixWithSystem);

      Assert.Equal(expectedConfig.Modules.Graphics, actualConfig.Modules.Graphics);
      Assert.Equal(expectedConfig.Modules.Audio, actualConfig.Modules.Audio);

      // ... add more assertions for other properties as needed
    }

    /// <summary>
    /// Tests that LoadConfig loads the configuration only once, even if called multiple times.
    /// </summary>
    [Fact]
    public void LoadConfig_LoadsOnlyOnce()
    {
      // Arrange
      // Config with a specific window title for the initial load
      var initialWindowConfig = new WindowConfig { Title = "Initial Load Title" };
      var initialGameConfig = new GameConfig { Window = initialWindowConfig };
      string initialJsonContent = JsonSerializer.Serialize(initialGameConfig);
      File.WriteAllText(this.configFilePath, initialJsonContent);

      // Act
      ConfigurationManager.LoadConfig(this.testDirectoryPath); // First load

      // Assert first load
      Assert.True(ConfigurationManager.IsLoaded);
      Assert.Equal(initialGameConfig.Window.Title, ConfigurationManager.CurrentConfig.Window.Title);

      // Arrange for second attempt - change the config file content
      var updatedWindowConfig = new WindowConfig { Title = "Updated Load Attempt Title" };
      var updatedGameConfig = new GameConfig { Window = updatedWindowConfig };
      string updatedJsonContent = JsonSerializer.Serialize(updatedGameConfig);
      File.WriteAllText(this.configFilePath, updatedJsonContent);

      // Act - attempt to load again
      ConfigurationManager.LoadConfig(this.testDirectoryPath);

      // Assert - config should NOT have changed because it's already loaded
      Assert.True(ConfigurationManager.IsLoaded); // Still true
      Assert.Equal(initialGameConfig.Window.Title, ConfigurationManager.CurrentConfig.Window.Title); // Should be from initial load
      Assert.NotEqual(updatedGameConfig.Window.Title, ConfigurationManager.CurrentConfig.Window.Title); // Should not be updated
    }

    /// <summary>
    /// Tests that CurrentConfig returns the default configuration when LoadConfig has not been called.
    /// </summary>
    [Fact]
    public void CurrentConfig_ReturnsDefaultWhenNotLoaded()
    {
      // Arrange (Reset ensures it's not loaded in constructor)

      // Act
      var config = ConfigurationManager.CurrentConfig;

      // Assert
      Assert.False(ConfigurationManager.IsLoaded);
      Assert.NotNull(config);
      Assert.Equal("Night Game", config.Window.Title); // Default value from WindowConfig
      Assert.Equal(800, config.Window.Width); // Default value from WindowConfig
    }

    /// <summary>
    /// Tests that IsLoaded is false before LoadConfig is called.
    /// </summary>
    [Fact]
    public void IsLoaded_IsFalseInitially()
    {
      // Arrange (Reset ensures it's not loaded)
      // ResetConfigurationManager(); // Done in constructor

      // Assert
      Assert.False(ConfigurationManager.IsLoaded);
    }

    // Helper to reset the static state of ConfigurationManager for isolated tests.
    // This is done via reflection as there's no public reset method.
    private static void ResetConfigurationManager()
    {
      FieldInfo? isLoadedField = typeof(ConfigurationManager).GetField("isLoaded", BindingFlags.NonPublic | BindingFlags.Static);
      FieldInfo? currentConfigField = typeof(ConfigurationManager).GetField("currentConfig", BindingFlags.NonPublic | BindingFlags.Static);

      if (isLoadedField != null)
      {
        isLoadedField.SetValue(null, false);
      }
      else
      {
        throw new InvalidOperationException("Could not find 'isLoaded' field in ConfigurationManager.");
      }

      if (currentConfigField != null)
      {
        currentConfigField.SetValue(null, new GameConfig()); // Reset to default config
      }
      else
      {
        throw new InvalidOperationException("Could not find 'currentConfig' field in ConfigurationManager.");
      }
    }
  }
}
