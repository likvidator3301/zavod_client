using System.Collections.Generic;
using Entities;
using UnityEngine;

namespace Systems
{
    public class MovementSystem
    {
        private readonly List<IUnitEntity> units;

        public MovementSystem(List<IUnitEntity> units)
        {
            this.units = units;
        }

        public void UpdateTargets(Vector3 targetPosition)
        {
            foreach (var unit in units)
            {
                unit.Agent.SetDestination(targetPosition);
                unit.Agent.speed = unit.Info.MoveSpeed;
            }
        }
    }
}