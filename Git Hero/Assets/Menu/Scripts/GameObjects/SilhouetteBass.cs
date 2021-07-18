using UnityEngine;

namespace Githero.Menu.GameObjects
{
    public class SilhouetteBass : MonoBehaviour
    {

        [SerializeField]
        private float smothtoRotate;

        [SerializeField]
        private bool isForwardRotation;

        private void Update() =>
            Rotate();

        private void Rotate()
        {
            var position = transform.position;
            var calculetedSmooth = Time.deltaTime * smothtoRotate;
            var axis = isForwardRotation ? Vector3.forward : Vector3.back;

            transform.RotateAround(position, axis, calculetedSmooth);
        }
    }

}