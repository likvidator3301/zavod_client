using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Components
{
    internal class KioskComponent
    {
        public DateTime LastBeerGeneration;
        public TimeSpan BeerGeneratingTiming = TimeSpan.FromSeconds(3);
        public int SeedsPerTiming = 10;
    }
}
