using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace FilesCleaner;

public sealed class SelectelFileUploader : IFileUploader
{
	private readonly SelectelHttpApi api;
	private readonly ILogger<SelectelFileUploader> logger;

	public SelectelFileUploader(
		SelectelHttpApi httpApi,
		ILogger<SelectelFileUploader> uploaderLogger
		)
	{
		api = httpApi;
		logger = uploaderLogger;
	}

	public async Task Initialize()
	{
		await api.Authorization();
	}

	public async Task Upload(FileSystemObject @object)
	{
		if (!System.IO.File.Exists(@object.FullPath))
		{
			logger.LogError("File by path: {FullPath} not found!", @object.FullPath);

			throw new ArgumentException();
		}

		StreamContent stream = new StreamContent(new FileStream(@object.FullPath, FileMode.Open));

		await api.UploadFile(stream, @object.RootPath);
	}
}