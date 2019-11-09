using UnityEngine;
using UnityEngine.AI;

namespace Components
{
    public interface IUnitEntity
    {
        GameObject Object { get; }
        NavMeshAgent Agent { get; }
        IConditionComponent ConditionComponent { get; }
        IStatsComponent StatsComponent { get; }
        IMovementComponent MovementComponent { get; }
        UnitTags Tag { get; }
    }
}
