using Githero.Game.Helpers;
using Githero.Managers;
using System.Collections;
using UnityEngine;

namespace Githero.Game.GameObjects
{
    public class Mark : MonoBehaviour
    {
        [SerializeField, Range(0, 3)]
        private int markPosition;

        [SerializeField]
        private TriggerHelper triggerHelper;

        [SerializeField]
        private GameObject halo;

        [SerializeField]
        private AudioSource audioSource;        

        public Note NoteOnCollision { get; private set; }

        private const float ShineTime = 0.15f;

        private bool isShining = false;

        private void Awake()
        {
            triggerHelper.ActionOnTriggerEnter = (collider) =>
                NoteOnCollision = collider.gameObject.GetComponent<Note>();

            triggerHelper.ActionOnTriggerExit = (collider) =>
                NoteOnCollision = null;

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

        public void Shine()
        {
            if (!isShining)
            {
                isShining = true;
                StartCoroutine(StarShine());
            }
        }

        public void Play() =>
            AudioManager.Instance.PlayAudioSource(audioSource);

        private IEnumerator StarShine()
        {
            halo.SetActive(true);

            yield return new WaitForSeconds(ShineTime);

            halo.SetActive(false);

            isShining = false;
        }

    }

}