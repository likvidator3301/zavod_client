using Leopotam.Ecs;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSelectionSystem : IEcsRunSystem, IEcsDestroySystem
{
    EcsWorld world = null;
    EcsFilter<UnitSelectionEvent> unitSelectionFilter = null;
    EcsFilter<SelectedUnit> selectedUnitFilter= null;


    public void Run()
    {
        foreach(var selectionEventId in unitSelectionFilter)
        {
            world.NewEntityWith<SelectedUnit>(out var selectedUnit);
            selectedUnit.Unit = unitSelectionFilter.Get1[selectionEventId].Unit;

            Debug.Log("Unit " + selectedUnit.Unit.Id + " is chosen");

            unitSelectionFilter.Entities[selectionEventId].Destroy();
        }
    }

    public void Destroy()
    {
        foreach(var selectedUnitId in selectedUnitFilter)
        {
            selectedUnitFilter.Entities[selectedUnitId].Destroy();
        }
    }
}
