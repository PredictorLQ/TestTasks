using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;

namespace FilesCleaner
{
    public abstract class FileSystemObject
    {
        public FileSystemObject(TypeObject type, [NotNull] FileSystemInfo fileSystemInfo, [NotNull] IFilesCollection parent, [NotNull] Storage storage)
        {
            Type = type;
            FileSystemInfo = fileSystemInfo;
            Parent = parent;
            Storage = storage;

            Parent.Children.Add(this);

            if (!(Parent is Storage))
                Storage.Children.Add(this);
        }

        public TypeObject Type { get; }
        public FileSystemInfo FileSystemInfo { get; }
        public Storage Storage { get; }
        protected IFilesCollection Parent { get; }

        public virtual void Delete()
        {
            Parent.DeleteChild(this);
            Storage?.DeleteChild(this);
        }

        public enum TypeObject
        {
            FILE,
            FOLDER
        }
    }
}