namespace Components
{
    public interface IConditionComponent
    {
        float MaxHp { get; }
        float CurrentHp { get; set; }
        float LastAttackTime { get; set; }
    }
}
