using System.Collections.Generic;
using Components;
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
        private readonly UserInputEvent inputEvent;

        public UnitSystems(PlayerComponent player, Dictionary<GameObject, IUnitEntity> allUnits)
        {
            DestroyingSystem = new DestroyingSystem(allUnits);
            AttackSystem = new AttackSystem();
            MovementSystem = new MovementSystem(player.Units);
            CreatingSystem = new CreatingSystem();
        }
    }
}