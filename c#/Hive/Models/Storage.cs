using System.Collections.Generic;
using System.IO;

namespace FilesCleaner
{
    public sealed class Storage : IFilesCollection
    {
        public List<FileSystemObject> Children { get; set; } = new();

        public File Add(FileInfo fileInfo, IFilesCollection parent)
            => new(fileInfo, parent, this);

        public IFilesCollection Add(DirectoryInfo directoryInfo, IFilesCollection parent)
            => new Folder(directoryInfo, parent, this);

        public void DeleteChild(FileSystemObject fileSystemObject)
        {
            if (Children.Contains(fileSystemObject))
                Children.Remove(fileSystemObject);
        }
    }
}