using UnityEngine;

namespace Components
{
    public class DefenseComponent : MonoBehaviour
    {
        public float Defense { get; set; }

        public void InitializeComponent(DefenseComponent defenseComponent)
        {
            Defense = defenseComponent.Defense;
        }
    }
}