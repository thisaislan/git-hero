using Githero.Game.Helpers;
using UnityEngine;

namespace Githero.Game.GameObjects
{
    public class Mark : MonoBehaviour
    {
        [SerializeField, Range(0, 3)]
        private int markPosition;

        [SerializeField]
        private TriggerHelper triggerHelper;

        public GameObject GameObjectOnCollision { get; private set; }

        private void Awake()
        {
            triggerHelper.ActionOnTriggerEnter = (collider) =>
                GameObjectOnCollision = collider.gameObject;

            triggerHelper.ActionOnTriggerExit = (collider) =>
                GameObjectOnCollision = null;

            SetColor();
        }

        public void SetColor()
        {
            string colorHex;
            Color color;

            switch (markPosition)
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