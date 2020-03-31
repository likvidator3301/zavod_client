using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Leopotam.Ecs;
using Components;
using Systems;

public class UnitLayoutUISystem : IEcsRunSystem, IEcsInitSystem
{
    private readonly EcsWorld world = null;
    private readonly EcsFilter<UnitSpawnedEvent> unitSpawnedEventFilter = null;
    private readonly EcsFilter<UnitButton> unitButtonsFilter = null;

    private readonly UnitLayout layout = new UnitLayout();
    private readonly PlayerComponent player = null;

    private GameObject unitLayoutPrefab;
    private GameObject unitButtonPrefab;
    private GameObject canvas;

    public void Init()
    {
        canvas = Object.Instantiate(Resources.Load("Prefabs/GUI/MainCanvas") as GameObject);

        unitLayoutPrefab = Resources.Load("Prefabs/GUI/UnitLayoutMenu") as GameObject;
        unitButtonPrefab = Resources.Load("Prefabs/GUI/UnitButton") as GameObject;

        InstantiateLayoutMenu();
    }

    public void Run()
    {
        foreach (var spawnedUnit in unitSpawnedEventFilter)
        {
            var newButton = InstantiateButtonObject();

            world.NewEntityWith<ButtonComponent>(out var newButtonComponent);
            newButtonComponent.buttonName = unitSpawnedEventFilter.Get1[spawnedUnit].Unit.Get<UnitComponent>().Tag.ToString();
            newButtonComponent.button = newButton.GetComponent<Button>();
            newButtonComponent.bounds = newButton.GetComponent<Button>().GetLayoutButtonBounds();

            world.NewEntityWith<UnitButton>(out var newUnitButton);
            newUnitButton.Button = newButtonComponent;
            newUnitButton.Unit = unitSpawnedEventFilter.Get1[spawnedUnit].Unit;

            newUnitButton.Button.button.onClick.AddListener(() => SelectAttachedUnit(newUnitButton.Unit));

            newButton.GetComponentInChildren<Text>().text = newButtonComponent.buttonName;
            unitSpawnedEventFilter.Entities[spawnedUnit].Destroy();
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
            SelectAllLayoutUnits();
    }

    private void InstantiateLayoutMenu()
    {
        layout.LayoutObject = Object.Instantiate(unitLayoutPrefab);
        layout.LayoutObject.transform.SetParent(canvas.transform);
        layout.LayoutObject.GetComponent<RectTransform>().position = new Vector3(canvas.transform.position.x, 0, canvas.transform.position.z);
    }

    private GameObject InstantiateButtonObject()
    {
        var button = Object.Instantiate(unitButtonPrefab) as GameObject;
        button.transform.SetParent(layout.LayoutObject.GetComponentInChildren<GridLayoutGroup>().transform);
        return button;
    }

    private void SelectAttachedUnit(EcsEntity unit)
    {
        if (!Input.GetKey(KeyCode.LeftAlt))
        {
            player.SelectedUnits.DehighlightObjects();
            player.SelectedUnits.Clear();
        }
        player.SelectedUnits.Add(unit);
        player.SelectedUnits.HighlightObjects();
    }

    private void SelectAllLayoutUnits()
    {
        player.SelectedUnits.DehighlightObjects();
        player.SelectedUnits.Clear();
        foreach (var unit in unitButtonsFilter)
                player.SelectedUnits.Add(unitButtonsFilter.Get1[unit].Unit);
        player.SelectedUnits.HighlightObjects();
    }
}
