using Leopotam.Ecs;
using Components;
using System.Linq;
using TMPro;
using UnityEngine;
using System;

namespace Systems
{
    public class ResoursesDisplaySystem : IEcsRunSystem
    {
        private readonly EcsFilter<PlayerResourcesComponent> resourses = null;
        private static readonly string cashText = "Нал: ";
        private static readonly string beerText = "Семки: ";
        private static readonly string cash = "Cash";
        private static readonly string seeds = "Seeds";
        
        public void Run()
        {
            var existResourses = resourses.Entities.Where(x => x.IsNotNullAndAlive());

            foreach(var res in existResourses)
            {
                var resComponent = res.Get<PlayerResourcesComponent>();
                var formattedCash = cashText + String.Format("{0:F1}", resComponent.Cash);
                var formattedBeer = beerText + resComponent.Beer;
                
                foreach (var child in resComponent.ResoursesUiDisplay.GetComponentsInChildren<TextMeshProUGUI>())
                {
                    UpdateUnityText(child, formattedCash, cash);
                    UpdateUnityText(child, formattedBeer, seeds);
                }
            }
        }

        private void UpdateUnityText(TextMeshProUGUI textComponent, string formattedNewText, string nameComponentForUpdating)
        {
            if (textComponent.name.Equals(nameComponentForUpdating) && !textComponent.text.Equals(formattedNewText))
            {
                textComponent.text = formattedNewText;
            }
        }
    }
}
