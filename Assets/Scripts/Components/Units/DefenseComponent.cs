using Models;
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
        
        public void InitializeComponent(ServerUnitDto unitDto)
        {
            Defense = unitDto.Defense;
        }
    }
}