using System;
using System.Linq;
using Components;
using Leopotam.Ecs;
using UnityEngine;
using UnityEngine.UI;

namespace Systems
{
    public static class GuiHelper
    {
        public static void OffAllButtons(Canvas canvas)
        {
            var buttons = canvas.GetComponentsInChildren<Button>();

            foreach (var button in buttons.Where(b => b.name != "Autorization"))
            {
                button.interactable = false;
            }
        }

        public static void OnAllButtons(Canvas canvas)
        {
            var buttons = canvas.GetComponentsInChildren<Button>();

            foreach (var button in buttons.Where(b => b.name != "Autorization"))
            {
                button.interactable = true;
            }
        }


        public static Canvas InstantiateAllButtons(Canvas canvasAsset, EcsWorld world)
        {
            var newCanvas = UnityEngine.Object.Instantiate(canvasAsset);

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
