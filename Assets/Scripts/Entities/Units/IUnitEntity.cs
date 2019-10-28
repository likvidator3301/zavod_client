using Components;
using UnityEngine;
using UnityEngine.AI;

namespace Entities
{
    public interface IUnitEntity
    {
        IUnitInfo Info { get; }
        GameObject Prefabs { get; }
        GameObject Object { get; set; }
        NavMeshAgent Agent { get; }
    }
}