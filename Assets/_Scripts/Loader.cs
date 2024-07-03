using UnityEngine.SceneManagement;

public static class Loader {
    public enum Scene {
        LoadingScene,
        MainMenuScene,
        PlayingScene
    }

    private static Scene targetScene;

    public static void Load(Scene scene)
    {
        targetScene = scene;
        SceneManager.LoadScene(Scene.LoadingScene.ToString());
    }

    public static void LoaderCallback()
    {
        SceneManager.LoadScene(targetScene.ToString());
    }
}
