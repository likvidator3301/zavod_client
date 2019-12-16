using System.Collections.Generic;

namespace Systems
{
    internal static class PricesKeeper
    {
        public static Dictionary<string, int> PricesFromTag = new Dictionary<string, int>
        {
            { "barraks", 300 },
            { "Kiosk", 250 }
        };
    }
}
