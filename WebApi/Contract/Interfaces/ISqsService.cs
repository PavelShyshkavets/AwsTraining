using System.Threading.Tasks;
using Contract.Model;

namespace Contract.Interfaces
{
    public interface ISqsService
    {
        Task SendMessage(Book book, MessageType messageType);
    }
}
