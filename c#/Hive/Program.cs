using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FilesCleaner;

public class Program
{
	private const String directoryPath = "./data";

	static async Task Main(string[] _)
	{
		ILoggerFactory loggerFactory = LoggerFactory.Create(builder =>
		{
			builder.ClearProviders();
			builder.AddSimpleConsole(options =>
			{
				options.IncludeScopes = true;
				options.SingleLine = true;
				options.TimestampFormat = "yyyy-MM-dd hh:mm:ss ";
			}).SetMinimumLevel(LogLevel.Information);
		});

		ILogger<Program> programmLogger = loggerFactory.CreateLogger<Program>();

		programmLogger.LogInformation("Run app");

		TimeSpan fileLifetimeDuration = TimeSpan.FromSeconds(Int32.Parse(Environment.GetEnvironmentVariable("FILE_LIFETIME_DURATION_IN_SECONDS") ?? "600"));
		TimeSpan raceSleepDuration = TimeSpan.FromHours(Int32.Parse(Environment.GetEnvironmentVariable("RACE_SLEEP_DURATION_IN_HOURS") ?? "6"));

		SelectelHttpApi selectelHttpApi = new SelectelHttpApi(
			new(
				accountNumber: Environment.GetEnvironmentVariable("SELECTEL_CLOUD_STORAGE_API_ACCOUNT_NUMBER") ?? "",
				payloadUser: Environment.GetEnvironmentVariable("SELECTEL_CLOUD_STORAGE_API_PAYLOAD_USER") ?? "",
				payloadPassword: Environment.GetEnvironmentVariable("SELECTEL_CLOUD_STORAGE_API_PAYLOAD_PASSWORD") ?? "",
				payloadContainer: Environment.GetEnvironmentVariable("SELECTEL_CLOUD_STORAGE_API_PAYLOAD_CONTAINER") ?? ""
			),
			loggerFactory.CreateLogger<SelectelHttpApi>()
		);

		SelectelFileUploader selectelUploader = new SelectelFileUploader(
			selectelHttpApi,
			loggerFactory.CreateLogger<SelectelFileUploader>()
		);

		IFileUploader fileUploader = selectelUploader;

		IFileManager fileManager = new FileManager(
			loggerFactory.CreateLogger<FileManager>()
		);

		IFolderReader fileReader = new FolderReader(
			fileManager,
			loggerFactory.CreateLogger<FolderReader>()
		);

		IFolderCleaner fileCleaner = new FolderCleaner(
			fileManager,
			fileUploader,
			loggerFactory.CreateLogger<FolderCleaner>()
		);

		while (true)
		{
			programmLogger.LogInformation("Run race");

			DateTimeOffset minFileCreationDateTime = DateTimeOffset.UtcNow.AddSeconds(-fileLifetimeDuration.TotalSeconds);

			await selectelUploader.Initialize();

			FileSystemObject @object = await fileReader.Read(directoryPath);

			await fileCleaner.Clean(@object, minFileCreationDateTime, true);

			programmLogger.LogInformation("Sleep race on {raceSleepDuration} before {raceAwakDuration}", raceSleepDuration, DateTimeOffset.UtcNow.Add(raceSleepDuration));

			Thread.Sleep(raceSleepDuration);
		}
	}
}