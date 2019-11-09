﻿using System;
using System.Collections.Generic;
using Components;
using Entities;
using Leopotam.Ecs;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Systems
{
    public class UnitConditionChangeSystem : IEcsRunSystem
    {
        private PlayerComponent player;
        private WorldComponent world;
        private const float minHeight = 2.5f;

        public void Run()
        {
            DestroyDeadUnits();
        }

        public static void CreateUnit(
            GameObject prefab,
            UnitTags unitTag,
            Vector3 position,
            Dictionary<GameObject, IUnitEntity> units)
        {
            position.y = Math.Min(position.y, minHeight);
            var newUnit = Object.Instantiate(prefab, position, Quaternion.identity);
            switch (unitTag)
            {
                case UnitTags.Warrior:
                    {
                        var warriorEntity = new WarriorEntity(newUnit, UnitTags.Warrior);
                        units.Add(newUnit, warriorEntity);
                        break;
                    }
                case UnitTags.EnemyWarrior:
                    {
                        var warriorEntity = new WarriorEntity(newUnit, UnitTags.EnemyWarrior);
                        units.Add(newUnit, warriorEntity);
                        break;
                    }
            }
        }

        public void DestroyDeadUnits()
        {
            var units = world.Units;
            var toDelete = new List<IUnitEntity>();
            foreach (var unit in units.Values)
            {
                if (unit.ConditionComponent.CurrentHp <= 0 && units.ContainsKey(unit.Object))
                    toDelete.Add(unit);
            }

            foreach (var unit in toDelete)
            {
                if (player.SelectedUnits.Contains(unit))
                    player.SelectedUnits.Remove(unit);
                units.Remove(unit.Object);
                Object.Destroy(unit.Object);
            }
        }
    }
}