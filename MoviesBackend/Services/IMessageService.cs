using System.Threading.Tasks;

namespace MoviesBackend.Services
{
  interface IMessageService
  {
    Task Send(string email, string subject, string message);
  }
}
