﻿using UnityEngine;
using TMPro;
using System.Text;
using Githero.Ultils;
using Githero.Game.Helpers;
using Githero.Game.GameObjects;
using System;

namespace Githero.Game.Managers
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField]
        private TMP_InputField gitInputFiled;

        [SerializeField]
        private TriggerHelper newNoteTriggerHelper;

        [SerializeField]
        private TriggerHelper destroyerTriggerHelper;

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
            newNoteTriggerHelper.ActionOnTriggerEnter = (_) => SpawnNote();

            destroyerTriggerHelper.ActionOnTriggerEnter = (collider) =>
            {
                Destroy(collider.gameObject);
                AddNewNote();
            };
        }

        public void StartGame() =>
            AddNewNotes(MaxSheetMusicSize, SpawnNote);

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