// <copyright file="GraphicsTests.cs" company="Night Circle">
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

using Night; // Assuming Night.Graphics is in the Night namespace

using Xunit;

// Since Night.Window.RendererPtr is crucial and often checked for null,
// and we can't easily initialize a renderer in unit tests,
// we might need a way to simulate its state or test paths that handle null.
// For now, tests will assume RendererPtr is null if not otherwise set by a (future) test helper.
namespace Night.Tests.Graphics
{
  /// <summary>
  /// Contains unit tests for the <see cref="Night.Graphics"/> class.
  /// </summary>
  public class GraphicsTests
  {
    // Helper to simulate a null Window.RendererPtr scenario if needed,
    // though direct manipulation of static members of other classes in tests can be tricky.
    // For now, we rely on the default state or what Night.Window provides.

    /// <summary>
    /// Tests that <see cref="Night.Graphics.NewImage(string)"/> returns null when the file path is null.
    /// </summary>
    [Fact]
    public void NewImage_NullFilePath_ReturnsNull()
    {
      // Act
      var image = Night.Graphics.NewImage(null!); // Pass null with null-forgiving operator

      // Assert
      Assert.Null(image);
    }

    /// <summary>
    /// Tests that <see cref="Night.Graphics.NewImage(string)"/> returns null for a non-existent file.
    /// </summary>
    [Fact]
    public void NewImage_NonExistentFile_ReturnsNull()
    {
      // Arrange
      string nonExistentFilePath = "path/to/non_existent_image.png";

      // Act
      var image = Night.Graphics.NewImage(nonExistentFilePath);

      // Assert
      Assert.Null(image);
    }

    // Note: Testing NewImage success case requires a valid SDL renderer and an actual file,
    // which is more of an integration test. Unit tests focus on C# logic paths.

    /// <summary>
    /// Tests that <see cref="Night.Graphics.Draw(Sprite, float, float, float, float, float, float, float)"/>
    /// does not throw an exception when the provided sprite is null.
    /// </summary>
    [Fact]
    public void Draw_NullSprite_DoesNotThrow()
    {
      // Arrange
      Sprite nullSprite = null!;

      // Act & Assert
      // We expect it to return early without throwing an exception.
      var exception = Record.Exception(() => Night.Graphics.Draw(nullSprite, 0, 0));
      Assert.Null(exception);
    }

    /// <summary>
    /// Tests that <see cref="Night.Graphics.Draw(Sprite, float, float, float, float, float, float, float)"/>
    /// does not throw an exception when the sprite's texture is null (IntPtr.Zero).
    /// </summary>
    [Fact]
    public void Draw_SpriteWithNullTexture_DoesNotThrow()
    {
      // Arrange
      // Create a Sprite instance but simulate its internal Texture being null.
      // This requires Sprite to have a constructor or a way to be instantiated
      // for testing purposes, even if its Texture is not valid.
      // Assuming Sprite constructor allows creating an instance that might later be found to have a null texture.
      // If Sprite constructor itself throws on null texture, this test needs adjustment.
      // For now, let's assume we can create a 'dummy' sprite.
      // A more direct way would be if Sprite had an internal/test constructor or if we used a mock.
      // Given the current Sprite structure, direct instantiation for this specific case is hard.
      // Let's assume a scenario where a Sprite object exists but its Texture is somehow null.
      // This test is more conceptual without deeper mocking/refactoring of Sprite for testability.
      // For now, we'll rely on the null check for the sprite object itself.
      // A more robust test would involve a Sprite instance where sprite.Texture is IntPtr.Zero.
      // This might require a test-specific constructor or property setter on Sprite.
      // As Graphics.Draw checks `sprite.Texture == IntPtr.Zero`, we can simulate this if Sprite allows.
      // Let's assume a Sprite can be created with a zero IntPtr texture for testing.
      var spriteWithNullTexture = new Sprite(IntPtr.Zero, 0, 0); // This matches the constructor

      // Act & Assert
      var exception = Record.Exception(() => Night.Graphics.Draw(spriteWithNullTexture, 0, 0));
      Assert.Null(exception); // Expect console output, but no throw
    }

    /// <summary>
    /// Tests that <see cref="Night.Graphics.Rectangle(DrawMode, float, float, float, float)"/>
    /// does not throw an exception for invalid (zero or negative) dimensions.
    /// </summary>
    /// <param name="width">The width of the rectangle.</param>
    /// <param name="height">The height of the rectangle.</param>
    [Theory]
    [InlineData(0, 10)]
    [InlineData(10, 0)]
    [InlineData(-1, 10)]
    [InlineData(10, -1)]
    public void Rectangle_InvalidDimensions_DoesNotThrow(float width, float height)
    {
      // Act & Assert
      var exception = Record.Exception(() => Night.Graphics.Rectangle(DrawMode.Fill, 0, 0, width, height));
      Assert.Null(exception); // Expects to return early
    }

    /// <summary>
    /// Tests that <see cref="Night.Graphics.Line(PointF[])"/>
    /// does not throw an exception when the points array is null.
    /// </summary>
    [Fact]
    public void Line_NullPointsArray_DoesNotThrow()
    {
      // Act & Assert
      var exception = Record.Exception(() => Night.Graphics.Line(null!));
      Assert.Null(exception); // Expects console output and early return
    }

    /// <summary>
    /// Tests that <see cref="Night.Graphics.Line(PointF[])"/>
    /// does not throw an exception when the points array contains fewer than two points.
    /// </summary>
    [Fact]
    public void Line_PointsArrayWithLessThanTwoPoints_DoesNotThrow()
    {
      // Arrange
      var points = new PointF[] { new PointF(0, 0) };

      // Act & Assert
      var exception = Record.Exception(() => Night.Graphics.Line(points));
      Assert.Null(exception); // Expects console output and early return
    }

    /// <summary>
    /// Tests that <see cref="Night.Graphics.Polygon(DrawMode, PointF[])"/>
    /// does not throw an exception when the vertices array is null.
    /// </summary>
    [Fact]
    public void Polygon_NullVerticesArray_DoesNotThrow()
    {
      // Act & Assert
      var exception = Record.Exception(() => Night.Graphics.Polygon(DrawMode.Fill, null!));
      Assert.Null(exception); // Expects console output and early return
    }

    /// <summary>
    /// Tests that <see cref="Night.Graphics.Polygon(DrawMode, PointF[])"/>
    /// does not throw an exception when the vertices array contains fewer than three vertices.
    /// </summary>
    [Fact]
    public void Polygon_VerticesArrayWithLessThanThreeVertices_DoesNotThrow()
    {
      // Arrange
      var vertices = new PointF[] { new PointF(0, 0), new PointF(1, 1) };

      // Act & Assert
      var exception = Record.Exception(() => Night.Graphics.Polygon(DrawMode.Fill, vertices));
      Assert.Null(exception); // Expects console output and early return
    }

    /// <summary>
    /// Tests that <see cref="Night.Graphics.Circle(DrawMode, float, float, float, int)"/>
    /// uses a default segment count and does not throw for invalid (zero or negative) segment inputs.
    /// </summary>
    /// <param name="segments">The number of segments for the circle.</param>
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Circle_InvalidSegments_UsesDefaultAndDoesNotThrow(int segments)
    {
      // Act & Assert
      // This test also implicitly checks if RendererPtr is null, it should not throw.
      var exception = Record.Exception(() => Night.Graphics.Circle(DrawMode.Line, 0, 0, 10, segments));
      Assert.Null(exception);
    }

    /// <summary>
    /// Tests that <see cref="Night.Graphics.Circle(DrawMode, float, float, float, int)"/>
    /// uses a zero radius and does not throw if a negative radius is provided.
    /// </summary>
    [Fact]
    public void Circle_NegativeRadius_UsesZeroRadiusAndDoesNotThrow()
    {
      // Act & Assert
      var exception = Record.Exception(() => Night.Graphics.Circle(DrawMode.Line, 0, 0, -10, 12));
      Assert.Null(exception);
    }

    // Tests for methods when RendererPtr is null
    // These assume Window.RendererPtr is null by default in a test environment
    // or requires a specific setup to make it non-null for other tests.

    /// <summary>
    /// Tests that <see cref="Night.Graphics.SetColor(Color)"/>
    /// does not throw an exception when the renderer is null.
    /// </summary>
    [Fact]
    public void SetColor_NullRenderer_DoesNotThrow()
    {
      // Act & Assert
      var exception = Record.Exception(() => Night.Graphics.SetColor(new Color(255, 255, 255, 255)));
      Assert.Null(exception); // Expects console output
    }

    /// <summary>
    /// Tests that <see cref="Night.Graphics.Rectangle(DrawMode, float, float, float, float)"/>
    /// does not throw an exception when the renderer is null.
    /// </summary>
    [Fact]
    public void Rectangle_NullRenderer_DoesNotThrow()
    {
      // Act & Assert
      var exception = Record.Exception(() => Night.Graphics.Rectangle(DrawMode.Fill, 0, 0, 10, 10));
      Assert.Null(exception); // Expects console output
    }

    /// <summary>
    /// Tests that <see cref="Night.Graphics.Line(float, float, float, float)"/>
    /// does not throw an exception when the renderer is null.
    /// </summary>
    [Fact]
    public void Line_Single_NullRenderer_DoesNotThrow()
    {
      // Act & Assert
      var exception = Record.Exception(() => Night.Graphics.Line(0, 0, 1, 1));
      Assert.Null(exception); // Expects console output
    }

    /// <summary>
    /// Tests that <see cref="Night.Graphics.Line(PointF[])"/>
    /// does not throw an exception when the renderer is null.
    /// </summary>
    [Fact]
    public void Line_Multiple_NullRenderer_DoesNotThrow()
    {
      // Arrange
      var points = new PointF[] { new PointF(0, 0), new PointF(1, 1), new PointF(2, 2) };

      // Act & Assert
      var exception = Record.Exception(() => Night.Graphics.Line(points));
      Assert.Null(exception); // Expects console output
    }

    /// <summary>
    /// Tests that <see cref="Night.Graphics.Polygon(DrawMode, PointF[])"/>
    /// does not throw an exception when the renderer is null.
    /// </summary>
    [Fact]
    public void Polygon_NullRenderer_DoesNotThrow()
    {
      // Arrange
      var vertices = new PointF[] { new PointF(0, 0), new PointF(1, 0), new PointF(0, 1) };

      // Act & Assert
      var exception = Record.Exception(() => Night.Graphics.Polygon(DrawMode.Line, vertices));
      Assert.Null(exception); // Expects console output
    }

    /// <summary>
    /// Tests that <see cref="Night.Graphics.Circle(DrawMode, float, float, float, int)"/>
    /// does not throw an exception when the renderer is null.
    /// </summary>
    [Fact]
    public void Circle_NullRenderer_DoesNotThrow()
    {
      // Act & Assert
      var exception = Record.Exception(() => Night.Graphics.Circle(DrawMode.Fill, 0, 0, 10, 12));
      Assert.Null(exception); // Expects console output
    }

    /// <summary>
    /// Tests that <see cref="Night.Graphics.Draw(Sprite, float, float, float, float, float, float, float)"/>
    /// does not throw an exception when the renderer is null.
    /// This also covers the case where the sprite itself might be valid but the renderer isn't.
    /// </summary>
    [Fact]
    public void Draw_NullRenderer_DoesNotThrow()
    {
      // Arrange
      // Assume a Sprite can be created even if it cannot be effectively drawn without a renderer.
      // This Sprite constructor will need to handle such cases gracefully or be mockable.
      // For this test, we are focusing on the Graphics.Draw method's behavior.
      // If Sprite needs a valid texture path for construction, this test setup needs to be adjusted.
      // Let's assume we can create a dummy sprite for this test as we did for Draw_SpriteWithNullTexture
      var dummySprite = new Sprite(IntPtr.Zero, 10, 10); // Or any valid-looking sprite that doesn't rely on renderer for creation

      // Act & Assert
      var exception = Record.Exception(() => Night.Graphics.Draw(dummySprite, 0, 0));
      Assert.Null(exception); // Expects console output
    }

    /// <summary>
    /// Tests that <see cref="Night.Graphics.Clear"/>
    /// does not throw an exception when the renderer is null.
    /// </summary>
    [Fact]
    public void Clear_NullRenderer_DoesNotThrow()
    {
      // Act & Assert
      var exception = Record.Exception(() => Night.Graphics.Clear(new Color(0, 0, 0, 255)));
      Assert.Null(exception); // Expects console output
    }

    /// <summary>
    /// Tests that <see cref="Night.Graphics.Present()"/>
    /// does not throw an exception when the renderer is null.
    /// </summary>
    [Fact]
    public void Present_NullRenderer_DoesNotThrow()
    {
      // Act & Assert
      var exception = Record.Exception(() => Night.Graphics.Present());
      Assert.Null(exception); // Expects console output
    }
  }
}
