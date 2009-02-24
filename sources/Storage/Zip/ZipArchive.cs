using System;
using System.IO;
using System.Text;
using System.Collections.Generic;

namespace Game.Storage.Zip
{
    public sealed class ZipArchive : IArchive
    {
        public ZipArchive(Stream zipFile)
        {
            this.zipFile = new BinaryReader(zipFile);

            for (; ; )
            {
                try
                {
                    UInt32 signature = this.zipFile.ReadUInt32();

                    if (signature != 0x04034b50)
                    {
                        break;
                    }

                    UInt16 version = this.zipFile.ReadUInt16();
                    UInt16 flags = this.zipFile.ReadUInt16();
                    UInt16 compressionMethod = this.zipFile.ReadUInt16();
                    UInt16 lastModFileTime = this.zipFile.ReadUInt16();
                    UInt16 lastModFileDate = this.zipFile.ReadUInt16();
                    UInt32 crc32 = this.zipFile.ReadUInt32();
                    UInt32 compressedSize = this.zipFile.ReadUInt32();
                    UInt32 uncompressedSize = this.zipFile.ReadUInt32();
                    UInt16 filenameLength = this.zipFile.ReadUInt16();
                    UInt16 extraFieldLength = this.zipFile.ReadUInt16();

                    string filename;

                    if ((flags & (1 << 11)) == 0)
                    {
                        filename = ASCIIEncoding.ASCII.GetString(this.zipFile.ReadBytes(filenameLength));
                    }
                    else
                    {
                        filename = UnicodeEncoding.UTF8.GetString(this.zipFile.ReadBytes(filenameLength));
                    }

                    if (extraFieldLength != 0)
                    {
                        this.zipFile.BaseStream.Seek(extraFieldLength, SeekOrigin.Current);
                    }

                    if ((flags & (1 << 3)) != 0)
                    {
                        crc32 = this.zipFile.ReadUInt32();
                        compressedSize = this.zipFile.ReadUInt32();
                        uncompressedSize = this.zipFile.ReadUInt32();
                    }

                    uint fileDataPosition = (uint)this.zipFile.BaseStream.Position;

                    this.zipFile.BaseStream.Seek(compressedSize, SeekOrigin.Current);

                    if (filename.EndsWith("/") == false && (flags & (1 << 0)) == 0)
                    {
                        int year = 1980 + ((lastModFileDate >> 9) & 0x7F);
                        int month = ((lastModFileDate >> 5) & 0x0F);
                        int day = ((lastModFileDate >> 0) & 0x1F);

                        int hour = (lastModFileTime >> 11) & 0x1F;
                        int minute = (lastModFileTime >> 5) & 0x3F;
                        int second = (lastModFileTime << 1) & 0x3E;

                        ZipFileInfo fileInfo = new ZipFileInfo();
                        fileInfo.compressionMethod = compressionMethod;
                        fileInfo.fileDataPosition = fileDataPosition;
                        fileInfo.compressedSize = compressedSize;
                        fileInfo.uncompressedSize = uncompressedSize;
                        fileInfo.datetime = new DateTime(year, month, day, hour, minute, second);

                        this.files.Add(filename, fileInfo);
                    }

                }
                catch (EndOfStreamException)
                {
                    break;
                }
            }
        }

        public bool HasFile(string filename)
        {
            return files.ContainsKey(filename);
        }

        public DateTime GetModDateTime(string filename)
        {
            if (files.ContainsKey(filename))
            {
                return files[filename].datetime;
            }

            throw new FileNotFoundException();
        }

        public Stream GetFile(string filename)
        {
            if (files.ContainsKey(filename))
            {
                return GetFile(files[filename]);
            }

            throw new FileNotFoundException();
        }

        private Stream GetFile(ZipFileInfo file)
        {
            if (file.compressionMethod == 0)
            {
                return new ZipStreamStored(zipFile.BaseStream, file.fileDataPosition, file.uncompressedSize);
            }
            else if (file.compressionMethod == 8)
            {
                return new ZipStreamDeflated(zipFile.BaseStream, file.fileDataPosition, file.compressedSize, file.uncompressedSize);
            }
            else
            {
                throw new IOException();
            }
        }

        public int FileCount
        {
            get
            {
                return files.Count;
            }
        }

        public IEnumerable<string> FileNames
        {
            get
            {
                foreach (var file in files)
                {
                    yield return file.Key;
                }
            }

        }

        private struct ZipFileInfo
        {
            public int compressionMethod;
            public uint fileDataPosition;
            public uint compressedSize;
            public uint uncompressedSize;
            public DateTime datetime;
        }

        private Dictionary<string, ZipFileInfo> files = new Dictionary<string, ZipFileInfo>();
        private BinaryReader zipFile;
    }
}
