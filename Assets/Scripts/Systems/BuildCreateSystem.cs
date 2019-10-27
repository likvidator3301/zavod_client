using Leopotam.Ecs;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Components;


namespace Systems
{
    public class BuildCreateSystem : IEcsRunSystem, IEcsInitSystem
    {
        EcsWorld world = null;
        EcsFilter<BuildCreateEvent> buildEvents = null;
        EcsFilter<ClickEvent> clickEvents = null;
        GameObject[] builds = null;
        Camera camera;
        //GameObject ground = null;
        private RaycastHit hitInfo;
        private Ray ray;
        private GameObject current_build;

        public void Init()
        {
            var ce = new BuildCreateEvent();
            world.NewEntityWith(out ce);
            ce.Type = "barracs";
        }

        public void Run()
        {
            if (camera == null)
            {
                camera = Camera.current;
                return;
            }

            if (!buildEvents.IsEmpty())
            {
                foreach (var build in builds)
                {
                    if (build.tag.Equals(buildEvents.Get1[0].Type))
                    {
                        current_build = Object.Instantiate(build);
                        buildEvents.Entities[0].Destroy();
                    }
                }
            }

            if(current_build != null)
            {
                ray = camera.ScreenPointToRay(Input.mousePosition);
                
                Physics.Raycast(ray, out hitInfo, 400, 1);
                current_build.transform.position = hitInfo.point;

                if (!clickEvents.IsEmpty())
                {
                    for(var i = 0; i < clickEvents.GetEntitiesCount(); i++)
                    {
                        if (clickEvents.Get1[i].ButtonNumber == 0)
                        {
                            current_build = null;
                            clickEvents.Entities[i].Destroy();
                        }
                    }
                }
            }
        }
    }
}