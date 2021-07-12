using System;
using System.Diagnostics;
using System.Threading;

namespace Githero.Ultils
{
    public class GitUtils
    {
        private string repoFolderName = "Repo";

#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
        private string fileName = "powerShell.exe";
        private string deleteFolderCommand = "Remove-Item -Recurse -Force";
        private string createFolderCommand = "New-Item -itemType Directory -Name";
        private string concateneCommandsChar = ";";
#else
        private string fileName = "/bin/bash";
        private string deleteFolderCommand = "rm -rf";
        private string createFolderCommand = "mkdir";
        private string concateneCommandsChar = "&&";
#endif

        private string gotToFolderCommand = "cd";
        private string cloneBareCommand = "git clone --bare";

        public void GetRepo(string gitCloneLink)
        {
            new Thread(delegate ()
            {
                DeleteRepoFolder();
                CreateFolder();
                CloneBareIntoRepoFolder(gitCloneLink);

            }).Start();
        }

        private void DeleteRepoFolder() =>
            StartCommand($"{deleteFolderCommand} {repoFolderName}", false);

        private void CreateFolder() =>
            StartCommand($"{createFolderCommand} {repoFolderName}");

        private void CloneBareIntoRepoFolder(string gitCloneLink) =>
            StartCommand(
                $"{gotToFolderCommand} {repoFolderName}" +
                $" {concateneCommandsChar}" +
                $" {cloneBareCommand} {gitCloneLink}");

        private void StartCommand(string argument, bool allowException = true)
        {
            try
            {
#if !UNITY_EDITOR_WIN || !UNITY_STANDALONE_WIN
                argument = " -c \"" + argument + " \"";                
#endif
                var startInfo = new ProcessStartInfo()
                {
                    FileName = fileName,
                    UseShellExecute = false,
                    RedirectStandardError = true,
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true,
                    Arguments = argument
                };

                var process = Process.Start(startInfo);
                process.WaitForExit();
                process.Close();
            }
            catch (Exception exception)
            {
                if (allowException) { throw new Exception(exception.Message); }
            }

        }

    }

}