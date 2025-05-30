// <copyright file="PointF.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System;

namespace Night
{
  /// <summary>
  /// Represents a 2D point with floating-point coordinates.
  /// </summary>
  public struct PointF
  {
    /// <summary>
    /// The X-coordinate of the point.
    /// </summary>
    public float X;

    /// <summary>
    /// The Y-coordinate of the point.
    /// </summary>
    public float Y;

    /// <summary>
    /// Initializes a new instance of the <see cref="PointF"/> struct.
    /// </summary>
    /// <param name="x">The X-coordinate.</param>
    /// <param name="y">The Y-coordinate.</param>
    public PointF(float x, float y)
    {
      this.X = x;
      this.Y = y;
    }
  }
}
