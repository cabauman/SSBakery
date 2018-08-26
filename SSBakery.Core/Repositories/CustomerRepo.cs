using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using Firebase.Database;
using GameCtor.FirebaseDatabase.DotNet;
using Square.Connect.Api;
using Square.Connect.Model;
using SSBakery.Repositories.Interfaces;

namespace SSBakery.Repositories
{
    public class CustomerRepo : FirebaseOfflineRepo<SSBakery.Models.SSBakeryUser>, ICustomerRepo
    {
        public CustomerRepo(FirebaseClient client, string path, string key = null)
            : base(client, path, key)
        {
        }

        public IObservable<Unit> PullFromPosSystemAndStoreInFirebase(string beginTime = null, int? limit = null)
        {
            var customersApi = new CustomersApi();
            var filter = new CustomerFilter(UpdatedAt: new TimeRange(beginTime));
            var query = new CustomerQuery(filter);
            var request = new SearchCustomersRequest(Query: query);

            return customersApi
                .SearchCustomersAsync(request)
                .ToObservable()
                .SelectMany(x => x.Customers)
                .Select(MapDtoToModel)
                .ToList()
                .SelectMany(items => Upsert(items));
        }

        private SSBakery.Models.SSBakeryUser MapDtoToModel(Customer dto)
        {
            return new SSBakery.Models.SSBakeryUser()
            {
                Id = dto.Id,
                Name = dto.GivenName + " " + dto.FamilyName,
                PhoneNumber = dto.PhoneNumber
            };
        }
    }
}
