using System;
using System.Collections.Generic;
using Components;
using Leopotam.Ecs;
using UnityEngine;

namespace Systems
{
    class LoadSystem : IEcsInitSystem
    {
        private readonly EcsWorld world = null;
        private readonly GameDefinitions definitions = null;

        private EcsEntity resoursesEntity;

        public void Init()
        {
            LoadMainCamera();
            LoadUiAssets();
            LoadUi();
            LoadBuildingsSwitches();
            InitConsole();
        }

        private void InitConsole()
        {
            world.NewEntityWith(out ConsoleMessagesComponent mes);
            mes.Messages = new List<Message>();
        }

        private void LoadBuildingsSwitches()
        {
            var switchesComponent = resoursesEntity.Set<BuildingSwitchesComponent>();

            foreach (BuildingTag tag in Enum.GetValues(typeof(BuildingTag)))
            {
                var type = tag.ToString();
                var currentSwitch = new BuildingSwitch();
                currentSwitch.instancedRedBuilding = GameObject.Instantiate(Resources.Load<GameObject>(@"Prefabs/Buildings/" + type + "Red"), new Vector3(-200, 200, -200), Quaternion.Euler(0, 0, 0));
                currentSwitch.instancedGreenBuilding = GameObject.Instantiate(Resources.Load<GameObject>(@"Prefabs/Buildings/" + type + "Green"), new Vector3(-200, 200, -200), Quaternion.Euler(0, 0, 0));
                switchesComponent.buildingsSwitch.Add(type, currentSwitch);
            }
        }

        private void LoadMainCamera()
        {
            world.NewEntityWith(out CameraComponent cameraComponent);
            cameraComponent.maxHeigth = definitions.CameraDefinitions.maxHeigth;
            cameraComponent.minHeigth = definitions.CameraDefinitions.minHeigth;
            cameraComponent.rotateSpeed = definitions.CameraDefinitions.rotateSpeed;
            cameraComponent.speed = definitions.CameraDefinitions.speed;
            cameraComponent.verticalMoveSpeed = definitions.CameraDefinitions.verticalMoveSpeed;
            cameraComponent.verticalMoveStepFactor = definitions.CameraDefinitions.verticalMoveStepFactor;
            cameraComponent.Camera = UnityEngine.Object.Instantiate(definitions.CameraDefinitions.mainCameraPrefab);
        }

        private void LoadUiAssets()
        {
            resoursesEntity = world.NewEntityWith(out PlayerResourcesComponent playerAsset);
            playerAsset.ResoursesUiDisplay = GameObject.Instantiate(definitions.GuiDefinitions.PlayerInfo);

            var assets = resoursesEntity.Set<AssetsComponent>();
            assets.InBuildingCanvasesAssets = GetInBuildingCanvasesAssets();
            assets.BuildingsAssets = GetAllBuildingAssets();
        }

        private Dictionary<string, GameObject> GetAllBuildingAssets()
        {
            var assets = new Dictionary<string, GameObject>();

            foreach (BuildingTag tag in Enum.GetValues(typeof(BuildingTag)))
            {
                var type = tag.ToString();
                assets.Add(type, Resources.Load<GameObject>(@"Prefabs\Buildings\" + type));
            }

            return assets;
        }

        private Dictionary<string, Canvas> GetInBuildingCanvasesAssets()
        {
            var assets = new Dictionary<string, Canvas>();

            foreach (BuildingTag tag in Enum.GetValues(typeof(BuildingTag)))
            {
                var type = tag.ToString();
                assets.Add(type, Resources.Load<Canvas>(@"Prefabs\GUI\BuildingsMenu\" + type + "Menu"));
            }

            return assets;
        }

        private void LoadUi()
        {
            var uiCanvases = resoursesEntity.Set<UiCanvasesComponent>();
            uiCanvases.UserInterface = GuiHelper.InstantiateAllButtons(definitions.GuiDefinitions.BuildMenu, world);
        }
    }
}
