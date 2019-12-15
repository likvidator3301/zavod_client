using System;
using Components;
using Leopotam.Ecs;
using UnityEngine;
using UnityEngine.UI;

namespace Systems
{
    public static class GuiHelper
    {
        public static Canvas InstantiateAllButtons(Canvas canvasAsset, EcsWorld world)
        {
            var newCanvas = UnityEngine.Object.Instantiate(canvasAsset, Vector3.zero, Quaternion.AngleAxis(0, Vector3.zero));

            foreach (var button in newCanvas.GetComponentsInChildren<Button>())
            {
                var buttonEntity = world.NewEntityWith(out ButtonComponent buttonComponent);
                buttonComponent.buttonName = button.name;
                buttonComponent.bounds = button.GetButtonBounds();
                buttonComponent.button = button;
                buttonComponent.button.onClick.AddListener(() => OnClick(buttonEntity));
            }

            return newCanvas;
        }

        private static void OnClick(EcsEntity btn)
        {
            btn.Set<ButtonClickEvent>();
        }
    }
}
