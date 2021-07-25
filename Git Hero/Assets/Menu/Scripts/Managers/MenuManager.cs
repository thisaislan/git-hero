using Githero.Menu.Ultils;
using Githero.Ultils;
using UnityEngine;
using UnityEngine.UI;

namespace Githero.Menu.Managers
{
    public class MenuManager : MonoBehaviour
    {
        [SerializeField]
        private Animator animator;

        [SerializeField]
        private InputField inputField;

        [SerializeField]
        private Transform loadingTrasnform;

        [SerializeField]
        private Text errorText;

        private const string GitUrlEnd = ".git";
        private const string TriggerCloseScene = "TriggerCloseScene";

        private GitUtils gitUtils = new GitUtils();
        private ReaderFile readerFileUtils = new ReaderFile();

        public void HandleInputGitUrl()
        {
            var inputFieldText = inputField.text;

            if (inputFieldText.Contains(GitUrlEnd))
            {
                StartLoading();
                GetGitGraph(inputFieldText);
            }

            CheckErrorText();
        }

        public void LoadGameScene() =>
            Core.App.LoadGameScene();

        private void StartLoading()
        {
            SetActiveOnInput(false);
            SetActiveOnLoading(true);
        }

        private void SetActiveOnInput(bool active) =>
            SetGameObjectActive(inputField.gameObject, active);

        private void SetGameObjectActive(GameObject gameObject, bool active) =>
            gameObject.SetActive(active);

        private void SetActiveOnLoading(bool active) =>
            SetGameObjectActive(loadingTrasnform.gameObject, active);

        private void CheckErrorText()
        {
            if (errorText.IsActive()) { SetActiveOnError(false); }
        }

        private void SetActiveOnError(bool active) =>
            SetGameObjectActive(errorText.gameObject, active);

        private void GetGitGraph(string gitUrl)
        {
            try
            {
                gitUtils.GetRepoGraph(gitUrl, () =>
                {
                    ReadFirstLine(gitUrl);
                });
            }
            catch
            {
                SetError();
            }
        }

        private void SetError()
        {
            SetActiveOnError(true);
            SetActiveOnLoading(false);
            SetActiveOnInput(true);
        }

        private void ReadFirstLine(string gitUrl)
        {
            readerFileUtils.ReadLine(Constants.Strings.GraphFilePath, 0, (line) =>
            {
                if (IsAValidLine(line))
                {
                    Core.App.NameOfGitProject = GetNameOfGitProject(gitUrl);
                    StartCloseSceneAnimation();
                }
                else
                {
                    SetError();
                }
            });
        }

        private bool IsAValidLine(string line) =>
            line != null && RemoveWhitespaces(line).Length > 0;

        private string RemoveWhitespaces(string line) => line.Replace(" ", "");

        private string GetNameOfGitProject(string gitUrl)
        {
            var startIndex = gitUrl.LastIndexOf("/") + 1;
            var length = gitUrl.Length - startIndex;

            var name = gitUrl.Substring(startIndex, length).Replace(GitUrlEnd, "");

            return CapitalizeFirstLetter(name);
        }

        private string CapitalizeFirstLetter(string value) =>
            value.Substring(0, 1).ToUpper() + value.Remove(0, 1).ToLower();

        private void StartCloseSceneAnimation() =>
            animator.SetTrigger(TriggerCloseScene);

    }

}