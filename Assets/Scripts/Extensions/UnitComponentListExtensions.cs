using System.Collections.Generic;
using System.Linq;
using Components;
using UnityEngine;

namespace Systems
{
    public static class UnitComponentListExtensions
    {
        public static void HighlightObjects(this IEnumerable<UnitComponent> units) =>
            units.Select(u => u.Object).HighlightObjects();
        
        public static void DehighlightObjects(this IEnumerable<UnitComponent> units) => 
            units.Select(u => u.Object).DehighlightObjects();
    }
}