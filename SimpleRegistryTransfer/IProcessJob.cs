using System.Threading.Tasks;

namespace SimpleRegistryTransfer;
public interface IProcessJob
{
    public ValueTask Run();
}
