// <copyright file="Framework.Events.cs" company="Night Circle">
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
// using Night.Joysticks; // This was incorrect as Joysticks is a static class, types are in Night namespace.
using SDL3;

namespace Night
{
  public static partial class Framework
  {
    private static void ProcessSdlEvents(IGame game)
    {
      while (SDL.PollEvent(out SDL.Event e) && !inErrorState)
      {
        var eventType = (SDL.EventType)e.Type;
        Logger.Debug($"SDL Event polled: {eventType}");
        if (eventType == SDL.EventType.Quit)
        {
          Logger.Info("SDL_QUIT event received. Closing window.");
          Window.Close();
        }
        else if (eventType == SDL.EventType.KeyDown)
        {
          try
          {
            game.KeyPressed((KeySymbol)e.Key.Key, (KeyCode)e.Key.Scancode, e.Key.Repeat);
          }
          catch (Exception exUser)
          {
            HandleGameException(exUser, game);
          }
        }
        else if (eventType == SDL.EventType.KeyUp)
        {
          try
          {
            game.KeyReleased((KeySymbol)e.Key.Key, (KeyCode)e.Key.Scancode);
          }
          catch (Exception exUser)
          {
            HandleGameException(exUser, game);
          }
        }
        else if (eventType == SDL.EventType.MouseButtonDown)
        {
          try
          {
            game.MousePressed((int)e.Button.X, (int)e.Button.Y, (MouseButton)e.Button.Button, e.Button.Which == SDL.TouchMouseID, e.Button.Clicks);
          }
          catch (Exception exUser)
          {
            HandleGameException(exUser, game);
          }
        }
        else if (eventType == SDL.EventType.MouseButtonUp)
        {
          try
          {
            game.MouseReleased((int)e.Button.X, (int)e.Button.Y, (MouseButton)e.Button.Button, e.Button.Which == SDL.TouchMouseID, e.Button.Clicks);
          }
          catch (Exception exUser)
          {
            HandleGameException(exUser, game);
          }
        }
        // Joystick and Gamepad event handling will be added here in later phases
      }
    }
  }
}