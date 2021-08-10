using UnityEngine;
using System.Text;
using Githero.Ultils;
using Githero.Game.Helpers;
using Githero.Game.GameObjects;
using System;
using Githero.UI;
using static UnityEngine.ParticleSystem;
using Githero.Managers;

namespace Githero.Game.Managers
{
    public class GameManager : MonoBehaviour
    {
        /* 
         * At first I wanted to create a machinne state to deal with this,
         * but at some point I found that this would be too much for a simple proof of concept.
         * But if you want to see a nice and simple explanation of the finite state machine, follow this link:
         * 
         * Channel: Infallible Code
         * Video: How to Code a Simple State Machine (Unity Tutorial):
         * Link: https://www.youtube.com/watch?v=G1bd75R10m4
        */

        private enum GameState
        {
            Open,
            Play,
            Close
        }

        [SerializeField]
        private GameSceneUI gameSceneUI;

        [SerializeField]
        private TriggerHelper newNoteTriggerHelper;

        [SerializeField]
        private TriggerHelper destroyerTriggerHelper;

        [SerializeField]
        private Mark leftMark;

        [SerializeField]
        private Mark upMark;

        [SerializeField]
        private Mark downMark;

        [SerializeField]
        private Mark rightMark;

        [SerializeField]
        private Transform noteObject;

        [SerializeField]
        private GameObject explosion;

        [SerializeField]
        private AudioSource baseAudioSource;

        private ParticleSystem explosionParticleSystem;

        private MainModule mainModuleExplosionParticleSystem;

        private const int MaxSheetMusicSize = 25;
        private const int NumberOfNotes = 4;

        private const string NoteRepresentationOnLine = "*";

        private const float NoteYPosition = 3f;
        private const float NoteZPosition = 28f;

        private const float FirstNoteXPosition = -3.6f;
        private const float SecondNoteXPosition = -1.2f;
        private const float ThirdNoteXPosition = 1.2f;
        private const float FourthNoteXPosition = 3.6f;

        private GameState currentGameState = GameState.Open;
        private ReaderFile readerFileUtils = new ReaderFile();
        private StringBuilder sheetMusicString = new StringBuilder(MaxSheetMusicSize);

        private bool hasMoreLinesToRead = true;
        private int skipLines = 0;

        private void Awake()
        {
            explosionParticleSystem = explosion.GetComponent<ParticleSystem>();
            mainModuleExplosionParticleSystem = explosionParticleSystem.main;

            gameSceneUI.SetGameplayTitle(Core.App.NameOfGitProject);
            
            newNoteTriggerHelper.ActionOnTriggerEnter = (_) => SpawnNote();

            destroyerTriggerHelper.ActionOnTriggerEnter = (collider) =>
            {
                Destroy(collider.gameObject);
                AddNewNote();
                gameSceneUI.NewMiss();
            };
        }

        private void Update()
        {
            if (currentGameState == GameState.Open) { HandleOpenStateInput(); }
            else { HandlePlayStateInput(); }
        }

        public void StartGame() =>
            AddNewNotes(MaxSheetMusicSize, SpawnNote);

        public void StartExit()
        {
            if (currentGameState != GameState.Close)
            {
                currentGameState = GameState.Close;
                gameSceneUI.StarExitAnimation();
            }
        }

        private void HandleOpenStateInput()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                currentGameState = GameState.Play;
                gameSceneUI.StarPlayAnimation();
                AudioManager.Instance.PlayBackgroundAudioSource(baseAudioSource, this);
            }
        }

        private void HandlePlayStateInput()
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow)) { HandleArrowInput(leftMark); }
            else if (Input.GetKeyDown(KeyCode.UpArrow)) { HandleArrowInput(upMark); }
            else if (Input.GetKeyDown(KeyCode.RightArrow)) { HandleArrowInput(rightMark); }
            else if (Input.GetKeyDown(KeyCode.DownArrow)) { HandleArrowInput(downMark); }
        }

        private void HandleArrowInput(Mark mark)
        {
            var noteOnColision = mark.NoteOnCollision;

            mark.Shine();

            if (noteOnColision != null)
            {
                PlayExplosion(noteOnColision.transform.position, noteOnColision.Color);
                mark.Play();

                Destroy(noteOnColision.gameObject);
                gameSceneUI.NewHit();
            }
            else
            {
                gameSceneUI.NewMiss();
            }
        }

        private void PlayExplosion(Vector3 position, Color color)
        {
            explosion.transform.position = position;
            mainModuleExplosionParticleSystem.startColor = color;
            explosionParticleSystem.Play();
        }

        private void LoadMenuScene() => Core.App.LoadMenuScene();

        private void AddNewNote() =>
            AddNewNotes(sheetMusicString.Length + 1);

        private void AddNewNotes(int expectedSize, Action actionOnEnd = null)
        {
            if (!hasMoreLinesToRead) { return; }

            readerFileUtils.ReadLine(Githero.Constants.Strings.GraphFilePath, skipLines, (line) =>
            {
                if (line == null)
                {
                    hasMoreLinesToRead = false;
                    actionOnEnd?.Invoke();
                }
                else
                {
                    skipLines++;
                    HandleNewLine(line);

                    if (sheetMusicString.Length < expectedSize) { AddNewNotes(expectedSize, actionOnEnd); }
                    else { actionOnEnd?.Invoke(); }
                }
            });
        }

        private void HandleNewLine(string line)
        {
            if (HasNote(line)) { AddOnSheetMusic(line); }
        }

        private bool HasNote(string line) =>
            line.Contains(NoteRepresentationOnLine);

        private void AddOnSheetMusic(string line)
        {
            var note = ConvertToNote(line);
            sheetMusicString.Append(note);
        }

        private int ConvertToNote(string line)
        {
            line = RemoveWhitespaces(line);
            return line.IndexOf(NoteRepresentationOnLine) % NumberOfNotes;
        }

        private string RemoveWhitespaces(string line) => line.Replace(" ", "");

        private void SpawnNote()
        {
            if (sheetMusicString.Length != 0)
            {
                var note = GetFirstNote();

                RemoveFirstNote();
                SpawnNote(note);
            }
            else
            {
                if (currentGameState != GameState.Close)
                {
                    currentGameState = GameState.Close;
                    gameSceneUI.StartCloseScene();
                    AudioManager.Instance.StopBackgroundAudioSource(this);
                }
            }
        }

        private int GetFirstNote() =>
            Int16.Parse(sheetMusicString.ToString(0, 1));

        private void RemoveFirstNote() =>
            sheetMusicString.Remove(0, 1);

        private void SpawnNote(int note)
        {
            var noteXPosition = GetNoteXPosition(note);
            var notePosition = new Vector3(noteXPosition, NoteYPosition, NoteZPosition);

            var noteInstance = Instantiate(noteObject, notePosition, noteObject.rotation);

            noteInstance.GetComponent<Note>().Init(note);
        }

        private float GetNoteXPosition(int note)
        {
            switch (note)
            {
                case 0: return FirstNoteXPosition;
                case 1: return SecondNoteXPosition;
                case 2: return ThirdNoteXPosition;
                // case 3
                default: return FourthNoteXPosition;
            }
        }

    }

}
