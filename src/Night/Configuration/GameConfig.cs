// <copyright file="GameConfig.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System.Text.Json.Serialization;

namespace Night
{
  /// <summary>
  /// Represents the overall game configuration settings, typically loaded from a config.json file.
  /// </summary>
  public class GameConfig
  {
    /// <summary>
    /// Gets or sets the identity of the game. This is used for the save directory.
    /// </summary>
    [JsonPropertyName("identity")]
    public string? Identity { get; set; } = null;

    /// <summary>
    /// Gets or sets a value indicating whether the game identity should be appended to the save directory path.
    /// </summary>
    [JsonPropertyName("appendidentity")]
    public bool AppendIdentity { get; set; } = false;

    /// <summary>
    /// Gets or sets the LÖVE version this game targets. Currently informational.
    /// </summary>
    [JsonPropertyName("version")]
    public string Version { get; set; } = "11.4"; // Default to LÖVE 11.4

    /// <summary>
    /// Gets or sets a value indicating whether a console window should be attached (Windows only, currently placeholder).
    /// </summary>
    [JsonPropertyName("console")]
    public bool Console { get; set; } = false;

    /// <summary>
    /// Gets or sets a value indicating whether the accelerometer should be used as a joystick.
    /// </summary>
    [JsonPropertyName("accelerometerjoystick")]
    public bool AccelerometerJoystick { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether to request external storage access (Android only, currently placeholder).
    /// </summary>
    [JsonPropertyName("externalstorage")]
    public bool ExternalStorage { get; set; } = false;

    /// <summary>
    /// Gets or sets a value indicating whether gamma correction should be enabled.
    /// </summary>
    [JsonPropertyName("gammacorrect")]
    public bool GammaCorrect { get; set; } = false;

    /// <summary>
    /// Gets or sets the audio module configuration.
    /// </summary>
    [JsonPropertyName("audio")]
    public AudioConfig Audio { get; set; } = new AudioConfig();

    /// <summary>
    /// Gets or sets the window module configuration.
    /// </summary>
    [JsonPropertyName("window")]
    public WindowConfig Window { get; set; } = new WindowConfig();

    /// <summary>
    /// Gets or sets the configuration for enabling/disabling engine modules.
    /// </summary>
    [JsonPropertyName("modules")]
    public ModulesConfig Modules { get; set; } = new ModulesConfig();
  }
}
