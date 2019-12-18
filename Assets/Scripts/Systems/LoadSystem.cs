using System;
using Components;
using Leopotam.Ecs;
using UnityEngine;

namespace Systems
{
    class LoadSystem : IEcsInitSystem
    {
        private readonly EcsWorld world = null;
        private readonly GameDefinitions definitions = null;

        public void Init()
        {
            LoadMainCamera();
            LoadBuildingAssets();
            LoadUiAssets();
            LoadUi();
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

        private void LoadBuildingAssets()
        {
            world.NewEntityWith(out BuildingAssetComponent barrak);
            barrak.buildingAsset = definitions.BuildingDefinitions.BarracsAsset;

            world.NewEntityWith(out BuildingAssetComponent kiosk);
            kiosk.buildingAsset = definitions.BuildingDefinitions.KioskAsset;
        }

        private void LoadUiAssets()
        {
            world.NewEntityWith(out PlayerResourcesComponent playerAsset);
            Debug.Log(definitions.GuiDefinitions == null);
            playerAsset.ResoursesUiDisplay = GameObject.Instantiate(definitions.GuiDefinitions.PlayerInfo);
        }

        private void LoadUi()
        {
            GuiHelper.InstantiateAllButtons(definitions.GuiDefinitions.BuildMenu, world);
        }
    }
}