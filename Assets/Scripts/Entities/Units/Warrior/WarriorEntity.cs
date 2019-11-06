using Component;
using Components;
using UnityEngine;
using UnityEngine.AI;

namespace Entities
{
    public class WarriorEntity : IUnitEntity
    {
        public GameObject Object { get; }
        public NavMeshAgent Agent { get; }
        public IConditionComponent ConditionComponent { get; }
        public IStatsComponent StatsComponent { get; }
        public IMovementComponent MovementComponent { get; }
        public UnitTags Tag { get; }

        public WarriorEntity(GameObject obj, UnitTags tag)
        {
            Agent = obj.GetComponent<NavMeshAgent>();
            Object = obj;
            ConditionComponent = new WarriorConditionComponent();
            StatsComponent = new WarriorStatsComponent();
            MovementComponent = new WarriorMovementComponent();
            this.Tag = tag;
        }
    }
}