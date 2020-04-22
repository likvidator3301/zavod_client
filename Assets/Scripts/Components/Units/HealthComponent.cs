using Models;

namespace Components
{
    public class HealthComponent
    {
        public float? MaxHp { get; set; }
        public float CurrentHp { get; set; }
        
        public void InitializeComponent(int value)
        {
            CurrentHp = value;
            MaxHp = null;
        }
    }
}