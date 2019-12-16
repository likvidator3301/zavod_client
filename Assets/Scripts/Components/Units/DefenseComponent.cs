using UnityEngine;

namespace Components
{
    public class DefenseComponent
    {
        public float Defense { get; set; }

        public void InitializeComponent(DefenseComponent defenseComponent)
        {
            Defense = defenseComponent.Defense;
        }
    }
}