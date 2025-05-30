// <copyright file="Sprite.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System;

namespace Night
{
  /// <summary>
  /// Represents a 2D sprite, typically an image loaded into a texture.
  /// </summary>
  public class Sprite
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="Sprite"/> class.
    /// </summary>
    /// <param name="texture">The SDL texture handle.</param>
    /// <param name="width">The width of the texture.</param>
    /// <param name="height">The height of the texture.</param>
    public Sprite(IntPtr texture, int width, int height)
    {
      this.Texture = texture;
      this.Width = width;
      this.Height = height;
    }

    /// <summary>
    /// Gets the SDL texture handle.
    /// </summary>
    public IntPtr Texture { get; }

    /// <summary>
    /// Gets the width of the sprite in pixels.
    /// </summary>
    public int Width { get; }

    /// <summary>
    /// Gets the height of the sprite in pixels.
    /// </summary>
    public int Height { get; }
  }
}
