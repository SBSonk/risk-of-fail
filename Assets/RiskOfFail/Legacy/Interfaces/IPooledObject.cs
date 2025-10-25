using UnityEngine;

namespace RiskOfFail.ObjectPooling
{
    public interface IPooledObject
    {
        public GameObject PrefabOrigin { get; set; }

        public void OnDeploy();
        public void OnReturn();
    }
}