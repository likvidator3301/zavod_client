using System;
using System.Collections.Generic;
using Components;
using Leopotam.Ecs;
using ServerCommunication;
using UnityEngine;
using UnityEngine.SceneManagement;

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
            LoadUnitsAssets();
            InitTime();
        }

        private void InitTime()
        {
            world.NewEntityWith(out TimeComponent time);
            time.GameStartTime = DateTime.Now;
        }

        private void InitConsole()
        {
            world.NewEntityWith(out ConsoleMessagesComponent mes);
            mes.Messages = new List<Message>();
        }

        private void LoadBuildingsSwitches()
        {
            var switchesComponent = resoursesEntity.Set<BuildingSwitchesComponent>();
            switchesComponent.buildingsSwitch = new Dictionary<string, BuildingSwitch>();   

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
            cameraComponent.Camera.transform.position = ServerClient.Communication.Sessions.IsCreator
                                                            ? new Vector3(20, cameraComponent.Camera.transform.position.y, 20)
                                                            : new Vector3(180, cameraComponent.Camera.transform.position.y, 180);
        }

        private void LoadUiAssets()
        {
            resoursesEntity = world.NewEntityWith(out PlayerResourcesComponent playerAsset);
            playerAsset.Semki = 50;
            playerAsset.Cash = 1000;
            playerAsset.ResoursesUiDisplay = GameObject.Instantiate(Resources.Load<Canvas>(@"Prefabs/GUI/PlayerInfo"));

            var assets = resoursesEntity.Set<BuildingAssetsComponent>();
            assets.InBuildingCanvasesAssets = GetInBuildingCanvasesAssets();
            assets.BuildingsAssets = GetAllBuildingAssets();
        }

        private Dictionary<string, GameObject> GetAllBuildingAssets()
        {
            var assets = new Dictionary<string, GameObject>();

            foreach (BuildingTag tag in Enum.GetValues(typeof(BuildingTag)))
            {
                var type = tag.ToString();
                var path = @"Prefabs\Buildings\" + type;
                var enemyPath = @"Prefabs\Buildings\Enemy" + type;
                assets.Add(type, Resources.Load<GameObject>(path));
                assets.Add("Enemy" + type, Resources.Load<GameObject>(enemyPath));
            }

            return assets;
        }

        private Dictionary<string, Canvas> GetInBuildingCanvasesAssets()
        {
            var assets = new Dictionary<string, Canvas>();

            assets.Add("None", Resources.Load<Canvas>(@"Prefabs\GUI\BuildingsMenu\None"));
            foreach (BuildingTag tag in Enum.GetValues(typeof(BuildingTag)))
            {
                var type = tag.ToString();
                assets.Add(type, Resources.Load<Canvas>(@"Prefabs\GUI\BuildingsMenu\" + type + "Menu"));
            }

            return assets;
        }

        private void LoadUnitsAssets()
        {
            world.NewEntityWith(out UnitAssetsComponent unitAssets);
            unitAssets.assetsByName = new Dictionary<string, GameObject>();

            foreach (UnitTag tag in Enum.GetValues(typeof(UnitTag)))
            {
                var type = tag.ToString();
                var path = @"Prefabs/Units/" + type;
                var enemyPath = @"Prefabs/Units/Enemy" + type;
                unitAssets.assetsByName.Add(type, Resources.Load<GameObject>(path));
                unitAssets.assetsByName.Add("Enemy" + type, Resources.Load<GameObject>(enemyPath));
            }
        }

        private void LoadUi()
        {
            var uiCanvases = resoursesEntity.Set<UiCanvasesComponent>();
            uiCanvases.UserInterface = GuiHelper.InstantiateAllButtons(Resources.Load<Canvas>(@"Prefabs/GUI/UserInterface"), world);
            uiCanvases.PauseMenu = GuiHelper.InstantiateAllButtons(Resources.Load<Canvas>("Prefabs/GUI/PauseMenu"), world);
            uiCanvases.LoseMenu = GuiHelper.InstantiateAllButtons(Resources.Load<Canvas>("Prefabs/GUI/LoseMenu"), world);
            uiCanvases.WonMenu = GuiHelper.InstantiateAllButtons(Resources.Load<Canvas>("Prefabs/GUI/WonMenu"), world);
            uiCanvases.LoseMenu.enabled = false;
            uiCanvases.WonMenu.enabled = false;
            uiCanvases.PauseMenu.enabled = false;
        }
    }
}
