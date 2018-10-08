using System;
using GameCtor.Repository;
using SSBakery.Models;

namespace SSBakery.Repositories.Interfaces
{
    public interface IAdminVarRepo : IRepository<AdminVar>
    {
        IObservable<AdminVar> GetInstance();
    }
}
