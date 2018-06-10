using SSBakery.Models;
using System;
using System.Collections.Generic;
using System.Reactive;

namespace SSBakery.Services.Interfaces
{
    public interface ICredentialsService
    {
        string FirebaseAuthJson { get; set; }

        string Provider { get; }

        string AuthToken { get; }

        bool CredentialsExist { get; }

        IObservable<Unit> Delete();

        IObservable<Unit> Set(string provider, string authToken, string firebaseAuthJson);
    }
}