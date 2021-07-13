using System;
using System.Diagnostics;
using System.Threading;

namespace Githero.Ultils
{
    public class GitUtils
    {
        private string repoFolderName = "Repo";
        private string graphFileName = "graph";

#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
        private string fileName = "powerShell.exe";
        private string deleteFileCommand = "Remove-Item -Recurse -Force";
        private string createFolderCommand = "New-Item -itemType Directory -Name";
        private string concateneCommandsChar = ";";
        private string createGitGraphCommand =
            "git --no-pager log --graph --full-history --all --pretty=format:\"\" > ..\\..\\";

#else
        private string fileName = "/bin/bash";
        private string deleteFileCommand = "rm -rf";
        private string createFolderCommand = "mkdir";
        private string concateneCommandsChar = "&&";
        private string createGitGraphCommand =
            "git --no-pager log --graph --full-history --all --pretty=format:\"\" > ../../";
#endif

        private string gotToFolderCommand = "cd";
        private string gotToFirstAvaliableFolder = "cd *";
        private string cloneBareCommand = "git clone --bare";

        public void GetRepoGraph(string gitCloneLink, Action<string> getGraphNameAction)
        {
            new Thread(delegate ()
            {
                DeleteGraphFile();
                DeleteRepoFolder();
                CreateFolder();
                CloneBareIntoRepoFolder(gitCloneLink);
                CreateGraphFile();
                DeleteRepoFolder();
                getGraphNameAction(graphFileName);

            }).Start();
        }

        private void DeleteGraphFile() =>
            StartCommand($"{deleteFileCommand} {graphFileName}", false);

        private void DeleteRepoFolder() =>
            StartCommand($"{deleteFileCommand} {repoFolderName}", false);

        private void CreateFolder() =>
            StartCommand($"{createFolderCommand} {repoFolderName}");

        private void CloneBareIntoRepoFolder(string gitCloneLink) =>
            StartCommand(
                $"{gotToFolderCommand} {repoFolderName}" +
                $" {concateneCommandsChar}" +
                $" {cloneBareCommand} {gitCloneLink}");

        private void CreateGraphFile()
        {
            StartCommand(
            $"{gotToFolderCommand} {repoFolderName}" +
            $" {concateneCommandsChar}" +
            $" {gotToFirstAvaliableFolder}" +
            $" {concateneCommandsChar}" +
            $" {createGitGraphCommand}{graphFileName}");
        }

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