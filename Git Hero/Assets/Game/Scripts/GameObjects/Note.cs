using UnityEngine;

namespace Githero.Game.GameObjects
{
    public class Note : MonoBehaviour
    {
        public void FixedUpdate()
        {
            var rigidbody = GetComponent<Rigidbody>();

            rigidbody.AddForce(
                Physics.gravity * Constants.Forces.GravityMultiplier,
                ForceMode.Acceleration);
        }

        public void SetColor(int numberOfNote)
        {
            string colorHex;
            Color color;

            switch (numberOfNote)
            {
                case 0:
                    colorHex = Constants.Colors.FirstLineHexColor;
                    break;
                case 1:
                    colorHex = Constants.Colors.SecondLineHexColor;
                    break;
                case 2:
                    colorHex = Constants.Colors.ThirdLineHexColor;
                    break;
                // case 3
                default:
                    colorHex = Constants.Colors.FourthLineHexColor;
                    break;
            }

            if (ColorUtility.TryParseHtmlString(colorHex, out color))
            {
                GetComponent<Renderer>().material.color = color;
            }
        }

    }

}