using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContosoFinancialServices.DataModel
{
    class CustomerGroup
    {
        public string Title { get; set; }

        public List<Customer> Items { get; set; }
    }
}
