using System;
using System.Threading.Tasks;

namespace FilesCleaner;

public interface IFolderCleaner
{
	Task Clean(FileSystemObject @object, DateTimeOffset maxFileCreationDateTime, Boolean removeEmtryFolders);
}