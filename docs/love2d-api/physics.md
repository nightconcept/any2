# `love.physics` Module API Mapping

This document maps the functions available in the `love.physics` module of Love2D to their proposed equivalents in the Night Engine. This entire module is **Out of Scope** for the initial prototype.

| Love2D Function (`love.physics.`) | Night Engine API (`Night.Physics.`) | Notes / C# Signature Idea | Status (Prototype Scope) | Done |
|-----------------------------------|-------------------------------------|---------------------------|--------------------------|------|
| `love.physics.newWorld(xg, yg, sleep)` | `Night.Physics.NewWorld(float gravityX = 0, float gravityY = 0, bool allowSleep = true)` | `public static Night.World NewWorld(...)` | Out of Scope | [ ] |
| `love.physics.newBody(world, x, y, type)` | `(Night.World).NewBody(float x, float y, Night.BodyType type)` | `public Night.Body NewBody(...)` (Method on `World` instance) | Out of Scope | [ ] |
| `love.physics.newFixture(body, shape, density)` | `(Night.Body).NewFixture(Night.Shape shape, float density = 1.0f)` | `public Night.Fixture NewFixture(...)` (Method on `Body` instance) | Out of Scope | [ ] |
| `love.physics.newCircleShape(x, y, radius)` | `Night.Physics.NewCircleShape(float offsetX, float offsetY, float radius)` or `Night.Physics.NewCircleShape(float radius)` | `public static Night.CircleShape NewCircleShape(...)` | Out of Scope | [ ] |
| `love.physics.newRectangleShape(x, y, w, h, angle)` | `Night.Physics.NewRectangleShape(float offsetX, float offsetY, float width, float height, float angle = 0)` or `Night.Physics.NewRectangleShape(float width, float height)` | `public static Night.RectangleShape NewRectangleShape(...)` | Out of Scope | [ ] |
| `love.physics.newPolygonShape(...)` | `Night.Physics.NewPolygonShape(params Night.PointF[] vertices)` | `public static Night.PolygonShape NewPolygonShape(...)` | Out of Scope | [ ] |
| `love.physics.newEdgeShape(x1, y1, x2, y2)` | `Night.Physics.NewEdgeShape(float x1, float y1, float x2, float y2)` | `public static Night.EdgeShape NewEdgeShape(...)` | Out of Scope | [ ] |
| `love.physics.newChainShape(loop, ...)` | `Night.Physics.NewChainShape(bool loop, params Night.PointF[] vertices)` | `public static Night.ChainShape NewChainShape(...)` | Out of Scope | [ ] |
| `love.physics.newDistanceJoint(body1, body2, x1, y1, x2, y2, collideConnected)` | `Night.Physics.NewDistanceJoint(...)` | `public static Night.DistanceJoint NewDistanceJoint(...)` | Out of Scope | [ ] |
| `love.physics.newMouseJoint(body, x, y)` | `Night.Physics.NewMouseJoint(...)` | `public static Night.MouseJoint NewMouseJoint(...)` | Out of Scope | [ ] |
| `love.physics.newRevoluteJoint(...)` | `Night.Physics.NewRevoluteJoint(...)` | `public static Night.RevoluteJoint NewRevoluteJoint(...)` | Out of Scope | [ ] |
| `love.physics.newPrismaticJoint(...)` | `Night.Physics.NewPrismaticJoint(...)` | `public static Night.PrismaticJoint NewPrismaticJoint(...)` | Out of Scope | [ ] |
| `love.physics.newPulleyJoint(...)` | `Night.Physics.NewPulleyJoint(...)` | `public static Night.PulleyJoint NewPulleyJoint(...)` | Out of Scope | [ ] |
| `love.physics.newGearJoint(...)`   | `Night.Physics.NewGearJoint(...)`   | `public static Night.GearJoint NewGearJoint(...)` | Out of Scope | [ ] |
| `love.physics.newFrictionJoint(...)` | `Night.Physics.NewFrictionJoint(...)` | `public static Night.FrictionJoint NewFrictionJoint(...)` | Out of Scope | [ ] |
| `love.physics.newWeldJoint(...)`   | `Night.Physics.NewWeldJoint(...)`   | `public static Night.WeldJoint NewWeldJoint(...)` | Out of Scope | [ ] |
| `love.physics.newRopeJoint(...)`   | `Night.Physics.NewRopeJoint(...)`   | `public static Night.RopeJoint NewRopeJoint(...)` | Out of Scope | [ ] |
| `love.physics.newWheelJoint(...)`  | `Night.Physics.NewWheelJoint(...)`  | `public static Night.WheelJoint NewWheelJoint(...)` | Out of Scope | [ ] |
| `love.physics.newMotorJoint(...)`  | `Night.Physics.NewMotorJoint(...)`  | `public static Night.MotorJoint NewMotorJoint(...)` | Out of Scope | [ ] |
| `love.physics.getDistance(fixtureA, fixtureB)` | `Night.Physics.GetDistance(Night.Fixture fixtureA, Night.Fixture fixtureB)` | `public static (float distance, float xA, float yA, float xB, float yB) GetDistance(...)` | Out of Scope | [ ] |
| `love.physics.getMeter()`          | `Night.Physics.GetPixelsPerMeter()` | `public static float GetPixelsPerMeter()` (Love2D default is 30) | Out of Scope | [ ] |
| `love.physics.setMeter(scale)`     | `Night.Physics.SetPixelsPerMeter(float scale)` | `public static void SetPixelsPerMeter(float scale)` | Out of Scope | [ ] |
| `love.physics.setCallbacks(beginContact, endContact, preSolve, postSolve)` | `(Night.World).SetContactCallbacks(...)` | Methods on `World` instance. | Out of Scope | [ ] |
| `love.physics.getCallbacks()`      | `(Night.World).GetContactCallbacks()` | Methods on `World` instance. | Out of Scope | [ ] |

**Night Engine Specific Types (if module were implemented):**
*   `Night.World`: Represents the physics simulation world. Methods: `Update(dt)`, `DrawDebug()`, `RayCast()`, `QueryAABB()`.
*   `Night.Body`: Represents a rigid body. Methods: `ApplyForce()`, `GetPosition()`, `SetAngle()`, etc.
*   `Night.BodyType`: Enum (`Static`, `Kinematic`, `Dynamic`).
*   `Night.Fixture`: Attaches a shape to a body, defines material properties.
*   `Night.Shape`: Base class for collision shapes.
*   `Night.CircleShape`, `Night.RectangleShape`, `Night.PolygonShape`, `Night.EdgeShape`, `Night.ChainShape`: Specific shape types.
*   Various `Night.Joint` types: `DistanceJoint`, `MouseJoint`, etc.
*   `Night.PointF`: Struct for 2D float points.
