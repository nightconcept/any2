// <copyright file="GraphicsClearColorTest.cs" company="Night Circle">
// zlib license
// Copyright (c) 2025 Danny Solivan, Night Circle
// (Full license boilerplate as in other files)
// </copyright>

using System;

using Night;

using NightTest.Core;

namespace NightTest.Groups.Graphics
{
  /// <summary>
  /// Tests the Graphics.Clear() method with a specific color.
  /// Requires manual confirmation that the color is correct.
  /// </summary>
  public class GraphicsClearColorTest : BaseManualTestCase
  {
    private readonly Color _skyBlue = new Color(135, 206, 235);
    private bool _promptRequested = false;

    /// <inheritdoc/>
    public override string Name => "Graphics.Clear";

    /// <inheritdoc/>
    public override string Description => "Tests clearing the screen to sky blue (135, 206, 235). User must confirm color.";

    /// <inheritdoc/>
    public override void Load()
    {
      base.Load();

      Details = "Test running, displaying sky blue color.";
    }

    /// <inheritdoc/>
    protected override void UpdateManual(double deltaTime)
    {
      if (!_promptRequested && TestStopwatch.ElapsedMilliseconds > ManualTestPromptDelayMilliseconds)
      {
        RequestManualConfirmation("Is the screen cleared to a SKY BLUE color (like a clear daytime sky)?");
        _promptRequested = true;
      }
    }

    /// <inheritdoc/>
    public override void Draw()
    {
      Night.Graphics.Clear(_skyBlue);

      base.Draw();

      Night.Graphics.Present();
    }
  }
}
