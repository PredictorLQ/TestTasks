using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace FilesCleaner
{
    public sealed class Folder : FileSystemObject, IFilesCollection
    {
        public Folder([NotNull] DirectoryInfo directoryInfo, [NotNull] IFilesCollection parent, [NotNull] Storage storage)
            : base(TypeObject.FOLDER, directoryInfo, parent, storage)
        { }

        public List<FileSystemObject> Children { get; private set; } = new();

        public override void Delete()
        {
            if (Directory.Exists(FileSystemInfo.FullName))
                Directory.Delete(FileSystemInfo.FullName, true);

            base.Delete();
        }

        public void DeleteChild(FileSystemObject fileSystemObject)
        {
            if (Children.Contains(fileSystemObject))
                Children.RemoveAll(u=> u == fileSystemObject);

            if (Children.Count > 0)
                return;

            Delete();
        }
    }
}