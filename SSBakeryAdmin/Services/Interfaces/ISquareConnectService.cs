using System.Collections.Generic;
using System.Threading.Tasks;
using SSBakery.Models;

namespace SSBakeryAdmin.Services.Interfaces
{
    public interface ISquareConnectService
    {
        Task<IList<SSBakeryUser>> RetrieveCustomerData();
    }
}