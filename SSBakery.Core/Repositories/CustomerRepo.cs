﻿using Square.Connect.Api;
using Square.Connect.Model;
using SSBakery.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;

namespace SSBakery.Repositories
{
    public class CustomerRepo : IRepository<Customer>
    {
        public CustomerRepo()
        {
        }

        public IObservable<Unit> Add(Customer obj)
        {
            throw new NotImplementedException();
        }

        public IObservable<Unit> Delete(string id)
        {
            throw new NotImplementedException();
        }

        public IObservable<Customer> Get(string id)
        {
            var customersApi = new CustomersApi();

            return customersApi
                .RetrieveCustomerAsync(id)
                .ToObservable()
                .Select(x => x.Customer);
        }

        public IObservable<IEnumerable<Customer>> GetAll(bool forceRefresh = false)
        {
            var customersApi = new CustomersApi();

            return customersApi
                .ListCustomersAsync()
                .ToObservable()
                .Select(x => x.Customers);
        }

        public IObservable<Unit> Update(Customer obj)
        {
            throw new NotImplementedException();
        }
    }
}