using Leopotam.Ecs;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitListDisplaySystem : IEcsRunSystem, IEcsInitSystem, IEcsDestroySystem
{
    EcsWorld world = null;
    EcsFilter<UnitCreationEvent> unitCreationFilter = null;
    EcsFilter<UnitPanel> unitListFilter = null;
    EcsFilter<UnitCard> unitCardFilter = null;

    public void Init()
    {
        var unitList = GameObject.FindGameObjectWithTag("UnitList");
        world.NewEntityWith<UnitPanel>(out var ui);
        ui.Layout = unitList.GetComponent<GridLayoutGroup>();
    }

    public void Run()
    {
        foreach (var creationEventId in unitCreationFilter)
        {
            world.NewEntityWith<UnitCard>(out var card);
            card.Unit = unitCreationFilter.Get1[creationEventId].Unit;
            card.Prefab.GetComponentInChildren<Text>().text = card.Unit.Id.ToString();

            var unitCard = Object.Instantiate(card.Prefab);
            unitCard.GetComponent<Button>().onClick.AddListener(() => CreateSelectionEvent(card.Unit));

            foreach (var unitListId in unitListFilter)
            {
                var unitList = unitListFilter.Get1[unitListId]; 
                unitCard.transform.SetParent(unitList.Layout.transform);
            }

            unitCreationFilter.Entities[creationEventId].Destroy();
        }
    }
    void CreateSelectionEvent(Unit unit)
    {
        world.NewEntityWith<UnitSelectionEvent>(out var selected);
        selected.Unit = unit;
    }

    public void Destroy()
    {
        foreach (var unitCardId in unitCardFilter)
        {
            unitCardFilter.Entities[unitCardId].Destroy();
        }

        foreach (var unitListId in unitListFilter)
        {
            unitListFilter.Entities[unitListId].Destroy();
        }
    }
}
