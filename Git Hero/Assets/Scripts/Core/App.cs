using UnityEngine.SceneManagement;

namespace Core
{
    public static class App
    {
        private const int IndexOfMenuScne = 0;
        private const int IndexOfGameScne = 1;

        public static string NameOfGitProject;

        public static void LoadGameScene() =>
            LoadScene(IndexOfGameScne);

        public static void LoadMenuScene() =>
            LoadScene(IndexOfMenuScne);

        public static void LoadScene(int indexOfScene) =>
            SceneManager.LoadScene(indexOfScene);

    }

}
