using Models;

namespace Components
{
    public class DefenseComponent
    {
        public float Defense { get; set; }
        
        public void InitializeComponent(UnitType unitType)
        {
            //Defense = unitDto.Defense;
        }
    }
}