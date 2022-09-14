using System;
using System.Threading.Tasks;

namespace FilesCleaner;

public sealed class FolderCleaningResult
{
	public Int32 RemovedFileCount { get; private set; }

	public async Task Incriment()
	{
		RemovedFileCount++;
	}

	public async Task UseView()
	{
		Console.WriteLine($"Count files removed: {RemovedFileCount}");
	}
}