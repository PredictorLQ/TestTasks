using System;

namespace FilesCleaner;

public sealed class File : FileSystemObject
{
    public File(String rootPath, String fullPath, String name, DateTimeOffset creationDateTime)
        : base(rootPath, fullPath, name, creationDateTime)
    { }
}