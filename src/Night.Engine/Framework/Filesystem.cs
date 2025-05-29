using System.IO;

namespace Night
{
  /// <summary>
  /// Provides basic file system operations.
  /// Inspired by Love2D's love.filesystem module.
  /// </summary>
  public static class Filesystem
  {
    /// <summary>
    /// Checks if a file or directory exists at the given path.
    /// </summary>
    /// <param name="path">The path to check.</param>
    /// <returns>True if a file or directory exists, false otherwise.</returns>
    public static bool Exists(string path)
    {
      return File.Exists(path) || Directory.Exists(path);
    }

    /// <summary>
    /// Checks if the given path corresponds to an existing file.
    /// </summary>
    /// <param name="path">The path to check.</param>
    /// <returns>True if the path is an existing file, false otherwise.</returns>
    public static bool IsFile(string path)
    {
      return File.Exists(path);
    }

    /// <summary>
    /// Checks if the given path corresponds to an existing directory.
    /// </summary>
    /// <param name="path">The path to check.</param>
    /// <returns>True if the path is an existing directory, false otherwise.</returns>
    public static bool IsDirectory(string path)
    {
      return Directory.Exists(path);
    }

    /// <summary>
    /// Reads the entire content of a file into a byte array.
    /// </summary>
    /// <param name="path">The path to the file to read.</param>
    /// <returns>A byte array containing the contents of the file.</returns>
    /// <exception cref="System.IO.FileNotFoundException">Thrown if the file is not found.</exception>
    public static byte[] ReadBytes(string path)
    {
      return File.ReadAllBytes(path);
    }

    /// <summary>
    /// Reads the entire content of a file into a string.
    /// </summary>
    /// <param name="path">The path to the file to read.</param>
    /// <returns>A string containing the contents of the file.</returns>
    /// <exception cref="System.IO.FileNotFoundException">Thrown if the file is not found.</exception>
    public static string ReadText(string path)
    {
      return File.ReadAllText(path);
    }
  }
}