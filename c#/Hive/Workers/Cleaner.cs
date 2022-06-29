using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace FilesCleaner
{
    public sealed class Cleaner
    {
        private readonly DateTime _deathDateUtc;

        public Cleaner(DateTime deathDateUtc, [NotNull] Storage storage)
            => (_deathDateUtc, _storage) = (deathDateUtc, storage);

        private Storage _storage { get; set; }

        public void Handle()
        {
            bool CreateSearchPredicate(FileSystemObject fileSystemObject)
                => fileSystemObject.Type switch
                {
                    FileSystemObject.TypeObject.FOLDER => ((Folder)fileSystemObject).Children.Count == 0,
                    FileSystemObject.TypeObject.FILE or _=> fileSystemObject.FileSystemInfo.CreationTimeUtc <= _deathDateUtc,
                };

            _storage.Children
                .Where(CreateSearchPredicate)
                .ToList()
                .ForEach(u => u.Delete());
        }
    }
}