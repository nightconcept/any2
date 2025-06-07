// <copyright file="WindowConfigTest.cs" company="Night Circle">
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
using System.IO;

using Night;

using NightTest.Core;

using Xunit;

namespace NightTest.Groups.Configuration
{
  /// <summary>
  /// Tests for Night.Configuration.WindowConfig.
  /// </summary>
  public class ConfigurationWindowConfig_GetSet : ModTestCase
  {
    /// <inheritdoc/>
    public override string Name => "Configuration.WindowConfig";

    /// <inheritdoc/>
    public override string Description => "Tests getters and setters.";

    /// <inheritdoc/>
    public override string SuccessMessage => "WindowConfig getters and setters passed successfully.";

    /// <inheritdoc/>
    public override void Run()
    {
      WindowConfig config = new WindowConfig();
      config.Title = "Test Window";
      Assert.Equal("Test Window", config.Title);

      config.IconPath = "icon.png";
      Assert.Equal("icon.png", config.IconPath);

      config.Width = 1024;
      Assert.Equal(1024, config.Width);

      config.Height = 768;
      Assert.Equal(768, config.Height);

      config.X = 100;
      Assert.Equal(100, config.X);

      config.Y = 200;
      Assert.Equal(200, config.Y);

      config.MinWidth = 800;
      Assert.Equal(800, config.MinWidth);

      config.MinHeight = 600;
      Assert.Equal(600, config.MinHeight);

      config.Resizable = true;
      Assert.True(config.Resizable);

      config.Borderless = true;
      Assert.True(config.Borderless);

      config.Fullscreen = true;
      Assert.True(config.Fullscreen);

      config.FullscreenType = "exclusive";
      Assert.Equal("exclusive", config.FullscreenType);

      config.VSync = true;
      Assert.True(config.VSync);

      config.HighDPI = true;
      Assert.True(config.HighDPI);

      config.MSAA = 4;
      Assert.Equal(4, config.MSAA);

      config.Depth = 24;
      Assert.Equal(24, config.Depth);

      config.Stencil = 8;
      Assert.Equal(8, config.Stencil);

      config.Display = 0;
      Assert.Equal(0, config.Display);

      config.UseDPIScale = false;
      Assert.False(config.UseDPIScale);
    }
  }
}
