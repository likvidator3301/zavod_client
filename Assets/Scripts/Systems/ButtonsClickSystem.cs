using Leopotam.Ecs;
using System;
using Components;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Systems
{
    class ButtonsClickSystem : IEcsRunSystem
    {
        private readonly EcsFilter<ButtonClickEvent> clicks = null;
        private readonly EcsWorld world = null;
        private readonly GameDefinitions gameDefinitions = null;
        private readonly EcsFilter<BuildingComponent> builds = null;
        private readonly EcsFilter<PlayerResourcesComponent> resources = null;

        public void Run()
        {
            var clickedButtonsEvents = clicks.Entities.Where(x => x.IsNotNullAndAlive());

            foreach (var btn in clickedButtonsEvents)
            {
                if (btn.Get<ButtonClickEvent>() == null)
                    continue;

                btn.Unset<ButtonClickEvent>();
                HandleButtonClick(btn.Get<ButtonComponent>());
            }
        }

        private void HandleButtonClick(ButtonComponent btn)
        {
            switch (btn.buttonName)
            {
                case "CreateBarraks":
                    OnGarageCreateClick();
                    break;
                case "Warrior":
                    CreateWarrior(btn);
                    break;
                case "CreateKiosk":
                    CreateKiosk();
                    break;
                case "Exit":
                    Application.Quit();
                    break;
                default:
                    break;
            }
        }

        private void CreateKiosk()
        {
            world.NewEntityWith(out BuildingCreateComponent buildEvent);
            buildEvent.Type = Components.Tags.Buildings.BuildingTag.Kiosk;
        }

        private void OnGarageCreateClick()
        {
            world.NewEntityWith(out BuildingCreateComponent buildEvent);
            buildEvent.Type = Components.Tags.Buildings.BuildingTag.Garage;
        }

        private void CreateWarrior(ButtonComponent button)
        {
            if (resources.Get1[0].Beer < 10)
                return;

            resources.Get1[0].Beer -= 10;
                
            var GameObjectOfButton = GetParentGameObjectFromButton(button.button);
            world.NewEntityWith(out UnitCreateEvent unitCreate);
            unitCreate.UnitTag = UnitTag.Warrior;

            unitCreate.Position = GameObjectOfButton == null
                ? throw new NullReferenceException()
                : GameObjectOfButton.transform.position;
        }

        private GameObject GetParentGameObjectFromButton(Button button)
        {
            for (var i = 0; i < builds.GetEntitiesCount(); i++)
            {
                foreach (var inBuildButton in builds.Entities[i].Get<BuildingComponent>().AllButtons)
                {
                    if (inBuildButton.GetInstanceID() == button.GetInstanceID())
                        return builds.Get1[i].obj;
                }
            }

            return null;
        }
    }
}
