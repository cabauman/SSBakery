using System.Collections.Generic;

namespace SSBakery.Services.Interfaces
{
    public interface IAnalyticsService
    {
        void SetUserId(string id);

        void SetUserProperty(string name, string value);

        void TrackSignInStarted(string method);
    }
}