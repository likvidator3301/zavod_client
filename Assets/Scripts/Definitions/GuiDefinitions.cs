using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "zavod_client/GuiDefinitions", fileName = "GuiDefinitions")]
public class GuiDefinitions : ScriptableObject
{
    public Canvas BuildMenu;
    public Canvas InBuildingMenu;
    public Canvas PlayerInfo;
    public Canvas DefaultInBuildingMenu;
}