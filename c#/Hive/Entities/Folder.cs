using System;

namespace FilesCleaner;

public sealed class Folder : FileSystemObject
{
    public Folder(String rootPath, String fullPath, String name, DateTimeOffset creationDateTime)
        : base(rootPath, fullPath, name, creationDateTime)
    { }
}