using System;
using UnityEngine;

namespace Githero.Helpers
{
    public class TriggerHelper : MonoBehaviour
    {
        public Action<Collider> actionOnTriggerEnter { private get; set; }

        private void OnTriggerEnter(Collider other)
        {
            actionOnTriggerEnter?.Invoke(other);
        }

    }

}
