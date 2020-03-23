using UnityEngine;

namespace Components.Zavod
{
    public class ResourceGeneratorComponent
    {
        public float LastGeneratedMoneyTime = Time.time;
        public int GenerateMoneyDelay = 5;
        public int GenerateMoneyCount = 25;
    }
}