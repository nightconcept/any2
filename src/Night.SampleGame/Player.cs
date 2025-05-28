using System;
using System.IO;
using System.Collections.Generic;

using Night;

namespace Night.SampleGame
{
  public class Player
  {
    public float X { get; private set; }
    public float Y { get; private set; }
    public int Width { get; private set; }
    public int Height { get; private set; }

    private float _velocityX;
    private float _velocityY;

    private const float HorizontalSpeed = 200f; // Pixels per second
    private const float JumpStrength = -450f;  // Initial upward velocity
    private const float Gravity = 1000f;       // Pixels per second squared

    private bool _isGrounded;

    private Sprite? _playerSprite; // To hold the blue rectangle sprite

    public Player()
    {
      // Initialize properties in Load()
      _isGrounded = false; // Start in the air or assume Load sets initial grounded state
    }

    public void Load()
    {
      Width = 32;
      Height = 64;

      // Initial position: Centered horizontally, resting on the first platform (Y=500, H=50).
      // Player's top-left Y = PlatformTopY - PlayerHeight = 500 - 64 = 436.
      // Player's top-left X = (WindowWidth / 2) - (PlayerWidth / 2) = (800 / 2) - (32 / 2) = 400 - 16 = 384.
      // Assuming window width is 800 as per Game.cs Load()
      X = 384f;
      Y = 436f; // This will be adjusted by gravity until grounded on a floor/platform

      _velocityX = 0f;
      _velocityY = 0f;

      // Attempt to load a pre-made blue image for the player.
      // This file needs to exist: "assets/images/player_sprite_blue_32x64.png"
      // Or a 1x1 blue pixel: "assets/images/pixel_blue.png" which we then scale.
      // For simplicity with current Draw method (no tinting), let's assume a 32x64 blue sprite.
      // If we used a 1x1 pixel_blue.png, scaleX would be Width, scaleY would be Height.
      string baseDirectory = AppContext.BaseDirectory;
      // Using a specific asset name as discussed due to lack of tinting.
      // This asset would be a 32x64 solid blue rectangle image.
      string imageRelativePath = Path.Combine("assets", "images", "player_sprite_blue_32x64.png");
      string imageFullPath = Path.Combine(baseDirectory, imageRelativePath);

      _playerSprite = Graphics.NewImage(imageFullPath);
      if (_playerSprite == null)
      {
        Console.WriteLine($"Player.Load: Failed to load player sprite at '{imageFullPath}'. A blue rectangle will not be drawn.");
        // Fallback: if sprite fails, player will be invisible.
        // Alternative: could try to create a 1x1 white pixel and draw it scaled,
        // but without tinting it would be a white rectangle.
        // For now, we rely on the pre-colored asset.
      }
      else
      {

      }
      _isGrounded = false; // Player starts potentially in the air and falls to ground.
    }

    private static bool CheckAABBCollision(Rectangle rect1, Rectangle rect2)
    {
      // True if the rectangles are overlapping
      return rect1.X < rect2.X + rect2.Width &&
             rect1.X + rect1.Width > rect2.X &&
             rect1.Y < rect2.Y + rect2.Height &&
             rect1.Y + rect1.Height > rect2.Y;
    }

    public void Update(double deltaTime, List<Rectangle> platforms)
    {
      float dt = (float)deltaTime;

      // 1. Handle Input & Apply Jump Impulse
      _velocityX = 0;
      if (Night.Keyboard.IsDown(Night.KeyCode.Left))
      {
        _velocityX = -HorizontalSpeed;
      }
      if (Night.Keyboard.IsDown(Night.KeyCode.Right))
      {
        _velocityX = HorizontalSpeed;
      }

      bool tryingToJump = Night.Keyboard.IsDown(Night.KeyCode.Space);
      if (tryingToJump && _isGrounded)
      {
        _velocityY = JumpStrength;
        _isGrounded = false; // Explicitly set to false when jump starts
      }

      // 2. Apply Gravity
      if (!_isGrounded)
      {
        _velocityY += Gravity * dt;
      }

      // 3. Horizontal Movement and Collision
      X += _velocityX * dt;
      Rectangle playerBoundingBox = new Rectangle((int)X, (int)Y, Width, Height);

      foreach (var platform in platforms)
      {
        // Update playerBoundingBox X for current position before check
        playerBoundingBox.X = (int)X;
        playerBoundingBox.Y = (int)Y; // Keep Y fixed for horizontal check pass

        if (CheckAABBCollision(new Rectangle((int)X, (int)Y, Width, Height), platform))
        {
          if (_velocityX > 0) // Moving right, collided with left edge of platform
          {
            X = platform.X - Width;
          }
          else if (_velocityX < 0) // Moving left, collided with right edge of platform
          {
            X = platform.X + platform.Width;
          }
          _velocityX = 0; // Stop horizontal movement on collision
        }
      }

      // 4. Vertical Movement and Collision
      Y += _velocityY * dt;

      // Update playerBoundingBox for vertical check using potentially corrected X and new Y
      playerBoundingBox.X = (int)X;
      playerBoundingBox.Y = (int)Y;

      bool resolvedAndGroundedThisFrame = false;

      foreach (var platform in platforms)
      {
        // Update playerBoundingBox.Y for each platform check, as Y might be adjusted by a previous platform.
        playerBoundingBox.Y = (int)Y;
        if (CheckAABBCollision(playerBoundingBox, platform))
        {
          if (_velocityY > 0) // Moving Down
          {
            Y = platform.Y - Height; // Snap to top of platform
            _velocityY = 0;
            resolvedAndGroundedThisFrame = true;
          }
          else if (_velocityY < 0) // Moving Up
          {
            // Check if player's top is actually hitting the platform's bottom
            if (playerBoundingBox.Y < platform.Y + platform.Height && playerBoundingBox.Y + Height > platform.Y + platform.Height)
            {
              Y = platform.Y + platform.Height; // Snap to bottom of platform
              _velocityY = 0;
            }
            // resolvedAndGroundedThisFrame remains false (hit head)
          }
          else // _velocityY == 0 (Stationary or slid horizontally into vertical overlap)
          {
            // If player is overlapping with the top part of the platform.
            // Player's top (Y) must be above platform's top (platform.Y).
            // Player's bottom (Y + Height) must be at or below platform's top (platform.Y).
            // And critically, the player's bottom must be very close to the platform top to count as grounded.
            float penetrationDepth = (Y + Height) - platform.Y;
            if (penetrationDepth >= 0 && Y < platform.Y && penetrationDepth < 5.0f /* Allow small penetration to count as ground */ )
            {
              Y = platform.Y - Height; // Snap to top
              resolvedAndGroundedThisFrame = true; // Become or stay grounded
            }
          }
        }
      }
      _isGrounded = resolvedAndGroundedThisFrame;

      // If a jump was initiated and _isGrounded became false,
      // and player is still moving upwards (_velocityY < 0), they are not grounded.
      // This ensures that if a jump starts, _isGrounded remains false until landing.
      if (tryingToJump && _velocityY < 0)
      { // Check if jump was initiated *this frame*
        _isGrounded = false;
      }


      // Prevent player from going off-screen left/right (simple boundary)
      // These values should ideally come from Window.GetWidth/Height if game resizes
      float gameWindowWidth = 800;
      if (X < 0) X = 0;
      if (X + Width > gameWindowWidth) X = gameWindowWidth - Width;
      // Top boundary (optional, could allow jumping off screen top)
      // if (Y < 0) { Y = 0; if (_velocityY < 0) _velocityY = 0; }
    }

    public void Draw()
    {
      if (_playerSprite != null)
      {
        // If player_sprite_blue_32x64.png is exactly 32x64, scaleX and scaleY are 1.
        // If it was a 1x1 pixel_blue.png, then scaleX=Width, scaleY=Height.
        // Assuming the loaded sprite is already the correct size (32x64):
        Graphics.Draw(_playerSprite, X, Y);
      }
      else
      {
        // Fallback: Could draw a placeholder or nothing if sprite failed to load.
        // For now, player is invisible if sprite load fails.
        // To draw a white rectangle as a fallback (if we had a white pixel sprite):
        // if (_fallbackWhitePixelSprite != null) {
        //    Graphics.Draw(_fallbackWhitePixelSprite, X, Y, 0, Width, Height);
        // }
      }
    }
  }
}
