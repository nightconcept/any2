// <copyright file="Structs.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Night
{
  using System;

  /// <summary>
  /// Represents a color with Red, Green, Blue, and Alpha components. Implements Love2D's RGBA color table.
  /// Each component is a byte value ranging from 0 to 255.
  /// The Alpha component controls the transparency of the color, where 0 is fully transparent and 255 is fully opaque.
  /// </summary>
  public struct Color
  {
    public byte R;
    public byte G;
    public byte B;
    public byte A;

    public Color(byte r, byte g, byte b, byte a = 255)
    {
      this.R = r;
      this.G = g;
      this.B = b;
      this.A = a;
    }

    // Common color presets
    public static readonly Color Black = new(0, 0, 0);
    public static readonly Color White = new(255, 255, 255);
    public static readonly Color Red = new(255, 0, 0);
    public static readonly Color Green = new(0, 255, 0);
    public static readonly Color Blue = new(0, 0, 255);
    public static readonly Color Yellow = new(255, 255, 0);
    public static readonly Color Magenta = new(255, 0, 255);
    public static readonly Color Cyan = new(0, 255, 255);
    public static readonly Color Transparent = new(0, 0, 0, 0);
  }

  /// <summary>
  /// Represents a rectangle with position (X, Y) and dimensions (Width, Height).
  /// </summary>
  public struct Rectangle
  {
    public int X;
    public int Y;
    public int Width;
    public int Height;

    public Rectangle(int x, int y, int width, int height)
    {
      this.X = x;
      this.Y = y;
      this.Width = width;
      this.Height = height;
    }
  }

  /// <summary>
  /// Represents a 2D point with floating-point coordinates.
  /// </summary>
  public struct PointF
  {
    public float X;
    public float Y;

    public PointF(float x, float y)
    {
      this.X = x;
      this.Y = y;
    }
  }
}
