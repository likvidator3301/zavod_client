using System;
using UnityEngine;


[CreateAssetMenu(menuName = "zavod_client/BuildingDefinitions", fileName = "BuildingDefinitions")]
public class BuildingsDefinitions : ScriptableObject
{
    public GameObject BarracsAsset;
    public int SecondsForWarriorCreate;
    public TimeSpan WarriorCreateTime => TimeSpan.FromSeconds(SecondsForWarriorCreate);
}


