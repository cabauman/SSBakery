using System;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using SSBakery.Services.Interfaces;
using Xamarin.Auth;

namespace SSBakery.Services
{
    public class XamarinAuthCredentialsService : ICredentialsService
    {
        private const string SERVICE_ID = "SSBakery";

        private Account _account;

        public XamarinAuthCredentialsService()
        {
            _account = AccountStore.Create().FindAccountsForService(SERVICE_ID).FirstOrDefault();
        }

        public bool CredentialsExist
        {
            get { return _account != null; }
        }

        public string FirebaseAuthJson
        {
            get
            {
                return _account?.Properties["FirebaseAuthJson"];
            }

            set
            {
                if(_account != null)
                {
                    _account.Properties["FirebaseAuthJson"] = value;
                    AccountStore.Create().Save(_account, SERVICE_ID);
                }
            }
        }

        public string Provider
        {
            get { return _account?.Properties["Provider"]; }
        }

        public string AuthToken
        {
            get { return _account?.Properties["AuthToken"]; }
        }

        public IObservable<Unit> Delete()
        {
            return AccountStore
                .Create()
                .DeleteAsync(_account, SERVICE_ID)
                .ToObservable();
        }

        public IObservable<Unit> Set(string provider, string authToken, string firebaseAuthJson)
        {
            _account = new Account();

            _account.Properties["Provider"] = provider;
            _account.Properties["AuthToken"] = authToken;
            _account.Properties["FirebaseAuthJson"] = firebaseAuthJson;

            return Save();
        }

        private IObservable<Unit> Save()
        {
            return AccountStore
                .Create()
                .SaveAsync(_account, SERVICE_ID)
                .ToObservable();
        }
    }
}