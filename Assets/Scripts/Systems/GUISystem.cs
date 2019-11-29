using UnityEngine;
using Leopotam.Ecs;
using UnityEngine.UI;
using Components;
using System.Linq;
using System.Collections.Generic;
using System;

namespace Systems
{
    class GUISystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly GameDefinitions gameDefinitions = null;
        private readonly EcsWorld world = null;
        private readonly EcsFilter<ButtonComponent> buttons = null;
        private readonly EcsFilter<ClickEvent> clicks = null;
        private readonly EcsFilter<BuildingComponent> builds = null;

        private Canvas buildMenu;
        private Canvas inBuildingMenu;

        public void Init()
        {
            buildMenu = gameDefinitions.GuiDefinitions.buildMenu;
            inBuildingMenu = gameDefinitions.GuiDefinitions.inBuildingMenu;

            InstantiateAllButtons(buildMenu);
        }

        public void Run()
        {
            if (!MouseClickHelper.IsPressClick(clicks, 0) 
                || MouseClickHelper.IsBlockedClick(clicks, 0))
                return;

            for (var i = 0; i < buttons.GetEntitiesCount(); i++)
            {
                if (buttons.Get1[i].buttonName.Equals("CreateBarraks")
                    && buttons.Get1[i].bounds.Contains(Input.mousePosition))
                {
                    MouseClickHelper.BlockAllClick(clicks, 0);
                    OnBarrakCreateClick();
                }

                CreateWarrior(buttons.Get1[i]);
            }
        }

        private void CreateWarrior(ButtonComponent button)
        {
            if (button.buttonName.Equals("Warrior")
                && button.bounds.Contains(Input.mousePosition)
                && button.button.GetComponentsInParent<Canvas>().First().enabled)
            {
                var GameObjectOfButton = GetParentGameObjectFromButton(button.button);
                world.NewEntityWith(out UnitCreateEvent unitCreate);
                unitCreate.UnitTag = UnitTag.Warrior;

                unitCreate.Position = GameObjectOfButton == null
                    ? throw new NullReferenceException()
                    : GameObjectOfButton.transform.position;

                MouseClickHelper.BlockAllClick(clicks, 0);
            }
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

        private Canvas InstantiateAllButtons(Canvas canvasAsset)
        {
            var newCanvas = UnityEngine.Object.Instantiate(canvasAsset, Vector3.zero, Quaternion.AngleAxis(0, Vector3.zero));

            foreach (var button in newCanvas.GetComponentsInChildren<Button>())
            {
                world.NewEntityWith(out ButtonComponent buttonComponent);
                buttonComponent.buttonName = button.name;
                buttonComponent.bounds = button.GetButtonBounds();
                buttonComponent.button = button;
            }

            return newCanvas;
        }

        private void OnBarrakCreateClick()
        {
            var buildingCanvas = InstantiateAllButtons(inBuildingMenu);
            world.NewEntityWith(out BuildCreateEvent buildEvent);
            buildEvent.Type = "barraks";
            buildEvent.buildingCanvas = buildingCanvas;
            buildEvent.buildingCanvas.enabled = false;
        }
    }
}
