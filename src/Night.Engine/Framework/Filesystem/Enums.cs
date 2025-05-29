using System;

namespace Night
{
  /// <summary>
  /// Represents the type of a file system object.
  /// </summary>
  public enum FileType
  {
    /// <summary>
    /// A regular file.
    /// </summary>
    File,
    /// <summary>
    /// A directory.
    /// </summary>
    Directory,
    /// <summary>
    /// A symbolic link.
    /// </summary>
    Symlink,
    /// <summary>
    /// Other type (e.g., device, pipe).
    /// </summary>
    Other,
    /// <summary>
    /// The path does not exist or its type cannot be determined.
    /// </summary>
    None
  }

  /// <summary>
  /// Represents the different modes you can open a File in.
  /// </summary>
  public enum FileMode
  {
    /// <summary>
    /// Open a file for read.
    /// </summary>
    Read,
    /// <summary>
    /// Open a file for write.
    /// </summary>
    Write,
    /// <summary>
    /// Open a file for append.
    /// </summary>
    Append,
    /// <summary>
    /// Do not open a file (represents a closed file.)
    /// </summary>
    Close,
    /// <summary>
    /// Open a file for write.
    /// </summary>
    w = Write,
    /// <summary>
    /// Open a file for read.
    /// </summary>
    r = Read,
    /// <summary>
    /// Open a file for append.
    /// </summary>
    a = Append,
    /// <summary>
    /// Do not open a file (represents a closed file.)
    /// </summary>
    c = Close
  }
}
