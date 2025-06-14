// <copyright file="GraphicsBackgroundColorTests.cs" company="Night Circle">
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

using Night;

using NightTest.Core;

namespace NightTest.Groups.Graphics
{
  /// <summary>
  /// Tests the default background color retrieval.
  /// </summary>
  public class GraphicsGetBackgroundColor_DefaultTest : GameTestCase
  {
    /// <inheritdoc/>
    public override string Name => "Graphics.GetBackgroundColor.Default";

    /// <inheritdoc/>
    public override string Description => "Tests that GetBackgroundColor returns the default color (black) if Clear has not been called.";

    private (float r, float g, float b, float a) defaultColor;

    /// <inheritdoc/>
    protected override void Load()
    {
      base.Load();
      this.Details = "Getting default background color.";
      try
      {
        this.defaultColor = Night.Graphics.GetBackgroundColor();
      }
      catch (Exception e)
      {
        this.RecordFailure($"Exception during GetBackgroundColor: {e.Message}", e);
      }
    }

    /// <inheritdoc/>
    protected override void Update(double deltaTime)
    {
      if (this.IsDone)
      {
        return;
      }

      if (this.CurrentStatus == TestStatus.Failed)
      {
        this.EndTest();
        return;
      }

      // Default color is black (0,0,0,1)
      bool rMatch = Math.Abs(this.defaultColor.r - 0.0f) < 0.001f;
      bool gMatch = Math.Abs(this.defaultColor.g - 0.0f) < 0.001f;
      bool bMatch = Math.Abs(this.defaultColor.b - 0.0f) < 0.001f;
      bool aMatch = Math.Abs(this.defaultColor.a - 1.0f) < 0.001f; // Alpha should be 1 for opaque black

      if (rMatch && gMatch && bMatch && aMatch)
      {
        this.CurrentStatus = TestStatus.Passed;
        this.Details = $"Default background color is correct (R={this.defaultColor.r}, G={this.defaultColor.g}, B={this.defaultColor.b}, A={this.defaultColor.a}).";
      }
      else
      {
        this.CurrentStatus = TestStatus.Failed;
        this.Details = $"Default background color is incorrect. Expected (0,0,0,1), Got (R={this.defaultColor.r}, G={this.defaultColor.g}, B={this.defaultColor.b}, A={this.defaultColor.a}).";
      }

      this.EndTest();
    }
  }

  /// <summary>
  /// Tests background color retrieval after calling Graphics.Clear().
  /// </summary>
  public class GraphicsGetBackgroundColor_AfterClearTest : GameTestCase
  {
    /// <inheritdoc/>
    public override string Name => "Graphics.GetBackgroundColor.AfterClear";

    /// <inheritdoc/>
    public override string Description => "Tests that GetBackgroundColor returns the correct color after Graphics.Clear() is called.";

    private readonly Color testColorByte = new(51, 102, 153, 204); // R=0.2, G=0.4, B=0.6, A=0.8
    private (float r, float g, float b, float a) retrievedColor;

    /// <inheritdoc/>
    protected override void Load()
    {
      base.Load();
      this.Details = $"Clearing screen with color ({this.testColorByte.R}, {this.testColorByte.G}, {this.testColorByte.B}, {this.testColorByte.A}) and getting background color.";
      try
      {
        Night.Graphics.Clear(this.testColorByte);
        this.retrievedColor = Night.Graphics.GetBackgroundColor();
      }
      catch (Exception e)
      {
        this.RecordFailure($"Exception during test setup: {e.Message}", e);
      }
    }

    /// <inheritdoc/>
    protected override void Update(double deltaTime)
    {
      if (this.IsDone)
      {
        return;
      }

      if (this.CurrentStatus == TestStatus.Failed)
      {
        this.EndTest();
        return;
      }

      float expectedR = this.testColorByte.R / 255.0f;
      float expectedG = this.testColorByte.G / 255.0f;
      float expectedB = this.testColorByte.B / 255.0f;
      float expectedA = this.testColorByte.A / 255.0f;

      bool rMatch = Math.Abs(this.retrievedColor.r - expectedR) < 0.001f;
      bool gMatch = Math.Abs(this.retrievedColor.g - expectedG) < 0.001f;
      bool bMatch = Math.Abs(this.retrievedColor.b - expectedB) < 0.001f;
      bool aMatch = Math.Abs(this.retrievedColor.a - expectedA) < 0.001f;

      if (rMatch && gMatch && bMatch && aMatch)
      {
        this.CurrentStatus = TestStatus.Passed;
        this.Details = $"Retrieved background color is correct (R={this.retrievedColor.r}, G={this.retrievedColor.g}, B={this.retrievedColor.b}, A={this.retrievedColor.a}).";
      }
      else
      {
        this.CurrentStatus = TestStatus.Failed;
        this.Details = $"Retrieved background color is incorrect. Expected (R={expectedR}, G={expectedG}, B={expectedB}, A={expectedA}), Got (R={this.retrievedColor.r}, G={this.retrievedColor.g}, B={this.retrievedColor.b}, A={this.retrievedColor.a}).";
      }

      this.EndTest();
    }
  }
}
