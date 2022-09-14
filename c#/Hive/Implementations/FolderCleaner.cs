using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace FilesCleaner;

public sealed class FolderCleaner : IFolderCleaner
{
	private readonly IFileManager manager;
	private readonly IFileUploader uploader;
	private readonly ILogger<FolderCleaner> logger;

	public FolderCleaningResult Result { get; private set; } = new();

	public FolderCleaner(
		IFileManager manager,
		IFileUploader uploader,
		ILogger<FolderCleaner> logger
		)
	{
		this.manager = manager;
		this.uploader = uploader;
		this.logger = logger;
	}

	public async Task Clean(FileSystemObject @object, DateTimeOffset maxFileCreationDateTime, Boolean removeEmtryFolders)
	{
		if (@object is not Folder folder)
		{
			logger.LogError("Object by path {FullPath} is not {Type}", @object.FullPath, nameof(Folder));

			throw new ArgumentException();
		}

		await CleanFolder(folder, maxFileCreationDateTime, removeEmtryFolders);

		await Result.UseView();
	}

	private async Task CleanFolder(FileSystemObject folder, DateTimeOffset maxFileCreationDateTime, Boolean removeEmtryFolders)
	{
		logger.LogInformation("Read directory {FullPath} for clean", folder.FullPath);

		for (int childIndex = 0; childIndex < folder.Children.Count; childIndex++)
		{
			FileSystemObject folderChild = folder.Children[childIndex];

			if (folderChild is Folder childFolder)
			{
				await CleanFolder(childFolder, maxFileCreationDateTime, removeEmtryFolders);

				if (childFolder.Children.Count == 0 && removeEmtryFolders)
				{
					await manager.Remove(folderChild);

					folder.Children.RemoveAt(childIndex);
					childIndex--;
				}

				continue;
			}

			if (folderChild is File file && folderChild.CreationDateTime < maxFileCreationDateTime)
			{
				await uploader.Upload(folderChild);

				await manager.Remove(file);

				await Result.Incriment();

				folder.Children.RemoveAt(childIndex);
				childIndex--;

				continue;
			}

			logger.LogCritical("Unknown object by path {FullPath}", folderChild.FullPath);
		}
	}
}