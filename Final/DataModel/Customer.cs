using System.Collections.Generic;

namespace ContosoFinancialServices.DataModel
{
    public partial class Customer
    {
        /// <summary>
        /// Gets or sets the customer id.
        /// </summary>
        /// <value>
        /// The customer id.
        /// </value>
        public string CustomerId { get; set; }

        /// <summary>
        /// Gets or sets the account number.
        /// </summary>
        /// <value>
        /// The account number.
        /// </value>
        public string AccountNumber { get; set; }

        /// <summary>
        /// Gets or sets the client since.
        /// </summary>
        /// <value>
        /// The client since.
        /// </value>
        public string ClientSince { get; set; }

        /// <summary>
        /// Gets or sets the name of the customer.
        /// </summary>
        /// <value>
        /// The name of the customer.
        /// </value>
        public string CustomerName { get; set; }

        /// <summary>
        /// Gets or sets the customer image.
        /// </summary>
        /// <value>
        /// The customer image.
        /// </value>
        public string CustomerImage { get; set; }

        /// <summary>
        /// Gets or sets the customer address.
        /// </summary>
        /// <value>
        /// The customer address.
        /// </value>
        public string CustomerAddress { get; set; }

        /// <summary>
        /// Gets or sets the customer phone.
        /// </summary>
        /// <value>
        /// The customer phone.
        /// </value>
        public string CustomerPhone { get; set; }

        /// <summary>
        /// Gets or sets the customer email.
        /// </summary>
        /// <value>
        /// The customer email.
        /// </value>
        public string CustomerEmail { get; set; }

        /// <summary>
        /// Gets or sets the marital status.
        /// </summary>
        /// <value>
        /// The marital status.
        /// </value>
        public string MaritalStatus { get; set; }

        /// <summary>
        /// Gets or sets the occupation.
        /// </summary>
        /// <value>
        /// The occupation.
        /// </value>
        public string Occupation { get; set; }

        /// <summary>
        /// Gets or sets the annual income.
        /// </summary>
        /// <value>
        /// The annual income.
        /// </value>
        public string AnnualIncome { get; set; }

        /// <summary>
        /// Gets or sets the product summary.
        /// </summary>
        /// <value>
        /// The product summary.
        /// </value>
        public List<string> ProductSummary { get; set; }

        /// <summary>
        /// Gets or sets the type of the client.
        /// </summary>
        /// <value>
        /// The type of the client.
        /// </value>
        public string ClientType { get; set; }
    }
}
