# `love.math` Module API Mapping

This document maps the functions available in the `love.math` module of Love2D to their proposed equivalents in the Night Engine. Most of this functionality can be achieved using `System.Math` and `System.Random` in C#. A dedicated `Night.Math` module is **Out of Scope** for the initial prototype, but specific advanced functions might be added later.

| Love2D Function (`love.math.`) | Night Engine API (`Night.Math` or `System`) | Notes / C# Signature Idea | Status (Prototype Scope) | Done |
|--------------------------------|---------------------------------------------|---------------------------|--------------------------|------|
| `love.math.triangulate(polygon)` | `Night.Math.Triangulate(Night.PointF[] polygon)` | `public static int[] Triangulate(Night.PointF[] polygon)` <br> Returns indices for triangles. | Out of Scope | [ ] |
| `love.math.isConvex(polygon)`  | `Night.Math.IsConvex(Night.PointF[] polygon)` | `public static bool IsConvex(Night.PointF[] polygon)` | Out of Scope | [ ] |
| `love.math.getAngle(x1, y1, x2, y2)` | `Night.Math.GetAngle(float x1, float y1, float x2, float y2)` | `public static double GetAngle(float x1, float y1, float x2, float y2)` <br> Similar to `Math.Atan2(y2 - y1, x2 - x1)`. | Out of Scope (Use `System.Math`) | [ ] |
| `love.math.noise(x, y, z, w)`  | `Night.Math.Noise(double x, double? y = null, double? z = null, double? w = null)` | `public static double Noise(...)` <br> Simplex noise. | Out of Scope | [ ] |
| `love.math.random()`             | `(new System.Random()).NextDouble()` or `Night.Math.Random()` | `public static double Random()` <br> Returns [0, 1). | Out of Scope (Use `System.Random`) | [ ] |
| `love.math.random(max)`          | `(new System.Random()).Next(max + 1)` or `Night.Math.Random(int max)` | `public static int Random(int max)` <br> Returns [0, max]. Or `Next(1, max + 1)` for [1, max]. Love2D is [1,max] for integer. | Out of Scope (Use `System.Random`) | [ ] |
| `love.math.random(min, max)`     | `(new System.Random()).Next(min, max + 1)` or `Night.Math.Random(int min, int max)` | `public static int Random(int min, int max)` <br> Returns [min, max]. | Out of Scope (Use `System.Random`) | [ ] |
| `love.math.randomNormal(stddev, mean)` | `Night.Math.RandomNormal(double stdDev = 1.0, double mean = 0.0)` | `public static double RandomNormal(...)` <br> Normally distributed random number. | Out of Scope | [ ] |
| `love.math.setRandomSeed(seed)`  | `Night.Math.SetRandomSeed(int seed)` or `new System.Random(seed)` | `public static void SetRandomSeed(int seed)` <br> For a global `Night.Math` random generator. | Out of Scope (Use `System.Random` instance) | [ ] |
| `love.math.getRandomSeed()`      | `Night.Math.GetRandomSeed()`      | `public static (int seed, int? highSeed) GetRandomSeed()` | Out of Scope | [ ] |
| `love.math.getRandomState()`     | `Night.Math.GetRandomState()`     | `public static string GetRandomState()` | Out of Scope | [ ] |
| `love.math.setRandomState(state)`| `Night.Math.SetRandomState(string state)` | `public static void SetRandomState(string state)` | Out of Scope | [ ] |
| `love.math.newBezierCurve(points)` | `Night.Math.NewBezierCurve(Night.PointF[] controlPoints)` | `public static Night.BezierCurve NewBezierCurve(...)` | Out of Scope | [ ] |
| `love.math.newRandomGenerator()` | `Night.Math.NewRandomGenerator()` | `public static System.Random NewRandomGenerator()` or a custom `Night.RandomGenerator` class. | Out of Scope | [ ] |
| `love.math.gammaToLinear(c)`     | `Night.Math.GammaToLinear(double colorComponent)` | `public static double GammaToLinear(double colorComponent)` | Out of Scope | [ ] |
| `love.math.linearToGamma(c)`     | `Night.Math.LinearToGamma(double colorComponent)` | `public static double LinearToGamma(double colorComponent)` | Out of Scope | [ ] |

**Night Engine Specific Types (if module were implemented):**
*   `Night.PointF`: Struct for a 2D point with float coordinates.
*   `Night.BezierCurve`: Class representing a Bezier curve, with methods like `Evaluate(t)`, `GetDerivative(t)`.
*   `Night.RandomGenerator`: A class that might encapsulate `System.Random` or a custom PRNG, potentially with Love2D-compatible state management.
