using Components;
using UnityEngine;

[CreateAssetMenu(menuName = "zavod_client/GameDefinitions", fileName = "GameDefinitions")]
public class GameDefinitions : ScriptableObject
{
    public CameraDefinition CameraDefinitions;
    public GUIDefinitions GuiDefinitions;
    public BuildingsDefinitions BuildingDefinitions;
}