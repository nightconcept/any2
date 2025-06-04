// <copyright file="CommandLineProcessor.cs" company="Night Circle">
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
using System.Globalization;
using System.IO;
using System.Linq;

using Night.Log;

namespace Night.Engine
{
  /// <summary>
  /// Applies settings based on parsed command-line arguments.
  /// </summary>
  public static class CommandLineProcessor
  {
    /// <summary>
    /// Applies logging and other settings based on the provided parsed CLI arguments.
    /// </summary>
    /// <param name="cliArgs">The parsed command-line arguments.</param>
    public static void ApplySettings(CLI cliArgs)
    {
      if (cliArgs == null)
      {
        // This should ideally not happen if Program.cs always passes a new CLI(args)
        // but as a safeguard:
        Console.WriteLine("[Night.Engine.CommandLineProcessor] Warning: cliArgs object was null. Cannot apply CLI settings.");
        return;
      }

      // Apply settings based on parsed CLI arguments
      if (cliArgs.ParsedLogLevel.HasValue)
      {
        LogManager.MinLevel = cliArgs.ParsedLogLevel.Value;
        if (!cliArgs.IsSilentMode)
        {
          Console.WriteLine($"[Night.Engine] Log level set to: {cliArgs.ParsedLogLevel.Value}");
        }
      }

      if (cliArgs.IsDebugMode)
      {
        LogManager.MinLevel = LogLevel.Debug; // Ensure debug level if --debug is set
        LogManager.EnableSystemConsoleSink(true);
        if (!cliArgs.IsSilentMode)
        {
          Console.WriteLine("[Night.Engine] Debug mode enabled: Log level set to Debug, console sink enabled.");
        }
      }

      if (cliArgs.EnableSessionLog)
      {
        try
        {
          string baseDirectory = AppContext.BaseDirectory ?? ".";
          string sessionDirPath = Path.Combine(baseDirectory, "session");
          _ = Directory.CreateDirectory(sessionDirPath);

          string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss", CultureInfo.InvariantCulture);
          string logFileName = $"session_log_{timestamp}.log";
          string logFilePath = Path.Combine(sessionDirPath, logFileName);

          LogManager.ConfigureFileSink(logFilePath, LogLevel.Trace); // FileSink itself will capture all from Trace, LogManager.MinLevel filters what's sent
          if (!cliArgs.IsSilentMode)
          {
            Console.WriteLine($"[Night.Engine] Session log enabled. Logging to: {logFilePath}");
          }
        }
        catch (Exception ex)
        {
          if (!cliArgs.IsSilentMode)
          {
            Console.WriteLine($"[Night.Engine] Error enabling session log: {ex.Message}");
          }
        }
      }

      // Handle any remaining arguments that were not processed by CLI.cs
      if (cliArgs.RemainingArgs.Any())
      {
        if (!cliArgs.IsSilentMode)
        {
          Console.WriteLine($"[Night.Engine] Warning: Unprocessed or invalid arguments found: {string.Join(" ", cliArgs.RemainingArgs)}");
          if (cliArgs.RemainingArgs.Contains("--log-level", StringComparer.OrdinalIgnoreCase))
          {
            bool valueMissing = true;
            int logLevelIndex = cliArgs.RemainingArgs.ToList().FindIndex(x => x.Equals("--log-level", StringComparison.OrdinalIgnoreCase));
            if (logLevelIndex != -1 && logLevelIndex + 1 < cliArgs.RemainingArgs.Count)
            {
              if (!cliArgs.RemainingArgs[logLevelIndex + 1].StartsWith("--"))
              {
                Console.WriteLine($"[Night.Engine] Warning: The value '{cliArgs.RemainingArgs[logLevelIndex + 1]}' provided for --log-level is invalid. Using current default.");
                valueMissing = false;
              }
            }

            if (valueMissing)
            {
              Console.WriteLine("[Night.Engine] Warning: --log-level option requires a valid level argument (Trace, Debug, Information, Warning, Error, Fatal).");
            }
          }
        }
      }
    }
  }
}
