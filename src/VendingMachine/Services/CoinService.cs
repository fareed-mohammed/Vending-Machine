using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VendingMachine.Interfaces;

namespace VendingMachine
{
    public class CoinService : ICoinService
    {
        private static readonly IEnumerable<ValidCoin> AcceptedCoins = new List<ValidCoin>() {
                new ValidCoin() {Diameter = 18.0m, Thickness = 1.7m, Type = CoinType.FivePence, Weight = 3.25m, Value = 0.05m},
                new ValidCoin() {Diameter = 24.5m, Thickness = 1.85m, Type = CoinType.TenPence, Weight = 6.5m, Value = 0.10m},
                new ValidCoin() {Diameter = 21.4m, Thickness = 1.7m, Type = CoinType.TwentyPence, Weight = 5.0m, Value = 0.20m},
                new ValidCoin() {Diameter = 27.3m, Thickness = 1.78m, Type = CoinType.FiftyPence, Weight = 8.0m, Value = 0.50m},
                new ValidCoin() {Diameter = 22.5m, Thickness = 3.15m, Type = CoinType.OnePound, Weight = 9.5m, Value = 1.00m},
                new ValidCoin() {Diameter = 28.4m, Thickness = 2.5m, Type = CoinType.TwoPound, Weight = 12.0m, Value = 2.00m}};

        public ValidCoin GetCoin(decimal weight, decimal diameter, decimal thickness)
        {
            return AcceptedCoins.FirstOrDefault(x => x.Weight == weight && x.Thickness == thickness && x.Diameter == diameter);
        }
    }
}
