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
    private (float R, float G, float B, float A) defaultColor;

    /// <inheritdoc/>
    public override string Name => "Graphics.GetBackgroundColor.Default";

    /// <inheritdoc/>
    public override string Description => "Tests that GetBackgroundColor returns the default color (black) if Clear has not been called.";

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
      bool rMatch = Math.Abs(this.defaultColor.R - 0.0f) < 0.001f;
      bool gMatch = Math.Abs(this.defaultColor.G - 0.0f) < 0.001f;
      bool bMatch = Math.Abs(this.defaultColor.B - 0.0f) < 0.001f;
      bool aMatch = Math.Abs(this.defaultColor.A - 1.0f) < 0.001f; // Alpha should be 1 for opaque black

      if (rMatch && gMatch && bMatch && aMatch)
      {
        this.CurrentStatus = TestStatus.Passed;
        this.Details = $"Default background color is correct (R={this.defaultColor.R}, G={this.defaultColor.G}, B={this.defaultColor.B}, A={this.defaultColor.A}).";
      }
      else
      {
        this.CurrentStatus = TestStatus.Failed;
        this.Details = $"Default background color is incorrect. Expected (0,0,0,1), Got (R={this.defaultColor.R}, G={this.defaultColor.G}, B={this.defaultColor.B}, A={this.defaultColor.A}).";
      }

      this.EndTest();
    }
  }

  /// <summary>
  /// Tests background color retrieval after calling Graphics.Clear().
  /// </summary>
  public class GraphicsGetBackgroundColor_AfterClearTest : GameTestCase
  {
    private readonly Color testColorByte = new(51, 102, 153, 204); // R=0.2, G=0.4, B=0.6, A=0.8
    private (float R, float G, float B, float A) retrievedColor;

    /// <inheritdoc/>
    public override string Name => "Graphics.GetBackgroundColor.AfterClear";

    /// <inheritdoc/>
    public override string Description => "Tests that GetBackgroundColor returns the correct color after Graphics.Clear() is called.";

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

      bool rMatch = Math.Abs(this.retrievedColor.R - expectedR) < 0.001f;
      bool gMatch = Math.Abs(this.retrievedColor.G - expectedG) < 0.001f;
      bool bMatch = Math.Abs(this.retrievedColor.B - expectedB) < 0.001f;
      bool aMatch = Math.Abs(this.retrievedColor.A - expectedA) < 0.001f;

      if (rMatch && gMatch && bMatch && aMatch)
      {
        this.CurrentStatus = TestStatus.Passed;
        this.Details = $"Retrieved background color is correct (R={this.retrievedColor.R}, G={this.retrievedColor.G}, B={this.retrievedColor.B}, A={this.retrievedColor.A}).";
      }
      else
      {
        this.CurrentStatus = TestStatus.Failed;
        this.Details = $"Retrieved background color is incorrect. Expected (R={expectedR}, G={expectedG}, B={expectedB}, A={expectedA}), Got (R={this.retrievedColor.R}, G={this.retrievedColor.G}, B={this.retrievedColor.B}, A={this.retrievedColor.A}).";
      }

      this.EndTest();
    }
  }
}
