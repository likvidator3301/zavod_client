using Leopotam.Ecs;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitListDisplaySystem : IEcsRunSystem, IEcsInitSystem, IEcsDestroySystem
{
    EcsWorld world = null;
    EcsFilter<UnitCreationEvent> unitCreationFilter = null;
    EcsFilter<UnitList> unitListFilter = null;
    EcsFilter<UnitCard> unitCardFilter = null;

    void IEcsInitSystem.Init()
    {
        var unitList = GameObject.FindGameObjectWithTag("UnitList");
        world.NewEntityWith<UnitList>(out var ui);
        ui.layout = unitList.GetComponent<GridLayoutGroup>();
    }

    void IEcsRunSystem.Run()
    {
        foreach (var creationEventId in unitCreationFilter)
        {
            world.NewEntityWith<UnitCard>(out var card);
            card.unit = unitCreationFilter.Get1[creationEventId].unit;
            card.prefab.GetComponentInChildren<Text>().text = card.unit.id.ToString();

            var unitCard = Object.Instantiate(card.prefab);
            unitCard.GetComponent<Button>().onClick.AddListener(() => CreateSelectionEvent(card.unit));

            foreach (var unitListId in unitListFilter)
            {
                var unitList = unitListFilter.Get1[unitListId]; 
                unitCard.transform.SetParent(unitList.layout.transform);
            }

            unitCreationFilter.Get1[creationEventId].unit = null;
            unitCreationFilter.Entities[creationEventId].Destroy();
        }
    }
    void CreateSelectionEvent(Unit unit)
    {
        world.NewEntityWith<UnitSelectionEvent>(out var selected);
        selected.unit = unit;
    }

    void IEcsDestroySystem.Destroy()
    {
        foreach (var unitCardId in unitCardFilter)
        {
            unitCardFilter.Get1[unitCardId].prefab = null;
            unitCardFilter.Get1[unitCardId].unit = null;
            unitCardFilter.Entities[unitCardId].Destroy();
        }

        foreach (var unitListId in unitListFilter)
        {
            unitListFilter.Get1[unitListId].layout = null;
            unitListFilter.Entities[unitListId].Destroy();
        }
    }
}
