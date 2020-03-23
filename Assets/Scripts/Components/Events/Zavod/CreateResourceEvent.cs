using UnityEngine;

namespace Components.Zavod
{
    public class CreateResourceEvent
    {
        public Vector3 Position { get; set; }
        public int Count { get; set; }
        public ResourceTag Tag { get; set; }
    }
}