using Leopotam.Ecs;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSelectionSystem : IEcsRunSystem, IEcsInitSystem, IEcsDestroySystem
{
    EcsWorld world = null;
    EcsFilter<UnitSelectionEvent> unitSelectionFilter = null;
    EcsFilter<SelectedUnit> selectedUnitFilter= null;

    void IEcsInitSystem.Init()
    {

    }

    void IEcsRunSystem.Run()
    {
        foreach(var selectionEventId in unitSelectionFilter)
        {
            world.NewEntityWith<SelectedUnit>(out var selectedUnit);
            selectedUnit.unit = unitSelectionFilter.Get1[selectionEventId].unit;

            Debug.Log("Unit " + selectedUnit.unit.id + " is chosen");

            unitSelectionFilter.Get1[selectionEventId].unit = null;
            unitSelectionFilter.Entities[selectionEventId].Destroy();
        }
    }

    void IEcsDestroySystem.Destroy()
    {
        foreach(var selectedUnitId in selectedUnitFilter)
        {
            selectedUnitFilter.Get1[selectedUnitId].unit = null;
            selectedUnitFilter.Entities[selectedUnitId].Destroy();
        }
    }
}
