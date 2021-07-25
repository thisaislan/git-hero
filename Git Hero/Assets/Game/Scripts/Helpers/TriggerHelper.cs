using System;
using UnityEngine;

namespace Githero.Game.Helpers
{
    public class TriggerHelper : MonoBehaviour
    {
        public bool OnTrigger { get; private set; }

        public Action<Collider> ActionOnTriggerEnter { private get; set; }

        public Action<Collider> ActionOnTriggerExit { private get; set; }

        private void OnTriggerEnter(Collider other)
        {
            OnTrigger = true;
            ActionOnTriggerEnter?.Invoke(other);
        }

        private void OnTriggerExit(Collider other)
        {
            OnTrigger = false;
            ActionOnTriggerExit?.Invoke(other);
        }

    }

}
