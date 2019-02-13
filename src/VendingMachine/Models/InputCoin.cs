using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VendingMachine.Interfaces;

namespace VendingMachine
{
    public class InputCoin : ICoinSpecification
    {
        public decimal Diameter { get; set; }
        public decimal Weight { get; set; }
        public decimal Thickness { get; set; }        
    }
}
