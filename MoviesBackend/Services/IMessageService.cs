using System.Threading.Tasks;

namespace MoviesBackend.Services
{
  public interface IMessageService
  {
    Task Send(string email, string subject, string message);
  }
}
