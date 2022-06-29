using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace FilesCleaner
{
    public sealed class File : FileSystemObject
    {
        public File([NotNull] FileInfo fileInfo, [NotNull] IFilesCollection parent, [NotNull] Storage storage)
            : base(TypeObject.FILE, fileInfo, parent, storage)
        { }

        public override void Delete()
        {
            if (System.IO.File.Exists(FileSystemInfo.FullName))
                System.IO.File.Delete(FileSystemInfo.FullName);

            base.Delete();
        }
    }
}