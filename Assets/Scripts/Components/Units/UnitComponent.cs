using UnityEditor;
using UnityEngine;

namespace Components
{
    public class UnitComponent
    {
        public GameObject Object { get; set; }
        public UnitTag Tag { get; set; }
        public GUID PlayerGuid { get; set; }
    }
}