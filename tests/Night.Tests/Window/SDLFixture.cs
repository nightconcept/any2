// <copyright file="SDLFixture.cs" company="Night Circle">
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

using SDL3;

namespace Night.Tests.Window
{
  /// <summary>
  /// Fixture for managing SDL initialization and shutdown for test classes.
  /// </summary>
  public class SDLFixture : IDisposable
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="SDLFixture"/> class.
    /// Initializes the SDL video subsystem.
    /// </summary>
    public SDLFixture()
    {
      if (!SDL3.SDL.Init(SDL3.SDL.InitFlags.Video))
      {
        string sdlError = SDL3.SDL.GetError();
        throw new InvalidOperationException($"SDL_Init(Video) failed: {sdlError}");
      }
    }

    /// <summary>
    /// Cleans up resources by quitting SDL.
    /// </summary>
    public void Dispose()
    {
      SDL3.SDL.Quit();
      GC.SuppressFinalize(this);
    }
  }
}
