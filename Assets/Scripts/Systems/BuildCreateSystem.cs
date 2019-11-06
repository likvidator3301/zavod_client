using Leopotam.Ecs;
using UnityEngine;
using Components;


namespace Systems
{
    public class BuildCreateSystem : IEcsRunSystem
    {
        EcsWorld world = null;
        EcsFilter<BuildCreateEvent> buildEvents = null;
        EcsFilter<ClickEvent> clickEvents = null;
        GameObject[] builds = null;

        private Camera camera;
        private RaycastHit hitInfo;
        private Ray ray;
        private GameObject currentBuild;

        public void Run()
        {
            if (Camera.current == null)
                return;

            camera = Camera.current;

            if (!buildEvents.IsEmpty())
            {
                foreach (var build in builds)
                {
                    if (build.tag.Equals(buildEvents.Get1[0].Type))
                        CreateOrSwitchBuild(build);
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
                currentBuild = Object.Instantiate(build);
            else if (!currentBuild.tag.Equals(build.tag))
            {
                Object.Destroy(currentBuild);
                currentBuild = Object.Instantiate(build);
            }

            buildEvents.Entities[0].Destroy();
        }

        private void BuildSet(GameObject build)
        {
            world.NewEntityWith(out Build newBuild);
            newBuild.obj = build;
            newBuild.Type = build.tag;
        }
    }
}