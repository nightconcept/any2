// <copyright file="AudioConfig.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System.Text.Json.Serialization;

namespace Night
{
  /// <summary>
  /// Configuration settings for the audio module.
  /// </summary>
  public class AudioConfig
  {
    /// <summary>
    /// Gets or sets a value indicating whether the microphone should be enabled.
    /// </summary>
    [JsonPropertyName("mic")]
    public bool Mic { get; set; } = false;

    /// <summary>
    /// Gets or sets a value indicating whether the game's audio should mix with system audio.
    /// </summary>
    [JsonPropertyName("mixwithsystem")]
    public bool MixWithSystem { get; set; } = true;
  }
}
