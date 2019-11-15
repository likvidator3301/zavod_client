using Leopotam.Ecs;
using UnityEngine;
using Components;


namespace Systems
{
    public class BuildCreateSystem : IEcsRunSystem
    {
        private readonly EcsWorld world = null;
        private readonly EcsFilter<BuildCreateEvent> buildEvents = null;
        private readonly EcsFilter<ClickEvent> clickEvents = null;
        private readonly GameObject[] builds = null;

        private Camera camera;
        private RaycastHit hitInfo;
        private Ray ray;
        private GameObject currentBuild;

        public void Run()
        {
            if (camera is null)
            {
                camera = Camera.current;
                return;
            }

            if (!buildEvents.IsEmpty())
            {
                foreach (var build in builds)
                {
                    if (build.tag.Equals(buildEvents.Get1[0].Type))
                        CreateOrSwitchBuild(build);
                }

                foreach (var buildEvent in buildEvents.Entities)
                {
                    if (!buildEvent.IsNull() && buildEvent.IsAlive())
                        buildEvent.Destroy();
                }
            }

            if (currentBuild == null)
                return;

            ray = camera.ScreenPointToRay(Input.mousePosition);
            Physics.Raycast(ray, out hitInfo, 400, 1);
            currentBuild.transform.position = hitInfo.point;

            if (!clickEvents.IsEmpty())
            {
                for (var i = 0; i < clickEvents.GetEntitiesCount(); i++)
                {
                    if (clickEvents.Get1[i].ButtonNumber == 0)
                    {
                        BuildSet(currentBuild);
                        currentBuild = null;
                    }
                }
            }
        }

        private void CreateOrSwitchBuild(GameObject build)
        {
            if (currentBuild == null)
            {
                currentBuild = Object.Instantiate(build);
            }
            else if (!currentBuild.tag.Equals(build.tag))
            {
                Object.Destroy(currentBuild);
                currentBuild = Object.Instantiate(build);
            }
        }

        private void BuildSet(GameObject build)
        {
            world.NewEntityWith(out Build newBuild);
            newBuild.obj = build;
            newBuild.Type = build.tag;
        }
    }
}