using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading.Tasks;

namespace FilesCleaner;

public sealed class FolderReader : IFolderReader
{
	private readonly IFileManager manager;
	private readonly ILogger<FolderReader> logger;

	public FolderReader(
		IFileManager manager,
		ILogger<FolderReader> logger
		)
	{
		this.manager = manager;
		this.logger = logger;
	}

	public async Task<FileSystemObject> Read(String fullPath)
	{
		return await ReadFolder("", fullPath);
	}

	private async Task<FileSystemObject> ReadFolder(String rootPath, String fullPath)
	{
		logger.LogInformation("Read directory with path {fullPath}", fullPath);

		DirectoryInfo directoryInfo = new DirectoryInfo(fullPath);
		FileSystemObject @object = await manager.Create(directoryInfo, rootPath);

		foreach (FileInfo file in directoryInfo.GetFiles())
		{
			logger.LogInformation("Add new file with name \"{Name}\" by path {FullName}", file.Name, file.FullName);

			@object.Children.Add(await manager.Create(file, rootPath + $"/{file.Name}"));
		}

		foreach (DirectoryInfo directory in directoryInfo.GetDirectories())
		{
			FileSystemObject directoryObject = await ReadFolder(rootPath + $"/{directory.Name}", directory.FullName);

			@object.Children.Add(directoryObject);
		}

		return @object;
	}
}