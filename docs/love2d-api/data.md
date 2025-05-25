# `love.data` Module API Mapping

This document maps the functions available in the `love.data` module of Love2D to their proposed equivalents in the Night Engine. This entire module is **Out of Scope** for the initial prototype. .NET provides extensive built-in support for these operations in namespaces like `System.IO.Compression`, `System.Security.Cryptography`, and `System.Text`.

| Love2D Function (`love.data.`) | Night Engine API (`Night.Data` or `System` namespaces) | Notes / C# Signature Idea | Status (Prototype Scope) | Done |
|--------------------------------|--------------------------------------------------------|---------------------------|--------------------------|------|
| `love.data.compress(container, format, rawstring, level)` | `Night.Data.Compress(Night.DataContainerType container, Night.CompressionFormat format, byte[] data, int? level = null)` | `public static byte[] Compress(...)` <br> Uses `System.IO.Compression`. | Out of Scope | [ ] |
| `love.data.decompress(container, format, compressedstring)` | `Night.Data.Decompress(Night.DataContainerType container, Night.CompressionFormat format, byte[] compressedData)` | `public static byte[] Decompress(...)` | Out of Scope | [ ] |
| `love.data.decode(container, format, encodedstring)` | `Night.Data.Decode(Night.DataContainerType container, Night.EncodingFormat format, string encodedString)` | `public static byte[] Decode(...)` <br> e.g., Base64, Hex. Uses `System.Convert`. | Out of Scope | [ ] |
| `love.data.encode(container, format, rawstring, linelength)` | `Night.Data.Encode(Night.DataContainerType container, Night.EncodingFormat format, byte[] data, int? lineLength = null)` | `public static string Encode(...)` | Out of Scope | [ ] |
| `love.data.getPackedSize(format)` | `Night.Data.GetPackedSize(string packFormat)` | `public static int GetPackedSize(string packFormat)` <br> For binary packing. | Out of Scope | [ ] |
| `love.data.hash(hashfunction, string_or_Data)` | `Night.Data.Hash(Night.HashFunction function, byte[] data)` or `Night.Data.Hash(Night.HashFunction function, string data)` | `public static string Hash(...)` <br> Uses `System.Security.Cryptography`. | Out of Scope | [ ] |
| `love.data.newDataView(data, offset, size)` | `Night.Data.NewDataView(byte[] data, int offset = 0, int? size = null)` | `public static Night.DataView NewDataView(...)` <br> Similar to `System.Memory<byte>` or `ArraySegment<byte>`. | Out of Scope | [ ] |
| `love.data.pack(format, values...)` | `Night.Data.Pack(string packFormat, params object[] values)` | `public static byte[] Pack(...)` <br> Binary packing. | Out of Scope | [ ] |
| `love.data.unpack(format, datastring)` | `Night.Data.Unpack(string packFormat, byte[] packedData)` | `public static object[] Unpack(...)` | Out of Scope | [ ] |

**Night Engine Specific Types (if module were implemented):**
*   `Night.DataContainerType`: Enum (e.g., `String`, `Data`). (Love2D distinction, less relevant for C# byte arrays).
*   `Night.CompressionFormat`: Enum (e.g., `Gzip`, `Zlib`, `Lz4`).
*   `Night.EncodingFormat`: Enum (e.g., `Base64`, `Hex`).
*   `Night.HashFunction`: Enum (e.g., `Md5`, `Sha1`, `Sha256`).
*   `Night.DataView`: Wrapper for a segment of byte array, similar to `System.Memory<byte>`.
