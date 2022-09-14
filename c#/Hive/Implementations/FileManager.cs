using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading.Tasks;

namespace FilesCleaner;

public sealed class FileManager : IFileManager
{
    private readonly ILogger<FileManager> logger;

    public FileManager(ILogger<FileManager> logger)
    {
        this.logger = logger;
    }

    public async Task<FileSystemObject> Create(FileSystemInfo info, String rootPath)
    {
        return info switch
        {
            DirectoryInfo => new Folder(rootPath, info.FullName, info.Name, info.CreationTimeUtc),
            FileInfo => new File(rootPath, info.FullName, info.Name, info.CreationTimeUtc),
            _ => throw new NotSupportedException()
        };
    }

    public async Task Remove(FileSystemObject @object)
    {
        if (@object is Folder)
        {
            if (!Directory.Exists(@object.FullPath))
            {
                logger.LogWarning("Directory by path {FullPath} not found!", @object.FullPath);

                return;
            }

            logger.LogInformation("Remove directory by path {FullPath}", @object.FullPath);

            Directory.Delete(@object.FullPath, true);

            return;
        }

        if (@object is File)
        {
            if (!System.IO.File.Exists(@object.FullPath))
            {
                logger.LogWarning("File by path: {FullPath} not found!", @object.FullPath);

                return;
            }

            logger.LogInformation("Remove file by path {FullPath}", @object.FullPath);

            System.IO.File.Delete(@object.FullPath);

            return;
        }

        logger.LogCritical("Unknown object by path {FullPath}", @object.FullPath);
    }
}