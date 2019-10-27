using Components;
using UnityEngine;

namespace Entities
{

    public interface IUnitEntity
    {
        IUnitInfo UnitInfo { get; }
        GameObject Prefabs { get; }
    }
}
