using System;
using System.Threading.Tasks;

namespace SSBakery.Services.Interfaces
{
    public interface IFirebaseStorageService
    {
        IObservable<string> GetDownloadUrl(string filename);
    }
}