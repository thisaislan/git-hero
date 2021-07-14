using UnityEngine;
using TMPro;
using System.Text;
using Githero.Ultils;
using System;

namespace Githero.Managers
{

    public class GameManager : MonoBehaviour
    {
        [SerializeField]
        private TMP_InputField gitInputFiled;

        private const int MaxSheetMusicSize = 25;
        private const int NumberOfNotes = 4;
        private const string NoteRepresentationOnLine = "*";

        private ReaderFileUtils readerFileUtils = new ReaderFileUtils();
        private StringBuilder sheetMusicString = new StringBuilder(MaxSheetMusicSize);

        private bool hasMoreLinesToRead = true;
        private int skipLines = 0;
        private string graphPath;

        public void GetGitGraph()
        {
            if (gitInputFiled.text.Length != 0)
            {
                var gitUtils = new GitUtils();

                gitUtils.GetRepoGraph(gitInputFiled.text, (string graphPath) =>
                {
                    this.graphPath = graphPath;
                    AddNewNotes(MaxSheetMusicSize, () => StartGame());
                });
            }
        }

        private void RemoveFirstNote() =>
            sheetMusicString.Remove(0, 1);

        private void AddNewNote() =>
            AddNewNotes(sheetMusicString.Length + 1);

        private void AddNewNotes(int sizeExpected, Action actionOnEnd = null)
        {
            if (!hasMoreLinesToRead) { return; }

            readerFileUtils.Read(graphPath, skipLines, (line) =>
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

                    if (sheetMusicString.Length < sizeExpected) { AddNewNotes(sizeExpected, actionOnEnd); }
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

        private void StartGame()
        {
            // TODO
        }

    }

}
