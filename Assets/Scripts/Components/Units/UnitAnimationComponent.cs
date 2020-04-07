using UnityEngine;

namespace Components
{
    public class UnitAnimationComponent
    {
        public Animator Animator { get; set; }
        public bool IsMoving { get; set; }
        public bool IsAttacking { get; set; }
        public float CurrentHp { get; set; }
    }
}