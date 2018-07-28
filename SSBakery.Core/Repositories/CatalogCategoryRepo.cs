using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using Square.Connect.Api;
using Square.Connect.Model;
using SSBakery.Repositories.Interfaces;

namespace SSBakery.Repositories
{
    public class CatalogCategoryRepo
    {
        public CatalogCategoryRepo()
        {
        }

        public IObservable<IEnumerable<SSBakery.Models.CatalogCategory>> GetAll(bool syncWithPos)
        {
            return Observable
                .Return(syncWithPos ? PullFromPosAndStoreInFirebase() : PullFromFirebase())
                .SelectMany(x => x);
        }

        private IObservable<IEnumerable<SSBakery.Models.CatalogCategory>> PullFromPosAndStoreInFirebase()
        {
            var firebaseRepo = new FirebaseOfflineRepo<SSBakery.Models.CatalogCategory>(null, null);
            var catalogApi = new CatalogApi();

            return catalogApi
                .ListCatalogAsync(null, "CATEGORY")
                .ToObservable()
                .SelectMany(x => x.Objects)
                .Select(MapDtoToModel)
                .ToList()
                .SelectMany(categories => firebaseRepo.Populate(categories).Select(_ => categories));
        }

        private IObservable<IEnumerable<SSBakery.Models.CatalogCategory>> PullFromFirebase()
        {
            var firebaseRepo = new FirebaseOfflineRepo<SSBakery.Models.CatalogCategory>(null, null);

            return firebaseRepo
                .GetAll()
                .ToList();
        }

        private SSBakery.Models.CatalogCategory MapDtoToModel(CatalogObject dto)
        {
            return new SSBakery.Models.CatalogCategory()
            {
                Id = dto.Id,
                Name = dto.CategoryData.Name
            };
        }
    }
}
