using Models;

namespace Components
{
    public class HealthComponent
    {
        public float MaxHp { get; set; }
        public float CurrentHp { get; set; }
        
        public void InitializeComponent(UnitType unitType)
        {
            //CurrentHp = unitDto.Health;
        }
    }
}