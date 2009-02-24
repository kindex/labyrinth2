using System;
using System.IO;
using System.Collections.Generic;

namespace Game.Storage
{
    public sealed class FileSystem
    {
        public FileSystem()
        {
            //if (Directory.Exists(userdir) == false)
            //{
            //    Directory.CreateDirectory(userdir);
            //}

            this.archives = new List<IArchive>();
            //this.archives.Add(new DirectoryArchive(userdir));
            //this.archives.Add(new DirectoryArchive(Path.GetDirectoryName(Application.ExecutablePath)));
        }

        public Stream GetFile(string filename)
        {
            foreach (IArchive archive in archives)
            {
                if (archive.HasFile(filename))
                {
                    return archive.GetFile(filename);
                }
            }

            throw new FileNotFoundException();
        }

        //public Stream CreateFile(string filename)
        //{
        //    string file = Path.Combine(userdir, filename);
        //    return new FileStream(file, FileMode.Create, FileAccess.Write, FileShare.None);
        //}

        public void AddZipFile(string zipfile)
        {
            IArchive last = archives[archives.Count - 1];
            archives[archives.Count - 1] = new Zip.ZipArchive(new FileStream(zipfile, FileMode.Open, FileAccess.Read, FileShare.Read));
            archives.Add(last);
        }

        public void AddDirectory(string directory)
        {
            IArchive last = archives[archives.Count - 1];
            archives[archives.Count - 1] = new DirectoryArchive(directory);
            archives.Add(last);
        }

        public IEnumerable<string> EnumDirectory(string directory)
        {
            if (directory.EndsWith("/") == false)
            {
                directory = directory + "/";
            }

            foreach (IArchive archive in archives)
            {
                foreach (string filename in archive.FileNames)
                {
                    if (filename.StartsWith(directory))
                    {
                        yield return filename.Remove(0, directory.Length);
                    }
                }
            }
        }

        public bool HasFile(string filename)
        {
            foreach (IArchive archive in archives)
            {
                if (archive.HasFile(filename))
                {
                    return true;
                }
            }

            return false;
        }

        public int FileCount
        {
            get
            {
                int result = 0;
                foreach (IArchive archive in archives)
                {
                    result += archive.FileCount;
                }
                return result;
            }
        }

        public IEnumerable<string> FileNames
        {
            get
            {
                foreach (IArchive archive in archives)
                {
                    foreach (string filename in archive.FileNames)
                    {
                        yield return filename;
                    }
                }
            }
        }

        //private static string userdir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), 
        //                                             Path.Combine(Application.CompanyName, Application.ProductName));
        private List<IArchive> archives;
    }
}
