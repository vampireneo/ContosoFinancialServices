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
        /// Populates the customers from store file.
        /// </summary>
        private async Task ReadXmlDataFromLocalStorageAsync()
        {
            // Return if data is already loaded 
            if (_customerDataSource.AllCustomers != null)
                return;

            try
            {
                var dataFolder = await Package.Current.InstalledLocation.GetFolderAsync(Constants.DataFilesFolder);
                StorageFile sessionFile = await dataFolder.GetFileAsync(Constants.CustomerFile);

                using (
                    IRandomAccessStreamWithContentType sessionInputStream =
                        await sessionFile.OpenReadAsync())
                {

                    var sessionSerializer = new DataContractSerializer(typeof(List<Customer>));
                    var restoredData = sessionSerializer.ReadObject(sessionInputStream.AsStreamForRead());
                    _allCustomers = (List<Customer>)restoredData;
                }

            }
            catch (Exception ex)
            {
                _allCustomers = null;
            }
        }

        /// <summary>
        /// Gets the customer list.
        /// </summary>
        /// <returns></returns>
        public async static Task<List<Customer>> GetCustomerListAsync()
        {
            await _customerDataSource.ReadXmlDataFromLocalStorageAsync();
            return _customerDataSource.AllCustomers;
        }

        /// <summary>
        /// Gets the customer details.
        /// </summary>
        /// <param name="customerId">The customer id.</param>
        /// <returns></returns>
public async static Task<Customer> GetCustomerDetailsAsync(string customerId)
{
    await _customerDataSource.ReadXmlDataFromLocalStorageAsync();
    var matches = _customerDataSource.AllCustomers.Where((item) => item.CustomerId.Equals(customerId));
    if (matches != null && matches.Count() == 1) return matches.First();
    return null;
}


        /// <summary>
        /// Gets recent customer list.
        /// </summary>
        /// <returns></returns>
        public async static Task<List<Customer>> GetRecentCustomerListAsync(int count)
        {
            await _customerDataSource.ReadXmlDataFromLocalStorageAsync();
            return _customerDataSource.AllCustomers.OrderByDescending(x => x.ClientSince).Take(count).ToList();
        }

        // <summary>
        /// Gets total customer count
        /// </summary>
        /// <returns></returns>
        public async static Task<int> GetCustomersCountAsync()
        {
            await _customerDataSource.ReadXmlDataFromLocalStorageAsync();
            return _customerDataSource.AllCustomers.Count();
        }
    }
}
