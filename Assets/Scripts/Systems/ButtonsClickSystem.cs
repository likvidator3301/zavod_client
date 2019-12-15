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
                    OnBarrakCreateClick();
                    break;
                case "Warrior":
                    CreateWarrior(btn);
                    break;
                default:
                    break;
            }
        }

        private void OnBarrakCreateClick()
        {
            world.NewEntityWith(out BuildCreateEvent buildEvent);
            buildEvent.Type = "barraks";
            buildEvent.buildingCanvas = GuiHelper.InstantiateAllButtons(gameDefinitions.GuiDefinitions.inBuildingMenu, world);
            buildEvent.buildingCanvas.enabled = false;
        }

        private void CreateWarrior(ButtonComponent button)
        {
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
                foreach (var inBuildButton in builds.Get1[i].AllButtons)
                {
                    if (inBuildButton.GetInstanceID() == button.GetInstanceID())
                        return builds.Get1[i].obj;
                }
            }

            return null;
        }
    }
}
