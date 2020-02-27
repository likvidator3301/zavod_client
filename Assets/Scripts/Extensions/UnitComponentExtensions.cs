using System;
using Components;
using UnityEngine;
using UnityEngine.AI;

namespace Systems
{
    public static class UnitComponentExtensions
    {
        public static void SetFields(this UnitComponent unitComponent, GameObject obj, UnitTag tag, Guid guid)
        {
            unitComponent.Object = obj;
            unitComponent.Tag = tag;
            unitComponent.Guid = guid;
            unitComponent.Animator = obj.GetComponent<Animator>();
            unitComponent.Agent = obj.GetComponent<NavMeshAgent>();
        }
    }
}