using System.Linq;
using Leopotam.Ecs;
using Components;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Systems
{
    public class CheckClickOnBuildsSystem : IEcsRunSystem
    {
        private readonly EcsFilter<ClickEvent> clickEvents = null;
        private readonly EcsFilter<BuildingComponent> buildings = null;
        private readonly EcsFilter<ButtonComponent> buttons = null;
        private readonly EcsFilter<ButtonClickEvent> btnClicks = null;

        private Camera camera;
        private RaycastHit hitInfo;
        private Ray ray;

        public void Run()
        {
            if (camera is null)
            {
                camera = Camera.current;
                return;
            }

            ray = camera.ScreenPointToRay(Input.mousePosition);

            if (MouseClickHelper.IsPressClick(clickEvents, 0))
            {
                for (var i = 0; i < buttons.GetEntitiesCount(); i++)
                {
                    if (btnClicks.GetEntitiesCount() > 0 || EventSystem.current.IsPointerOverGameObject())
                        return;
                }

                for (var i = 0; i < buildings.GetEntitiesCount(); i++)
                {
                    if (buildings.Entities[i].Get<BuildingComponent>().obj.GetComponent<Collider>().Raycast(ray, out hitInfo, 400))
                    {
                        buildings.Entities[i].Get<BuildingComponent>().InBuildCanvas.enabled = true;
                    }
                    else 
                    {
                        buildings.Entities[i].Get<BuildingComponent>().InBuildCanvas.enabled = false;
                    }
                }
            }
        }
    }
}
