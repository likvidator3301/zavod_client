using System.Collections.Generic;

namespace Systems
{
    internal static class PricesKeeper
    {
        public static Dictionary<string, int> PricesFromTag = new Dictionary<string, int>
        {
            { "Garage", 300 },
            { "Stall", 250 }
        };
    }
}
