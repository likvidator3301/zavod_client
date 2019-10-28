namespace Components
{
    public interface IUnitInfo
    {
        float AttackDamage { get; }
        float AttackSpeed { get; }
        float AttackRange { get; }
        float Defense { get; }
        float MoveSpeed { get; }
        float MaxHp { get; }
        float CurrentHp { get; set; }
    }
}