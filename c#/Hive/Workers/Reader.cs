using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace FilesCleaner
{
    public sealed class Reader
    {
        private readonly string _startRead;

        public Reader([NotNull] string startRead, [NotNull] Storage storage)
            => (_startRead, _storage) = (startRead, storage);

        private Storage _storage { get; set; }

        public void Handle()
        {
            if (!Directory.Exists(_startRead))
                throw new ArgumentException();

            ReadAsTree(_startRead, _storage);
        }

        private void ReadAsTree(string pathDirectory, IFilesCollection parent)
        {
            var directoryInfo = new DirectoryInfo(pathDirectory);
            var directoryParent = _storage.Add(directoryInfo, parent);

            var files = directoryInfo.GetFiles();
            var directories = directoryInfo.GetDirectories();

            for (int i = 0; i < files.Length; i++)
                _storage.Add(files[i], directoryParent);

            for (int i = 0; i < directories.Length; i++)
                ReadAsTree(directories[i].FullName, directoryParent);
        }
    }
}