using System.Collections.Generic;
using Entities;
using UnityEngine;

namespace Systems
{
    public class UnitSystems : MonoBehaviour
    {
        public readonly AttackSystem AttackSystem;
        public readonly CreatingSystem CreatingSystem;
        public readonly DestroyingSystem DestroyingSystem;
        public readonly MovementSystem MovementSystem;

        public UnitSystems(List<IUnitEntity> units)
        {
            DestroyingSystem = new DestroyingSystem();
            AttackSystem = new AttackSystem();
            MovementSystem = new MovementSystem(units);
            CreatingSystem = new CreatingSystem();
        }
    }
}