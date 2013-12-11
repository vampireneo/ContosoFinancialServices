using System.Collections.Generic;

namespace ContosoFinancialServices.DataModel
{
    public partial class Product
    {
        /// <summary>
        /// Gets or sets the product id.
        /// </summary>
        public string ProductId { get; set; }

        /// <summary>
        /// Gets or sets the name of the product.
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// Gets or sets the product category.
        /// </summary>
        public string ProductCategory { get; set; }

        /// <summary>
        /// Gets or sets the product image.
        /// </summary>
       public string ProductImage { get; set; }

        /// <summary>
        /// Gets or sets the larger product image.
        /// </summary>
       public string ProductImageLarge { get; set; }

        /// <summary>
        /// Gets or sets the product description.
        /// </summary>
        public string ProductDescription { get; set; }

        /// <summary>
        /// Gets or sets the product characteristics.
        /// </summary>
        public List<ProductCharacteristicItem> ProductCharacteristics { get; set; }

        /// <summary>
        /// Gets or sets the product statistics.
        /// </summary>
        public List<ProductStatisticsItem> ProductStatistics { get; set; }

        /// <summary>
        /// Gets or sets the customization parameters.
        /// </summary>
        public List<CustomizationParameter> CustomizationParameters { get; set; }

        /// <summary>
        /// Gets or sets the premium per month.
        /// </summary>
        public string PremiumPerMonth { get; set; }

        /// <summary>
        /// Is this product featured?
        /// </summary>
        public bool IsFeatured { get; set; }
    }    
}
