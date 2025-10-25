using System;
using RiskOfFail.Combat;
using UnityEngine;

namespace RiskOfFail.Combat.ScriptableObjects
{
    [Serializable]
    public abstract class StatusEffectBase : ScriptableObject
    {
        public string statusName;
        public Sprite statusIcon;

        [Range(0, 1)] public float inflictChance = 1;
        public int duration = 5;

        public virtual void OnApply(Alive target) {}
    
        public virtual void OnClear(Alive target) {}

        public abstract void OnStatusTick(Alive target);
    }
}

