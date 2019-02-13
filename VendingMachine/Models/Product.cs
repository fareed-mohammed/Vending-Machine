using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VendingMachine
{
    public class Product
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public decimal Price { get; set; }
        public ProductItemType Type { get; set; }
    }
}
