namespace dotnetVEE.Abstractions.Serialization
{
    /// <summary>
    /// Represents a class whose contents can be serialized into
    /// a binary writer.
    /// </summary>
    public interface IBinarySerializable
    {
        /// <summary>
        /// Serializes a class to a <see cref="BinaryReader" />.
        /// </summary>
        /// <param name="bw"><see cref="BinaryWriter" /> where binary data will serialize to.</param>
        void BinarySerialize(BinaryWriter bw);
    }
}
