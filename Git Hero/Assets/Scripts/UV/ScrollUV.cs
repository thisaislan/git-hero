using UnityEngine;

namespace Githero.UV
{

    public class ScrollUV : MonoBehaviour
    {

        void Update()
        {
            var offsetX = 0f;
            var offsetY = Time.time * -(Constants.Forces.GravityMultiplier * 0.03f);

            var offsetVector = new Vector2(offsetX, offsetY);
            var renderer = GetComponent<Renderer>();

            renderer.material.mainTextureOffset = offsetVector;
        }
    }

}