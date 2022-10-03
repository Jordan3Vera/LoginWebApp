using System.Threading.Tasks;
using WebApiRest.Model.DTOs;

namespace WebApiRest.Helper
{
    public interface IStoreHub
    {
        Task NotifyNewInfo(CustomerOrderDTO order);
    }
}
