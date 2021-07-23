using System;
using UnityEngine;

namespace Githero.Game.Helpers
{
    public class TriggerHelper : MonoBehaviour
    {
        public bool OnTrigger { get; private set; }

        public GameObject GameObjectOnCollision { get; private set; }

        public Action<Collider> ActionOnTriggerEnter { private get; set; }

        private void OnTriggerEnter(Collider other)
        {
            OnTrigger = true;
            GameObjectOnCollision = other.gameObject;
            ActionOnTriggerEnter?.Invoke(other);
        }

        private void OnTriggerExit(Collider other)
        {
            OnTrigger = false;
            GameObjectOnCollision = null;
        }

    }

}
