using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using ContosoFinancialServices.Common;
using ContosoFinancialServices.DataModel;
using Windows.ApplicationModel;
using Windows.Storage;
using Windows.Storage.Streams;

namespace ContosoFinancialServices.DataSource
{
    class CustomerDataSource
    {
        private static CustomerDataSource _customerDataSource = new CustomerDataSource();

        private List<Customer> _allCustomers = null;

        public List<Customer> AllCustomers
        {
            get { return this._allCustomers; }

        }

        /// <summary>
        /// This method is used to create dummy customers list.
        /// </summary>
        private async Task PrepareDummyData()
        {
            try
            {
                //If customer list is already loaded from XML file, skip reloading it again. 
                if (_customerDataSource.AllCustomers != null)
                    return;

                _allCustomers = new List<Customer>();
                for (int i=0; i<7; i++)
                {
                    Customer customer = new Customer()
                    {
                        CustomerId = i.ToString(),
                        CustomerName = "Customer" + i.ToString(),
                        AccountNumber = "Account" + i.ToString(),
                        AnnualIncome = "100000",
                        ClientSince = DateTime.Now.AddDays(-1*i).ToString(),
                        CustomerAddress = "Address"+i.ToString(),
                        ClientType = "Type" + i.ToString(),
                        CustomerEmail = "Email" + i.ToString() + "@hotmail.com",
                        CustomerImage = "Data/CustomerImages/placeholder.jpg",
                        CustomerPhone = "111-111-1111",
                        MaritalStatus = "Single",
                        Occupation = "Employee",
                        ProductSummary = new List<string> {
                            "00-001", "00-002"
                        }
                    };
                    _allCustomers.Add(customer);
                }
            }
            catch
            {
                _allCustomers = null;
            }
        }

        /// <summary>
        /// Gets the customer list.
        /// </summary>
        /// <returns></returns>
        public async static Task<List<Customer>> GetCustomerListAsync_Dummy()
        {
            await _customerDataSource.PrepareDummyData();
            return _customerDataSource.AllCustomers;
        }

        /// <summary>
        /// Gets the customer details.
        /// </summary>
        /// <param name="customerId">The customer id.</param>
        /// <returns></returns>
        public async static Task<Customer> GetCustomerDetailsAsync_Dummy(string customerId)
        {
            await _customerDataSource.PrepareDummyData();
            var matches = _customerDataSource.AllCustomers.Where((item) => item.CustomerId.Equals(customerId));
            if (matches != null && matches.Count() == 1) return matches.First();
            return null;
        }


        /// <summary>
        /// Gets recent customer list.
        /// </summary>
        /// <returns></returns>
        public async static Task<List<Customer>> GetRecentCustomerListAsync_Dummy(int count)
        {
            await _customerDataSource.PrepareDummyData();
            return _customerDataSource.AllCustomers.OrderByDescending(x => x.ClientSince).Take(count).ToList();
        }
    }
}
