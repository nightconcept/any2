using System;
using System.IO;

namespace Night
{
  /// <summary>
  /// Provides basic file system operations.
  /// Inspired by Love2D's love.filesystem module.
  /// </summary>
  public static class Filesystem
  {
    private static readonly DateTimeOffset UnixEpoch = new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero);

    /// <summary>
    /// Gets information about the specified file or directory.
    /// </summary>
    /// <param name="path">The file or directory path to check.</param>
    /// <param name="filterType">If supplied, this parameter causes getInfo to only return the info table if the item at the given path matches the specified file type.</param>
    /// <returns>A FileSystemInfo object containing information about the specified path, or null if nothing exists at the path or if it doesn't match the filterType.</returns>
    public static FileSystemInfo? GetInfo(string path, FileType? filterType = null)
    {
      if (string.IsNullOrEmpty(path))
      {
        return null;
      }

      Night.FileType type = Night.FileType.None;
      long? size = null;
      long? modTime = null;

      try
      {
        if (File.Exists(path))
        {
          var fileInfo = new FileInfo(path);
          if ((fileInfo.Attributes & FileAttributes.ReparsePoint) == FileAttributes.ReparsePoint)
          {
            // Basic symlink check for files. More robust detection might be needed.
            // For now, treat as Symlink if it's a reparse point, otherwise File.
            // True symlink resolution (target type) is complex and platform-dependent.
            type = Night.FileType.Symlink; // Could be a symlink to a file or directory
                                           // To determine target type, one might need to resolve it,
                                           // but for now, just identifying it as a symlink.
          }
          else
          {
            type = Night.FileType.File;
          }
          size = fileInfo.Length;
          modTime = ((DateTimeOffset)fileInfo.LastWriteTimeUtc).ToUnixTimeSeconds();
        }
        else if (Directory.Exists(path))
        {
          var dirInfo = new DirectoryInfo(path);
          if ((dirInfo.Attributes & FileAttributes.ReparsePoint) == FileAttributes.ReparsePoint)
          {
            type = Night.FileType.Symlink; // Could be a symlink to a directory
          }
          else
          {
            type = Night.FileType.Directory;
          }
          // Size is typically not applicable or consistently defined for directories in this context.
          modTime = ((DateTimeOffset)dirInfo.LastWriteTimeUtc).ToUnixTimeSeconds();
        }
        else
        {
          return null; // Path does not exist
        }
      }
      catch (Exception) // IOException, UnauthorizedAccessException, etc.
      {
        return null; // Error accessing path
      }

      if (filterType.HasValue && type != filterType.Value)
      {
        // If a filter is provided and the determined type doesn't match, return null.
        // Exception: if the actual type is Symlink, and filter is File or Directory,
        // a more advanced check would be needed to see if the symlink *points* to
        // the filtered type. For 0.1.0, a direct type match is required.
        return null;
      }

      return new FileSystemInfo(type, size, modTime);
    }

    /// <summary>
    /// Gets information about the specified file or directory and populates an existing FileSystemInfo object.
    /// </summary>
    /// <param name="path">The file or directory path to check.</param>
    /// <param name="info">A FileSystemInfo object which will be filled in.</param>
    /// <returns>The FileSystemInfo object given as an argument, filled with information, or null if nothing exists at the path.</returns>
    public static FileSystemInfo? GetInfo(string path, FileSystemInfo info)
    {
      if (info == null) return null; // Or throw ArgumentNullException, depending on desired strictness

      var newInfo = GetInfo(path);
      if (newInfo != null)
      {
        info.Type = newInfo.Type;
        info.Size = newInfo.Size;
        info.ModTime = newInfo.ModTime;
        return info;
      }
      return null;
    }

    /// <summary>
    /// Gets information about the specified file or directory, filtered by type, and populates an existing FileSystemInfo object.
    /// </summary>
    /// <param name="path">The file or directory path to check.</param>
    /// <param name="filterType">Causes getInfo to only return the info table if the item at the given path matches the specified file type.</param>
    /// <param name="info">A FileSystemInfo object which will be filled in.</param>
    /// <returns>The FileSystemInfo object given as an argument, filled with information, or null if nothing exists at the path or if it doesn't match the filterType.</returns>
    public static FileSystemInfo? GetInfo(string path, FileType filterType, FileSystemInfo info)
    {
      if (info == null) return null;

      var newInfo = GetInfo(path, filterType);
      if (newInfo != null)
      {
        info.Type = newInfo.Type;
        info.Size = newInfo.Size;
        info.ModTime = newInfo.ModTime;
        return info;
      }
      return null;
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