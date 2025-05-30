// <copyright file="WindowConfig.cs" company="Night Circle">
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

using System.Text.Json.Serialization;

namespace Night
{
  /// <summary>
  /// Configuration settings for the game window.
  /// </summary>
  public class WindowConfig
  {
    /// <summary>
    /// Gets or sets the title of the window.
    /// </summary>
    [JsonPropertyName("title")]
    public string Title { get; set; } = "Untitled";

    /// <summary>
    /// Gets or sets the path to the window icon image file. Null for no icon.
    /// </summary>
    [JsonPropertyName("icon")]
    public string? Icon { get; set; } = null;

    /// <summary>
    /// Gets or sets the initial width of the window in pixels.
    /// </summary>
    [JsonPropertyName("width")]
    public int Width { get; set; } = 800;

    /// <summary>
    /// Gets or sets the initial height of the window in pixels.
    /// </summary>
    [JsonPropertyName("height")]
    public int Height { get; set; } = 600;

    /// <summary>
    /// Gets or sets a value indicating whether the window should be borderless.
    /// </summary>
    [JsonPropertyName("borderless")]
    public bool Borderless { get; set; } = false;

    /// <summary>
    /// Gets or sets a value indicating whether the window should be resizable.
    /// </summary>
    [JsonPropertyName("resizable")]
    public bool Resizable { get; set; } = false;

    /// <summary>
    /// Gets or sets the minimum width of the window if resizable.
    /// </summary>
    [JsonPropertyName("minwidth")]
    public int MinWidth { get; set; } = 1;

    /// <summary>
    /// Gets or sets the minimum height of the window if resizable.
    /// </summary>
    [JsonPropertyName("minheight")]
    public int MinHeight { get; set; } = 1;

    /// <summary>
    /// Gets or sets a value indicating whether the window should start in fullscreen mode.
    /// </summary>
    [JsonPropertyName("fullscreen")]
    public bool Fullscreen { get; set; } = false;

    /// <summary>
    /// Gets or sets the type of fullscreen mode ("desktop" or "exclusive").
    /// </summary>
    [JsonPropertyName("fullscreentype")]
    public string FullscreenType { get; set; } = "desktop"; // "desktop" or "exclusive"

    /// <summary>
    /// Gets or sets the VSync mode. -1 for adaptive, 0 for disabled, 1 for enabled.
    /// </summary>
    [JsonPropertyName("vsync")]
    public int VSync { get; set; } = 1; // -1 adaptive, 0 disabled, 1 enabled

    /// <summary>
    /// Gets or sets the multisample anti-aliasing (MSAA) level.
    /// </summary>
    [JsonPropertyName("msaa")]
    public int MSAA { get; set; } = 0;

    /// <summary>
    /// Gets or sets the depth buffer bits. Null for system default.
    /// </summary>
    [JsonPropertyName("depth")]
    public int? Depth { get; set; } = null;

    /// <summary>
    /// Gets or sets the stencil buffer bits. Null for system default.
    /// </summary>
    [JsonPropertyName("stencil")]
    public int? Stencil { get; set; } = null;

    /// <summary>
    /// Gets or sets the 1-indexed display number to use for the window.
    /// </summary>
    [JsonPropertyName("display")]
    public int Display { get; set; } = 1; // 1-indexed

    /// <summary>
    /// Gets or sets a value indicating whether to enable High DPI mode if available.
    /// </summary>
    [JsonPropertyName("highdpi")]
    public bool HighDPI { get; set; } = false;

    /// <summary>
    /// Gets or sets a value indicating whether to use DPI scaling.
    /// </summary>
    [JsonPropertyName("usedpiscale")]
    public bool UseDPIScale { get; set; } = true;

    /// <summary>
    /// Gets or sets the initial X position of the window. Null for centered.
    /// </summary>
    [JsonPropertyName("x")]
    public int? X { get; set; } = null;

    /// <summary>
    /// Gets or sets the initial Y position of the window. Null for centered.
    /// </summary>
    [JsonPropertyName("y")]
    public int? Y { get; set; } = null;
  }
}
