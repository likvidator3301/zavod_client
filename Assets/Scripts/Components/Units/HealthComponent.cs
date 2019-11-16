using UnityEngine;

namespace Components
{
    public class HealthComponent : MonoBehaviour
    {
        public float MaxHp { get; set; }
        public float CurrentHp { get; set; }

        public void InitializeComponent(HealthComponent healthComponent)
        {
            MaxHp = healthComponent.MaxHp;
            CurrentHp = healthComponent.CurrentHp;
        }
    }
}