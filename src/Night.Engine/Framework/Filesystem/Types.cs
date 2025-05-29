using System;

namespace Night
{
  /// <summary>
  /// Contains information about a file or directory.
  /// </summary>
  public class FileSystemInfo
  {
    /// <summary>
    /// The type of the object at the path (file, directory, symlink, etc.).
    /// Default is None.
    /// </summary>
    public FileType Type { get; set; } = FileType.None;

    /// <summary>
    /// The size in bytes of the file, or null if it can't be determined or not applicable (e.g. for a directory).
    /// </summary>
    public long? Size { get; set; }

    /// <summary>
    /// The file's last modification time in seconds since the Unix epoch, or null if it can't be determined.
    /// </summary>
    public long? ModTime { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="FileSystemInfo"/> class.
    /// </summary>
    public FileSystemInfo() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="FileSystemInfo"/> class with specified values.
    /// </summary>
    /// <param name="type">The type of the file system object.</param>
    /// <param name="size">The size of the file in bytes.</param>
    /// <param name="modTime">The last modification time in Unix epoch seconds.</param>
    public FileSystemInfo(FileType type, long? size, long? modTime)
    {
      Type = type;
      Size = size;
      ModTime = modTime;
    }
  }
}
