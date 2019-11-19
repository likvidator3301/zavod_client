using Components;
using UnityEngine;

namespace Systems
{
    public static class UnitComponentExtensions
    {
        public static void SetFields(this UnitComponent unitComponent, GameObject obj, UnitTag tag)
        {
            unitComponent.Object = obj;
            unitComponent.Tag = tag;
        }
    }
}