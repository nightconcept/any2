// <copyright file="GameConfig.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System.Text.Json.Serialization;

namespace Night
{

  public class GameConfig
  {
    [JsonPropertyName("identity")]
    public string? Identity { get; set; } = null;

    [JsonPropertyName("appendidentity")]
    public bool AppendIdentity { get; set; } = false;

    [JsonPropertyName("version")]
    public string Version { get; set; } = "11.4"; // Default to LÖVE 11.4

    [JsonPropertyName("console")]
    public bool Console { get; set; } = false;

    [JsonPropertyName("accelerometerjoystick")]
    public bool AccelerometerJoystick { get; set; } = true;

    [JsonPropertyName("externalstorage")]
    public bool ExternalStorage { get; set; } = false;

    [JsonPropertyName("gammacorrect")]
    public bool GammaCorrect { get; set; } = false;

    [JsonPropertyName("audio")]
    public AudioConfig Audio { get; set; } = new AudioConfig();

    [JsonPropertyName("window")]
    public WindowConfig Window { get; set; } = new WindowConfig();

    [JsonPropertyName("modules")]
    public ModulesConfig Modules { get; set; } = new ModulesConfig();
  }

  public class AudioConfig
  {
    [JsonPropertyName("mic")]
    public bool Mic { get; set; } = false;

    [JsonPropertyName("mixwithsystem")]
    public bool MixWithSystem { get; set; } = true;
  }

  public class WindowConfig
  {
    [JsonPropertyName("title")]
    public string Title { get; set; } = "Untitled";

    [JsonPropertyName("icon")]
    public string? Icon { get; set; } = null;

    [JsonPropertyName("width")]
    public int Width { get; set; } = 800;

    [JsonPropertyName("height")]
    public int Height { get; set; } = 600;

    [JsonPropertyName("borderless")]
    public bool Borderless { get; set; } = false;

    [JsonPropertyName("resizable")]
    public bool Resizable { get; set; } = false;

    [JsonPropertyName("minwidth")]
    public int MinWidth { get; set; } = 1;

    [JsonPropertyName("minheight")]
    public int MinHeight { get; set; } = 1;

    [JsonPropertyName("fullscreen")]
    public bool Fullscreen { get; set; } = false;

    [JsonPropertyName("fullscreentype")]
    public string FullscreenType { get; set; } = "desktop"; // "desktop" or "exclusive"

    [JsonPropertyName("vsync")]
    public int VSync { get; set; } = 1; // -1 adaptive, 0 disabled, 1 enabled

    [JsonPropertyName("msaa")]
    public int MSAA { get; set; } = 0;

    [JsonPropertyName("depth")]
    public int? Depth { get; set; } = null;

    [JsonPropertyName("stencil")]
    public int? Stencil { get; set; } = null;

    [JsonPropertyName("display")]
    public int Display { get; set; } = 1; // 1-indexed

    [JsonPropertyName("highdpi")]
    public bool HighDPI { get; set; } = false;

    [JsonPropertyName("usedpiscale")]
    public bool UseDPIScale { get; set; } = true;

    [JsonPropertyName("x")]
    public int? X { get; set; } = null;

    [JsonPropertyName("y")]
    public int? Y { get; set; } = null;
  }

  public class ModulesConfig
  {
    // TODO: Implement actual module enabling/disabling based on these flags.
    // For now, they are just placeholders. love.filesystem, love.data, and love (Night core) are mandatory.
    // love.graphics needs love.window.
    [JsonPropertyName("audio")]
    public bool Audio { get; set; } = true;

    [JsonPropertyName("data")]
    public bool Data { get; set; } = true; // Mandatory in LÖVE

    [JsonPropertyName("event")]
    public bool Event { get; set; } = true;

    [JsonPropertyName("font")]
    public bool Font { get; set; } = true;

    [JsonPropertyName("graphics")]
    public bool Graphics { get; set; } = true; // Needs Window

    [JsonPropertyName("image")]
    public bool Image { get; set; } = true;

    [JsonPropertyName("joystick")]
    public bool Joystick { get; set; } = true;

    [JsonPropertyName("keyboard")]
    public bool Keyboard { get; set; } = true;

    [JsonPropertyName("math")]
    public bool Math { get; set; } = true;

    [JsonPropertyName("mouse")]
    public bool Mouse { get; set; } = true;

    [JsonPropertyName("physics")]
    public bool Physics { get; set; } = true;

    [JsonPropertyName("sound")]
    public bool Sound { get; set; } = true;

    [JsonPropertyName("system")]
    public bool System { get; set; } = true;

    [JsonPropertyName("thread")]
    public bool Thread { get; set; } = true;

    [JsonPropertyName("timer")]
    public bool Timer { get; set; } = true;

    [JsonPropertyName("touch")]
    public bool Touch { get; set; } = true;

    [JsonPropertyName("video")]
    public bool Video { get; set; } = true;

    [JsonPropertyName("window")]
    public bool Window { get; set; } = true; // Mandatory for Graphics
  }
}
