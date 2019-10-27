using System;

namespace Systems
{
    public class UnitSystems
    {
        public readonly CreatingSystem CreatingSystem;
        public readonly AttackSystem AttackSystem;
        public readonly MovementSystem MovementSystem;
        public readonly DestroyingSystem DestroyingSystem;

        public UnitSystems()
        {
            DestroyingSystem = new DestroyingSystem();
            AttackSystem = new AttackSystem();
            MovementSystem = new MovementSystem();
            CreatingSystem = new CreatingSystem();
        }
    }
}
