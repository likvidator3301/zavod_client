﻿using System;
using Components;
using UnityEditor;
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
        
        public static void SetFields(this UnitComponent unitComponent, GameObject obj, UnitTag tag)
        {
            unitComponent.Object = obj;
            unitComponent.Tag = tag;
            unitComponent.Guid = Guid.NewGuid();
            unitComponent.Animator = obj.GetComponent<Animator>();
            unitComponent.Agent = obj.GetComponent<NavMeshAgent>();
        }
    }
}