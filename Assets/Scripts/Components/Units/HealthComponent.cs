using Models;

namespace Components
{
    public class HealthComponent
    {
        public float MaxHp { get; set; }
        public float CurrentHp { get; set; }
        
        public void InitializeComponent(ServerUnitDto unitDto)
        {
            MaxHp = unitDto.MaxHp;
            CurrentHp = unitDto.CurrentHp;
        }
    }
}