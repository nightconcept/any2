// <copyright file="ConfigurationManager.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System;
using System.IO;
using System.Text.Json;

using Night;

namespace Night
{
  /// <summary>
  /// Manages the loading and accessing of game configuration settings.
  /// </summary>
  public static class ConfigurationManager
  {
    private static readonly string ConfigFileName = "config.json";
    private static GameConfig currentConfig = new GameConfig();
    private static bool isLoaded = false;

    /// <summary>
    /// Gets the currently loaded game configuration. If no configuration has been loaded, returns the default configuration.
    /// </summary>
    public static GameConfig CurrentConfig => currentConfig;

    /// <summary>
    /// Gets a value indicating whether the configuration has been loaded.
    /// </summary>
    public static bool IsLoaded => isLoaded;

    /// <summary>
    /// Loads the game configuration from a 'config.json' file.
    /// If the file is not found, or if an error occurs during loading or parsing, default settings are used.
    /// The configuration is loaded only once; subsequent calls will not reload the configuration.
    /// </summary>
    /// <param name="gameDirectory">The directory to search for 'config.json'. If null, the application's base directory is used.</param>
    public static void LoadConfig(string? gameDirectory = null)
    {
      if (isLoaded)
      {
        return;
      }

      string effectiveGameDirectory = gameDirectory ?? AppContext.BaseDirectory;
      string configFilePath = Path.Combine(effectiveGameDirectory, ConfigFileName);

      if (File.Exists(configFilePath))
      {
        try
        {
          string jsonContent = File.ReadAllText(configFilePath);
          if (!string.IsNullOrEmpty(jsonContent))
          {
            var options = new JsonSerializerOptions
            {
              PropertyNameCaseInsensitive = true,
              AllowTrailingCommas = true,
              ReadCommentHandling = JsonCommentHandling.Skip,
            };
            GameConfig? loadedConfig = JsonSerializer.Deserialize<GameConfig>(jsonContent, options);
            if (loadedConfig != null)
            {
              currentConfig = loadedConfig;
            }
            else
            {
              Console.WriteLine($"Warning: Could not parse '{ConfigFileName}' from '{configFilePath}'. Using default configuration.");
            }
          }
          else
          {
            Console.WriteLine($"Warning: '{ConfigFileName}' found at '{configFilePath}' is empty. Using default configuration.");
          }
        }
        catch (JsonException jsonEx)
        {
          Console.WriteLine($"Error deserializing '{ConfigFileName}' from '{configFilePath}': {jsonEx.Message}. Using default configuration.");
        }

        // Catch-all for other potential issues
        catch (Exception ex)
        {
          Console.WriteLine($"Night.ConfigurationManager: Error loading or deserializing config.json: {ex.Message}. Using default configuration.");
        }
      }
      else
      {
        Console.WriteLine($"Info: '{ConfigFileName}' not found at '{configFilePath}'. Using default configuration.");
      }

      isLoaded = true;
    }
  }
}
