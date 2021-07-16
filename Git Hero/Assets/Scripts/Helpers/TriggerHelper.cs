using System;
using UnityEngine;

namespace Githero.Helpers
{
    public class TriggerHelper : MonoBehaviour
    {
        public bool OnTrigger { get; private set; }

        public Action<Collider> ActionOnTriggerEnter { private get; set; }

        private void OnTriggerEnter(Collider other)
        {
            OnTrigger = true;
            ActionOnTriggerEnter?.Invoke(other);
        }

        private void OnTriggerExit(Collider other)
        {
            OnTrigger = false;
        }

    }

}
