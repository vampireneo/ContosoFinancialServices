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
    class ProductDataSource
    {
        private static ProductDataSource _productDataSource = new ProductDataSource();

        private List<Product> _allProducts = null;

        public List<Product> AllProducts
        {
            get { return this._allProducts; }

        }

        /// <summary>
        /// Populates the products from store file.
        /// </summary>
        private async Task ReadXmlDataFromLocalStorageAsync()
        {
            try
            {
                //If product list is already loaded from XML file, skip reloading it again. 
                if (_productDataSource.AllProducts != null)
                    return;

                var dataFolder = await Package.Current.InstalledLocation.GetFolderAsync(Constants.DataFilesFolder);
                StorageFile sessionFile = await dataFolder.GetFileAsync(Constants.ProductFile);

                using (
                    IRandomAccessStreamWithContentType sessionInputStream =
                        await sessionFile.OpenReadAsync())
                {

                    var sessionSerializer = new DataContractSerializer(typeof(List<Product>));
                    var restoredData = sessionSerializer.ReadObject(sessionInputStream.AsStreamForRead());
                    _allProducts = (List<Product>)restoredData;
                }

            }
            catch (Exception ex)
            {
                _allProducts = null;
            }
        }

        /// <summary>
        /// Ges the product list.
        /// </summary>
        /// <returns></returns>
        public async static Task<List<Product>> GeProductListAsync()
        {
            await _productDataSource.ReadXmlDataFromLocalStorageAsync();
            return _productDataSource.AllProducts;
        }

        /// <summary>
        /// Ges the product list.
        /// </summary>
        /// <param name="productIds">The product ids.</param>
        /// <returns></returns>
        public async static Task<List<Product>> GetProductListByIDAsync(List<string> productIds)
        {
            await _productDataSource.ReadXmlDataFromLocalStorageAsync();
            return _productDataSource.AllProducts.Where(x=> productIds.Contains(x.ProductId)).ToList();
        }

        /// <summary>
        /// Ges the product list.
        /// </summary>
        /// <param name="productIds">The product ids.</param>
        /// <returns></returns>
        public async static Task<List<Product>> GetFeaturedProductListAsync(int count)
        {
            await _productDataSource.ReadXmlDataFromLocalStorageAsync();
            return _productDataSource.AllProducts.Where(x => x.IsFeatured == true).Take(count).ToList();
        }

        /// <summary>
        /// Gets the product details.
        /// </summary>
        /// <param name="productId">The product id.</param>
        /// <returns></returns>
        public async static Task<Product> GetProductDetailsAsync(string productId)
        {
            await _productDataSource.ReadXmlDataFromLocalStorageAsync();
            var matches = _productDataSource.AllProducts.Where((item) => item.ProductId.Equals(productId));
            if (matches != null && matches.Count() == 1) return matches.First();
            return null;
        }

        /// <summary>
        /// Gets some random products for live tile to flip through.
        /// </summary>
        /// <param name="count">Number of random products</param>
        /// <returns>List of products</returns>
        public static async Task<List<Product>> GetRandomProductsAsync(int count)
        {
            await _productDataSource.ReadXmlDataFromLocalStorageAsync();
            var matches = _productDataSource.AllProducts.OrderBy(item => Guid.NewGuid()).Take(count).ToList();
            return matches;
        }

        /// <summary>
        /// This method searches the products by search text. The search text can be a part of the 
        /// Product name or Product description or Product Category
        /// </summary>
        /// <param name="searchText">The search text.</param>
        /// <returns>Search results</returns>
        public static async Task<List<Product>> SearchProductsAsync(string searchText)
        {
            await _productDataSource.ReadXmlDataFromLocalStorageAsync();
            return _productDataSource.AllProducts.Where(
                    item =>
                    item.ProductName.ToUpper().Contains(searchText.ToUpper()) ||
                    item.ProductDescription.ToUpper().Contains(searchText.ToUpper())).ToList();
        }

    }
}
