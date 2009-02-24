using System;
using System.IO;
using System.Collections.Generic;

namespace Game.Storage
{
    public sealed class DirectoryArchive : IArchive
    {
        public DirectoryArchive(string path)
        {
            this.basepath = Path.GetFullPath(path);
        }

        public bool HasFile(string filename)
        {
            return File.Exists(Path.Combine(basepath, filename.Replace('/', Path.DirectorySeparatorChar)));
        }

        public DateTime GetModDateTime(string filename)
        {
            if (HasFile(filename))
            {
                string fullpath = Path.Combine(this.basepath, filename.Replace('/', Path.DirectorySeparatorChar));
                return File.GetLastWriteTime(fullpath);
            }

            throw new FileNotFoundException();
        }

        public Stream GetFile(string filename)
        {
            if (HasFile(filename))
            {
                string fullpath = Path.Combine(this.basepath, filename.Replace('/', Path.DirectorySeparatorChar));
                return new FileStream(fullpath, FileMode.Open, FileAccess.Read, FileShare.Read);
            }

            throw new FileNotFoundException();
        }

        public int FileCount
        {
            get
            {
                return Directory.GetFiles(basepath, "*", SearchOption.AllDirectories).Length;
            }
        }

        public IEnumerable<string> FileNames
        {
            get
            {
                foreach (string filename in Directory.GetFiles(basepath, "*", SearchOption.AllDirectories))
                {
                    yield return filename.Remove(0, basepath.Length + 1).Replace(Path.DirectorySeparatorChar, '/');
                }
            }
        }

        private string basepath;
    }
}
