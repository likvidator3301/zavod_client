using Components;
using UnityEngine;

namespace Systems
{
    public static class UnitComponentExtensions
    {
        private const string pathToInfo = "./Assets/Scripts/Components/Units/Info";
        private const string fileType = "json";
        
        public static void SetFields(this UnitComponent unitComponent, GameObject obj, UnitTag tag)
        {
            unitComponent.Object = obj;
            unitComponent.Tag = tag;
        }
        
        public static void AddWarriorComponents(this UnitComponent unitComponent)
        {
            var unitObject = unitComponent.Object;
            unitObject.AddComponent<AttackComponent>().InitializeComponent(
                GetComponentFor<AttackComponent>(UnitTag.Warrior, UnitComponentTag.AttackComponent));
            unitObject.AddComponent<DefenseComponent>().InitializeComponent(
                GetComponentFor<DefenseComponent>(UnitTag.Warrior, UnitComponentTag.DefenseComponent));
            unitObject.AddComponent<HealthComponent>().InitializeComponent(
                GetComponentFor<HealthComponent>(UnitTag.Warrior, UnitComponentTag.HealthComponent));
            unitObject.AddComponent<MovementComponent>().InitializeComponent(
                GetComponentFor<MovementComponent>(UnitTag.Warrior, UnitComponentTag.MovementComponent));
        }
        
        private static T GetComponentFor<T>(UnitTag unit, UnitComponentTag unitComponent)
        {
            var pathToComponent = $@"/{unit}/{unitComponent}.{fileType}";
            return Deserializer.GetComponent<T>($"{pathToInfo}{pathToComponent}");
        }
    }
}