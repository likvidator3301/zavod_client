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
        private readonly PlayerComponent player;

        public UnitSystems(
            PlayerComponent player,
            Dictionary<GameObject, IUnitEntity> allUnits,
            UserInputEvent inputEvent)
        {
            this.player = player;
            DestroyingSystem = new DestroyingSystem(allUnits);
            AttackSystem = new AttackSystem();
            MovementSystem = new MovementSystem(player.Units);
            CreatingSystem = new CreatingSystem();
            this.inputEvent = inputEvent;
        }

        public void Handle()
        {
            inputEvent.HandleInput();
            DestroyingSystem.HandleDestroy(player);
        }
    }
}