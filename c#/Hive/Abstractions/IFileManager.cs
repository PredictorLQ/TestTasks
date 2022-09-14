using System;
using System.IO;
using System.Threading.Tasks;

namespace FilesCleaner;

public interface IFileManager
{
    Task<FileSystemObject> Create(FileSystemInfo info, String rootPath);
    Task Remove(FileSystemObject info);
}