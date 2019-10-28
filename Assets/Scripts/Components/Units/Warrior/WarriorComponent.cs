namespace Components
{
    public class WarriorComponent : IUnitInfo
    {
        public float AttackDamage => 10;
        public float AttackDelay => 1;
        public float AttackRange => 10;
        public float Defense => 10;
        public float MoveSpeed => 25;
        public float MaxHp => 50;
        public float CurrentHp { get; set; } = 50;
    }
}