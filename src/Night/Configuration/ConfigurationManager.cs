// <copyright file="ConfigurationManager.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System;
using System.IO;
using System.Text.Json;

namespace Night
{
  using Night;
  using Night.Configuration;

  public static class ConfigurationManager
  {
    private static readonly string ConfigFileName = "config.json";
    private static Night.Configuration.GameConfig currentConfig = new Night.Configuration.GameConfig();
    private static bool isLoaded = false;

    public static Night.Configuration.GameConfig CurrentConfig => currentConfig;

    public static bool IsLoaded => isLoaded;

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
            Night.Configuration.GameConfig? loadedConfig = JsonSerializer.Deserialize<Night.Configuration.GameConfig>(jsonContent, options);
            if (loadedConfig != null)
            {
              currentConfig = loadedConfig;
              Console.WriteLine($"Info: Successfully loaded '{ConfigFileName}' from '{configFilePath}'.");
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
        catch (Exception ex) // Catches IOExceptions, SecurityExceptions, etc.
        {
          Console.WriteLine($"Error loading '{ConfigFileName}' from '{configFilePath}': {ex.Message}. Using default configuration.");
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
