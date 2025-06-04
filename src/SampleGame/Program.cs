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

using Night;
using Night.Engine;

// using Night.Log; // No longer directly used here
// using System.Globalization; // No longer directly used here
// using System.IO; // No longer directly used here
// using System.Linq; // No longer directly used here (was implicit via .Any())
// using System; // Console.WriteLine was removed
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
      // All CLI argument parsing and application of settings (like logging)
      // is now handled within Night.Engine.CLI and Night.Engine.CommandLineProcessor,
      // which is called by Framework.Run().
      Framework.Run(new Game(), new CLI(args));
    }
  }
}
