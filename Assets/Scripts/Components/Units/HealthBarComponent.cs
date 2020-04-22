using System;
using System.Linq;
using UnityEngine;

namespace Components
{
    public class HealthBarComponent
    {
        public GameObject HealthBar { get; set; }

        internal void InitializeComponent(GameObject unitObject)
        {
            HealthBar = unitObject.GetComponentsInChildren<Transform>()
                .Where(o => o.name.Equals("Bar"))
                .First().gameObject;
        }
    }
}