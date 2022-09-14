using System;
using System.Threading.Tasks;

namespace FilesCleaner;

public interface IFolderReader
{
    Task<FileSystemObject> Read(String fullPath);
}