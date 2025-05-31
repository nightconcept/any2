# `love.filesystem` Module API Mapping

This document maps the functions available in the `love.filesystem` module of Love2D to their proposed equivalents in the Night Engine. Most functions in this module are **Out of Scope** for the initial prototype.

| Love2D Function (`love.filesystem.`) | Night Engine API (`Night.Filesystem.`) | Notes / C# Signature Idea | Status (Prototype Scope) | Done |
|--------------------------------------|----------------------------------------|---------------------------|--------------------------|------|
| `love.filesystem.append(name, data, size)` | `Night.Filesystem.Append(string path, byte[] data, int? size = null)` or `Night.Filesystem.AppendText(string path, string content)` | `public static bool Append(string path, byte[] data, int? size = null)` <br> `public static bool AppendText(string path, string content)` | Out of Scope | [ ] |
| `love.filesystem.areSymlinksEnabled()` | `Night.Filesystem.AreSymlinksEnabled()` | `public static bool AreSymlinksEnabled()` | Out of Scope | [ ] |
| `love.filesystem.createDirectory(name)` | `Night.Filesystem.CreateDirectory(string path)` | `public static bool CreateDirectory(string path)` | Out of Scope | [ ] |
| `love.filesystem.getAppdataDirectory()` | `Night.Filesystem.GetAppDataDirectory()` | `public static string GetAppDataDirectory()` | Out of Scope | [ ] |
| `love.filesystem.getDirectoryItems(name)` | `Night.Filesystem.GetDirectoryItems(string path)` | `public static string[] GetDirectoryItems(string path)` | Out of Scope | [ ] |
| `love.filesystem.getExecutablePath()` | `Night.Filesystem.GetExecutablePath()` | `public static string GetExecutablePath()` | Out of Scope | [ ] |
| `love.filesystem.getIdentity()`     | `Night.Filesystem.GetIdentity()`   | `public static string GetIdentity()` <br> Gets the save directory identity. | Out of Scope | [ ] |
| `love.filesystem.getLastModified(name)` | `Night.Filesystem.GetInfo(string path).ModTime` | `public static DateTime GetLastModifiedTime(string path)` (or long timestamp) | Superseded by GetInfo | [x] |
| `love.filesystem.getRealDirectory(name)` | `Night.Filesystem.GetRealDirectory(string path)` | `public static string GetRealDirectory(string path)` <br> Resolves symlinks. | Out of Scope | [ ] |
| `love.filesystem.getSaveDirectory()` | `Night.Filesystem.GetSaveDirectory()` | `public static string GetSaveDirectory()` | Out of Scope | [ ] |
| `love.filesystem.getSize(name)`     | `Night.Filesystem.GetInfo(string path).Size` | `public static long GetFileSize(string path)` | Superseded by GetInfo | [x] |
| `love.filesystem.getSource()`       | `Night.Filesystem.GetSourcePath()` | `public static string GetSourcePath()` <br> Path to the game's source (.love file or directory). | Out of Scope | [ ] |
| `love.filesystem.getSourceBaseDirectory()` | `Night.Filesystem.GetSourceBaseDirectory()` | `public static string GetSourceBaseDirectory()` | Out of Scope | [ ] |
| `love.filesystem.getUserDirectory()` | `Night.Filesystem.GetUserDirectory()` | `public static string GetUserDirectory()` | Out of Scope | [ ] |
| `love.filesystem.getWorkingDirectory()` | `Night.Filesystem.GetWorkingDirectory()` | `public static string GetWorkingDirectory()` | Out of Scope | [ ] |
| `love.filesystem.isFused()`         | `Night.Filesystem.IsFused()`       | `public static bool IsFused()` <br> True if game is a .love file and merged with interpreter. | Out of Scope | [ ] |
| `love.filesystem.isDirectory(name)` | `Night.Filesystem.GetInfo(string path).Type == Night.FileType.Directory` | `public static bool IsDirectory(string path)` | Superseded by GetInfo | [x] |
| `love.filesystem.isFile(name)`      | `Night.Filesystem.GetInfo(string path).Type == Night.FileType.File` | `public static bool IsFile(string path)` | Superseded by GetInfo | [x] |
| `love.filesystem.isSymlink(name)`   | `Night.Filesystem.GetInfo(string path).Type == Night.FileType.Symlink` | `public static bool IsSymlink(string path)` | Superseded by GetInfo | [x] |
| `love.filesystem.lines(name)`       | `Night.Filesystem.ReadLines(string path)` | `public static IEnumerable<string> ReadLines(string path)` | Out of Scope | [ ] |
| `love.filesystem.load(name)`        | `Night.Filesystem.LoadLuaScript(string path)` | `public static Action LoadLuaScript(string path)` <br> Loads and runs a Lua file. Night Engine might not support this directly. | Out of Scope | [ ] |
| `love.filesystem.mount(archive, mountpoint, appendToPath)` | `Night.Filesystem.Mount(string archivePath, string mountPoint, bool appendToSearchPath = false)` | `public static bool Mount(...)` | Out of Scope | [ ] |
| `love.filesystem.newFile(filename, mode)` | `Night.Filesystem.NewFileStream(string path, Night.FileMode mode = Read)` | `public static Night.FileStream NewFileStream(...)` <br> `FileMode` enum: `Read`, `Write`, `Append`. `FileStream` would be a custom stream wrapper. | Out of Scope | [ ] |
| `love.filesystem.newFileData(contents, name, decoder)` | `Night.Filesystem.NewFileData(byte[] content, string name, Night.FileDecoder decoder = Raw)` | `public static Night.FileData NewFileData(...)` <br> `FileDecoder` enum: `Raw`, `Base64`. `FileData` is an in-memory file. | Out of Scope | [ ] |
| `love.filesystem.read(name, size)`  | `Night.Filesystem.ReadBytes(string path, int? count = null)` or `Night.Filesystem.ReadText(string path)` | `public static byte[]? ReadBytes(string path, int? count = null)` <br> `public static string? ReadText(string path)` | Out of Scope | [ ] |
| `love.filesystem.remove(name)`      | `Night.Filesystem.Remove(string path)` | `public static bool Remove(string path)` <br> Removes file or empty directory. | Out of Scope | [ ] |
| `love.filesystem.setIdentity(name, appendToPath)` | `Night.Filesystem.SetIdentity(string identity, bool appendToPath = false)` | `public static void SetIdentity(...)` | Out of Scope | [ ] |
| `love.filesystem.setSymlinksEnabled(enable)` | `Night.Filesystem.SetSymlinksEnabled(bool enable)` | `public static void SetSymlinksEnabled(bool enable)` | Out of Scope | [ ] |
| `love.filesystem.setSource(path)`   | `Night.Filesystem.SetSource(string path)` | `public static void SetSource(string path)` | Out of Scope | [ ] |
| `love.filesystem.unmount(archive)`  | `Night.Filesystem.Unmount(string archivePath)` | `public static bool Unmount(string archivePath)` | Out of Scope | [ ] |
| `love.filesystem.write(name, data, size)` | `Night.Filesystem.WriteBytes(string path, byte[] data, int? size = null)` or `Night.Filesystem.WriteText(string path, string content)` | `public static bool WriteBytes(...)` <br> `public static bool WriteText(...)` | Out of Scope | [ ] |

| `love.filesystem.getInfo(path, filtertype, info)` | `Night.Filesystem.GetInfo(string path, Night.FileType? filterType = null, Night.FileSystemInfo? existingInfo = null)` | `public static Night.FileSystemInfo? GetInfo(string path, Night.FileType? filterType = null)` <br> `public static Night.FileSystemInfo? GetInfo(string path, Night.FileSystemInfo info)` <br> `public static Night.FileSystemInfo? GetInfo(string path, Night.FileType filterType, Night.FileSystemInfo info)` | In Scope | [x] |

**Night Engine Specific Types:**
*   `Night.FileMode`: Enum (`Read`, `Write`, `Append`).
*   `Night.FileStream`: Custom stream wrapper for file operations.
*   `Night.FileData`: Represents an in-memory file.
*   `Night.FileDecoder`: Enum (`Raw`, `Base64`).
*   `Night.FileType`: Enum (`File`, `Directory`, `Symlink`, `Other`, `None`).
*   `Night.FileSystemInfo`: Class (Properties: `Type`, `Size`, `ModTime`).
