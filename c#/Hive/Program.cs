using System;
using System.IO;

namespace FilesCleaner
{
    public sealed class Program
    {
        private const string _pathToDirectory = "";

        static void Main(string[] args)
        {
            if (args.Length < 1 || string.IsNullOrWhiteSpace(args[0]))
                throw new Exception("First argument not found or empty!");

            if(!DateTime.TryParse(args[0], out var deathDateUtc))
                throw new Exception($"First argument {args[0]} has a invalid date's format!");

            var startPath = Path.Combine(Environment.CurrentDirectory, _pathToDirectory);

            if(!Directory.Exists(startPath))
                throw new Exception($"Directory by path {startPath} not found!");

            var storage = new Storage();
            var reader = new Reader(startPath, storage);
            var cleaner = new Cleaner(deathDateUtc, storage);

            reader.Handle();
            cleaner.Handle();
        }
    }
}