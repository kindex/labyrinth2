using System;
using System.IO;
using System.Collections.Generic;

namespace Game.Storage
{
    public interface IArchive
    {
        bool HasFile(string filename);

        DateTime GetModDateTime(string filename);
        Stream GetFile(string filename);

        int FileCount { get; }
        IEnumerable<string> FileNames { get; }
    }
}
