using System;
using System.IO;
using System.IO.Compression;
using System.Collections.Generic;

namespace Game.Storage.Zip
{
    public class ZipStreamDeflated : Stream
    {
        public ZipStreamDeflated(Stream zipFile, long offset, long compressedSize, long uncompressedSize)
        {
            this.inflater = new DeflateStream(zipFile, CompressionMode.Decompress, true);
            this.offset = offset;
            this.compressedSize = compressedSize;
            this.uncompressedSize = uncompressedSize;
        }

        public override void Close()
        {
            base.Close();
            this.inflater.Close();
        }

        public override void Flush()
        {
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotSupportedException();
        }

        public override void SetLength(long value)
        {
            throw new NotSupportedException();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            inflater.BaseStream.Seek(this.offset + this.compressedPosition, SeekOrigin.Begin);
            int result = inflater.Read(buffer, offset, count);
            this.uncompressedPosition += result;
            this.compressedPosition = inflater.BaseStream.Position - this.offset;
            return result;
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotSupportedException();
        }

        public override bool CanRead { get { return true; } }

        public override bool CanSeek { get { return false; } }

        public override bool CanWrite { get { return false; } }

        public override long Length { get { return uncompressedSize; } }

        public override long Position
        {
            get
            {
                return uncompressedPosition;
            }
            set
            {
                throw new NotSupportedException();
            }
        }

        private DeflateStream inflater;
        private long offset;
        private long compressedSize;
        private long uncompressedSize;
        private long compressedPosition;
        private long uncompressedPosition;
    }
}
