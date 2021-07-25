using UnityEngine;
using System.Text;
using Githero.Ultils;
using Githero.Game.Helpers;
using Githero.Game.GameObjects;
using System;
using Githero.UI;

namespace Githero.Game.Managers
{
    public class GameManager : MonoBehaviour
    {

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
        private Mark rightMark;

        [SerializeField]
        private Mark downMark;

        [SerializeField]
        private Transform noteObject;

        private const int MaxSheetMusicSize = 25;
        private const int NumberOfNotes = 4;

        private const string NoteRepresentationOnLine = "*";

        private const float NoteYPosition = 3f;
        private const float NoteZPosition = 28f;

        private const float FirstNoteXPosition = -3.6f;
        private const float SecondNoteXPosition = -1.2f;
        private const float ThirdNoteXPosition = 1.2f;
        private const float FourthNoteXPosition = 3.6f;
                
        private ReaderFile readerFileUtils = new ReaderFile();
        private StringBuilder sheetMusicString = new StringBuilder(MaxSheetMusicSize);

        private bool hasMoreLinesToRead = true;
        private int skipLines = 0;

        private void Awake()
        {
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
            //TODO - fazer um tratar melhor a inicialização do jogo 
            if (Input.GetKeyDown(KeyCode.Space))
            {
                gameSceneUI.StarPlayAnimation();
            }

            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                if (leftMark.GameObjectOnCollision != null)
                {
                    Destroy(leftMark.GameObjectOnCollision);
                    gameSceneUI.NewHit();
                }
                else
                {
                    gameSceneUI.NewMiss();
                }
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                if (upMark.GameObjectOnCollision != null)
                {
                    Destroy(upMark.GameObjectOnCollision);
                    gameSceneUI.NewHit();
                }
                else
                {
                    gameSceneUI.NewMiss();
                }
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                if (rightMark.GameObjectOnCollision != null)
                {
                    Destroy(rightMark.GameObjectOnCollision);
                    gameSceneUI.NewHit();
                }
                else
                {
                    gameSceneUI.NewMiss();
                }
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                if (downMark.GameObjectOnCollision != null)
                {
                    Destroy(downMark.GameObjectOnCollision);
                    gameSceneUI.NewHit();
                }
                else
                {
                    gameSceneUI.NewMiss();
                }
            }
        }

        public void StartGame() =>
            AddNewNotes(MaxSheetMusicSize, SpawnNote);

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
                //TODO - avoid call that animation two times
                gameSceneUI.StartCloseScene();
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

            noteInstance.GetComponent<Note>().SetColor(note);
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
