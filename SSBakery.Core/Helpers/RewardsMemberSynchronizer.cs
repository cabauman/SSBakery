using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Xml;
using DynamicData;
using GameCtor.Repository;
using Splat;
using Square.Connect.Api;
using Square.Connect.Model;
using SSBakery.Models;
using SSBakery.Repositories.Interfaces;

namespace SSBakery.Helpers
{
    public class RewardsMemberSynchronizer : IRewardsMemberSynchronizer
    {
        private IRepository<RewardsMember> _rewardsMemberRepo;
        private IAdminVarRepo _adminVarRepo;

        public RewardsMemberSynchronizer(
            IRepository<RewardsMember> rewardsMemberRepo = null,
            IAdminVarRepo adminVarRepo = null)
        {
            _rewardsMemberRepo = rewardsMemberRepo ?? Locator.Current.GetService<IRepository<RewardsMember>>();
            _adminVarRepo = adminVarRepo ?? Locator.Current.GetService<IAdminVarRepo>();
        }

        public IObservable<Unit> PullFromPosSystemAndStoreInFirebase(ISourceCache<RewardsMember, string> rewardsMemberCache)
        {
            return GetTimestampOfLatestSync()
                .SelectMany(beginTime => Sync(beginTime, rewardsMemberCache))
                .Concat(SaveTimestampOfLatestSync());
        }

        public IObservable<Unit> Sync(string beginTime, ISourceCache<RewardsMember, string> rewardsMemberCache)
        {
            var customersApi = new CustomersApi();
            var filter = new CustomerFilter(UpdatedAt: new TimeRange(beginTime));
            var query = new CustomerQuery(filter);

            return GetRecentlyUpdatedCustomers(customersApi, query, null)
                .Where(x => x != null)
                .SelectMany(x => x)
                .Where(customer => !string.IsNullOrWhiteSpace(customer.PhoneNumber))
                .Select(customer => HandlePotentialCustomerMerge(customer, rewardsMemberCache))
                .SelectMany(rewardsMembers => _rewardsMemberRepo.Upsert(rewardsMembers))
                .LastOrDefaultAsync();
        }

        private IObservable<IList<Customer>> GetRecentlyUpdatedCustomers(CustomersApi customersApi, CustomerQuery query, string cursor)
        {
            var request = new SearchCustomersRequest(Cursor: cursor, Query: query);

            return customersApi
                .SearchCustomersAsync(request)
                .ToObservable()
                .SelectMany(
                    response =>
                    {
                        if(response.Cursor == null)
                        {
                            return Observable
                                .Return(response.Customers);
                        }
                        else
                        {
                            return Observable
                                .Return(response.Customers)
                                .Concat(GetRecentlyUpdatedCustomers(customersApi, query, response.Cursor));
                        }
                    });
        }

        private RewardsMember HandlePotentialCustomerMerge(Customer customer, ISourceCache<RewardsMember, string> rewardsMemberCache)
        {
            var lookupResult = rewardsMemberCache.Lookup(customer.Id);
            var isNew = !lookupResult.HasValue;
            var rewardsMember = isNew ? null : lookupResult.Value;
            if(isNew)
            {
                rewardsMember = MapDtoToModel(customer);
                rewardsMemberCache.AddOrUpdate(rewardsMember);
            }
            else
            {
                rewardsMember.Name = customer.GivenName + " " + customer.FamilyName;
                rewardsMember.PhoneNumber = customer.PhoneNumber;
            }

            //if(isNew && customer.CreationSource == Customer.CreationSourceEnum.MERGE)
            //{
            //    foreach(var member in rewardsMemberCache.Items)
            //    {
            //        if(member.PhoneNumber == customer.PhoneNumber && member.Id != customer.Id)
            //        {
            //            rewardsMember.TotalVisits += member.TotalVisits;
            //            rewardsMember.UnclaimedRewardCount += member.UnclaimedRewardCount;
            //        }
            //    }
            //}

            return rewardsMember;
        }

        private RewardsMember MapDtoToModel(Customer dto)
        {
            return new RewardsMember()
            {
                Id = dto.Id,
                Name = dto.GivenName + " " + dto.FamilyName,
                PhoneNumber = dto.PhoneNumber,
            };
        }

        private IObservable<string> GetTimestampOfLatestSync()
        {
            return _adminVarRepo
                .GetInstance()
                .Select(x => x.CustomerSyncTimestamp);
        }

        private IObservable<Unit> SaveTimestampOfLatestSync()
        {
            return _adminVarRepo
                .GetInstance()
                .Do(
                    x =>
                    {
                        var timestamp = XmlConvert.ToString(DateTime.UtcNow, XmlDateTimeSerializationMode.Utc);
                        x.CustomerSyncTimestamp = timestamp;
                    })
                .SelectMany(
                    x =>
                    {
                        return _adminVarRepo.Upsert(x);
                    });
        }
    }
}
