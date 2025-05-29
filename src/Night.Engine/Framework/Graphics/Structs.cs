using System;

namespace Night
{
  /// <summary>
  /// Represents a color with Red, Green, Blue, and Alpha components.
  /// </summary>
  public struct Color
  {
    public byte R;
    public byte G;
    public byte B;
    public byte A;

    public Color(byte r, byte g, byte b, byte a = 255)
    {
      R = r;
      G = g;
      B = b;
      A = a;
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
      X = x;
      Y = y;
      Width = width;
      Height = height;
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
      X = x;
      Y = y;
    }
  }
}
