using Components;

namespace Systems
{
    public static class AnimationComponentExtensions
    {
        public static void SetFields(
            this UnitAnimationComponent animationComponent,
            bool isAttacking,
            bool isMoving,
            float currentHp)
        {
            animationComponent.CurrentHp = currentHp;
            animationComponent.IsAttacking = isAttacking;
            animationComponent.IsMoving = isMoving;
        }
    }
}