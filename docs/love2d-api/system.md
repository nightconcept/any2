# `love.system` Module API Mapping

This document maps the functions available in the `love.system` module of Love2D to their proposed equivalents in the Night Engine. Most functions in this module are **Out of Scope** for the initial prototype. Standard .NET `System.Environment` or `System.Runtime.InteropServices.RuntimeInformation` can provide some of this.

| Love2D Function (`love.system.`) | Night Engine API (`Night.System` or `System` namespace) | Notes / C# Signature Idea | Status (Prototype Scope) | Done |
|----------------------------------|---------------------------------------------------------|---------------------------|--------------------------|------|
| `love.system.getOS()`            | `Night.System.GetOSName()` or `System.Runtime.InteropServices.RuntimeInformation.OSDescription` | `public static string GetOSName()` | Out of Scope | [ ] |
| `love.system.getProcessorCount()`| `System.Environment.ProcessorCount` | `public static int GetProcessorCount()` (via `System.Environment`) | Out of Scope | [ ] |
| `love.system.getPowerInfo()`     | `Night.System.GetPowerInfo()`     | `public static Night.PowerInfo GetPowerInfo()` <br> `PowerInfo` class: `State` (enum), `SecondsLeft` (nullable int), `Percent` (nullable int). | Out of Scope | [ ] |
| `love.system.getClipboardText()` | `Night.System.GetClipboardText()` | `public static string GetClipboardText()` <br> Would need platform-specific implementation or a library. | Out of Scope | [ ] |
| `love.system.setClipboardText(text)` | `Night.System.SetClipboardText(string text)` | `public static void SetClipboardText(string text)` | Out of Scope | [ ] |
| `love.system.openURL(url)`       | `Night.System.OpenURL(string url)` or `System.Diagnostics.Process.Start()` | `public static bool OpenURL(string url)` <br> `Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });` | Out of Scope | [ ] |
| `love.system.vibrate(seconds)`   | `Night.System.Vibrate(double seconds)` | `public static void Vibrate(double seconds)` <br> For mobile devices. | Out of Scope | [ ] |
| `love.system.getPreferredLocales()` | `Night.System.GetPreferredLocales()` | `public static string[] GetPreferredLocales()` <br> From `System.Globalization.CultureInfo.CurrentUICulture` etc. | Out of Scope | [ ] |

**Night Engine Specific Types (if module were implemented):**
*   `Night.PowerInfo`: Class/Struct with properties `State` (enum: `NoBattery`, `Charging`, `Charged`, `Draining`), `SecondsLeft` (nullable int), `Percent` (nullable int).
