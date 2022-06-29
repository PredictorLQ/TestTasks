using System.Collections.Generic;

namespace FilesCleaner
{
    public interface IFilesCollection
    {
        public List<FileSystemObject> Children { get; }
        void DeleteChild(FileSystemObject fileSystemObject);
    }
}