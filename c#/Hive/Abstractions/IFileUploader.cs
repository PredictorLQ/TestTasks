using System.Threading.Tasks;

namespace FilesCleaner;

public interface IFileUploader
{
    Task Upload(FileSystemObject @object);
}