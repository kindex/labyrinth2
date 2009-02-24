using System;
using System.IO;
using System.Collections.Generic;

namespace Game.Storage.Zip
{
    public sealed class ZipStreamStored : Stream
    {
        public ZipStreamStored(Stream zipFile, long offset, long length)
        {
            this.zipFile = zipFile;
            this.offset = offset;
            this.length = length;
        }

        public override void Flush()
        {
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            switch (origin)
            {
                case SeekOrigin.Begin:
                    this.position = offset;
                    break;

                case SeekOrigin.Current:
                    this.position += offset;
                    break;

                case SeekOrigin.End:
                    this.position = this.length + offset;
                    break;
            }

            if (this.position < 0 || this.position > this.length)
            {
                throw new IOException();
            }

            return this.position;
        }

        public override void SetLength(long value)
        {
            throw new NotSupportedException();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            zipFile.Seek(this.offset + this.position, SeekOrigin.Begin);
            int result = zipFile.Read(buffer, offset, count);
            this.position += result;
            return result;
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotSupportedException();
        }

        public override bool CanRead { get { return true; } }

        public override bool CanSeek { get { return true; } }

        public override bool CanWrite { get { return false; } }

        public override long Length { get { return length; } }

        public override long Position
        {
            get
            {
                return position;
            }
            set
            {
                throw new NotSupportedException();
            }
        }

        private Stream zipFile;
        private long offset;
        private long length;
        private long position;
    }
}
