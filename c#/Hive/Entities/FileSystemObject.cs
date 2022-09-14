using System;
using System.Collections.Generic;

namespace FilesCleaner;

public abstract class FileSystemObject
{
    public String RootPath { get; set; }
    public String FullPath { get; set; }
    public String Name { get; set; }
    public DateTimeOffset CreationDateTime { get; set; }
    public List<FileSystemObject> Children { get; set; } = new();

    public FileSystemObject(
        String rootPath,
        String fullPath,
        String name,
        DateTimeOffset creationDateTime
        )
    {
        RootPath = rootPath;
        FullPath = fullPath;
        Name = name;
        CreationDateTime = creationDateTime;
    }
}