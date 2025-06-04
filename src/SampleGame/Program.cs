// <copyright file="Program.cs" company="Night Circle">
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

using Night;
using Night.Log;

namespace SampleGame
{
  /// <summary>
  /// Main program class for the SampleGame.
  /// Contains the entry point of the application.
  /// </summary>
  public class Program
  {
    /// <summary>
    /// The main entry point for the application.
    /// Initializes and runs the game using the Night.Framework.
    /// </summary>
    /// <param name="args">Command-line arguments.</param>
    public static void Main(string[] args)
    {
      // Default log level can be set here if desired, e.g., LogManager.MinLevel = LogLevel.Information;
      // By default, it's Information as per LogManager.
      for (int i = 0; i < args.Length; i++)
      {
        string arg = args[i];

        if (string.Equals(arg, "--log-level", StringComparison.OrdinalIgnoreCase))
        {
          if (i + 1 < args.Length)
          {
            i++; // Consume the level value
            string levelString = args[i];
            if (Enum.TryParse(levelString, true, out LogLevel parsedLevel))
            {
              LogManager.MinLevel = parsedLevel;
              Console.WriteLine($"[SampleGame] Log level set to: {parsedLevel}"); // Early feedback
            }
            else
            {
              Console.WriteLine($"[SampleGame] Warning: Invalid log level '{levelString}'. Using current default.");
            }
          }
          else
          {
            Console.WriteLine("[SampleGame] Warning: --log-level option requires a level argument (Trace, Debug, Information, Warning, Error, Fatal).");
          }
        }
        else if (string.Equals(arg, "--debug", StringComparison.OrdinalIgnoreCase))
        {
          LogManager.MinLevel = LogLevel.Debug;
          LogManager.EnableSystemConsoleSink(true);
          Console.WriteLine("[SampleGame] Debug mode enabled: Log level set to Debug, console sink enabled."); // Early feedback
        }
        else if (string.Equals(arg, "--session-log", StringComparison.OrdinalIgnoreCase))
        {
          try
          {
            string baseDirectory = AppContext.BaseDirectory ?? "."; // Fallback to current dir if null
            string sessionDirPath = Path.Combine(baseDirectory, "session");
            _ = Directory.CreateDirectory(sessionDirPath); // Ensures the directory exists

            string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss", CultureInfo.InvariantCulture);
            string logFileName = $"session_log_{timestamp}.log";
            string logFilePath = Path.Combine(sessionDirPath, logFileName);

            // Configure file sink to capture all trace levels and above.
            // The actual filtering will be done by LogManager.MinLevel.
            LogManager.ConfigureFileSink(logFilePath, LogLevel.Trace);
            Console.WriteLine($"[SampleGame] Session log enabled. Logging to: {logFilePath}"); // Early feedback
          }
          catch (Exception ex)
          {
            Console.WriteLine($"[SampleGame] Error enabling session log: {ex.Message}");
          }
        }
      }

      Framework.Run(new Game());
    }
  }
}
