using VendingMachine.Interfaces;

namespace VendingMachine
{
    public class ItemChange
    {
        public CoinType Type { get; set; }
        public int Number { get; set; }

        /*public int NoOfFivePences;
        public int NoOfTenPences;
        public int NoOfTwentyPences;
        public int NoOfFiftyPences;
        public int NoOfOnePounds;
        public int NoOfTwoPounds;*/
    }
}