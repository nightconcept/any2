namespace Night.Filesystem
{
    /// <summary>
    /// Specifies how a file's buffer is flushed.
    /// </summary>
    public enum BufferMode
    {
        /// <summary>
        /// No buffering. Data is written as soon as possible.
        /// </summary>
        None,

        /// <summary>
        /// Line buffering. Data is written when a newline character is output, or when the buffer is full.
        /// </summary>
        Line,

        /// <summary>
        /// Full buffering. Data is written only when the buffer is full.
        /// </summary>
        Full
    }
}
