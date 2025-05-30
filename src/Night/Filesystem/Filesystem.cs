// <copyright file="Filesystem.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

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

      long? size = null;
      FileType type;
      long? modTime;
      try
      {
        if (File.Exists(path))
        {
          var fileInfo = new FileInfo(path);
          if ((fileInfo.Attributes & FileAttributes.ReparsePoint) == FileAttributes.ReparsePoint)
          {
            type = FileType.Symlink;
          }
          else
          {
            type = FileType.File;
          }

          size = fileInfo.Length;
          modTime = ((DateTimeOffset)fileInfo.LastWriteTimeUtc).ToUnixTimeSeconds();
        }
        else if (Directory.Exists(path))
        {
          var dirInfo = new DirectoryInfo(path);
          if ((dirInfo.Attributes & FileAttributes.ReparsePoint) == FileAttributes.ReparsePoint)
          {
            type = FileType.Symlink;
          }
          else
          {
            type = FileType.Directory;
          }

          modTime = ((DateTimeOffset)dirInfo.LastWriteTimeUtc).ToUnixTimeSeconds();
        }
        else
        {
          return null;
        }
      }
      catch (Exception)
      {
        return null;
      }

      if (filterType.HasValue && type != filterType.Value)
      {
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
      if (info == null)
      {
        return null;
      }

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
      if (info == null)
      {
        return null;
      }

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
