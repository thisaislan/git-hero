using UnityEngine;

namespace Githero.UV
{

    public class ScrollUV : MonoBehaviour
    {
        [SerializeField]
        private float scrollX;
        [SerializeField]
        private float scrollY;
        [SerializeField]

        void Update()
        {
            var offsetX = Time.time * scrollX;
            var offsetY = Time.time * scrollY;

            var offsetVector = new Vector2(offsetX, offsetY);
            var renderer = GetComponent<Renderer>();

            renderer.material.mainTextureOffset = offsetVector;
        }
    }

}