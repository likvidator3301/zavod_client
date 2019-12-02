using System.Linq;
using Leopotam.Ecs;
using Components;
using UnityEngine;

namespace Systems
{
    public class CheckClickOnBuildsSystem : IEcsRunSystem
    {
        private readonly EcsFilter<ClickEvent> clickEvents = null;
        private readonly EcsFilter<BuildingComponent> buildings = null;
        private readonly EcsFilter<ButtonComponent> buttons = null;

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
                    if (buttons.Get1[i].bounds.Contains(Input.mousePosition))
                        return;
                }

                for (var i = 0; i < buildings.GetEntitiesCount(); i++)
                {
                    if (buildings.Get1[i].obj.GetComponent<Collider>().Raycast(ray, out hitInfo, 400))
                    {
                        buildings.Get1[i].InBuildCanvas.enabled = true;
                    }
                    else 
                    {
                        buildings.Get1[i].InBuildCanvas.enabled = false;
                    }
                }
            }
        }
    }
}
