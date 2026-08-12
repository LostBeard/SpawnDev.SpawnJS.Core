namespace SpawnDev.SpawnJS.Marshallers
{
    /// <summary>
    /// A view of the dotnet heap
    /// </summary>
    public class HeapViewDescriptor
    {
        /// <summary>
        /// Memory offset
        /// </summary>
        public long Offset { get; set; }
        /// <summary>
        /// Length in bytes
        /// </summary>
        public long Length { get; set; }
        /// <summary>
        /// Revive as this global type</br>
        /// Revives as a Uint8Array by default
        /// </summary>
        public string? Type { get; set; }
        public HeapViewDescriptor(long offset, long length, string? type = null)
        {
            Offset = offset;
            Length = length;
            Type = type;
        }
    }
}
