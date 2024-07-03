using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePauseUI : MonoBehaviour {
    [SerializeField] private List<Transform> childrenTransformList;
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button optionsButton;
    [SerializeField] private Button returnMenuButton;

    private void Awake()
    {
        resumeButton.onClick.AddListener(() =>
        {
            GameHandler.Instance.TogglePauseGame();
        });
        optionsButton.onClick.AddListener(() =>
        {
            OptionsUI.Instance.Show();
        });
        returnMenuButton.onClick.AddListener(() =>
        {
            Loader.Load(Loader.Scene.MainMenuScene);
        });
        Hide();
    }

    private void Start()
    {
        GameHandler.Instance.OnPauseGame += GameHandler_OnPauseGame;
        GameHandler.Instance.OnUnpauseGame += GameHandler_OnUnpauseGame;
    }

    private void GameHandler_OnUnpauseGame(object sender, System.EventArgs e)
    {
        Hide();
    }

    private void GameHandler_OnPauseGame(object sender, System.EventArgs e)
    {
        Show();
    }

    private void Show()
    {
        foreach (Transform child in childrenTransformList)
        {
            child.gameObject.SetActive(true);
        }
    }

    private void Hide()
    {
        foreach (Transform child in childrenTransformList)
        {
            child.gameObject.SetActive(false);
        }
    }
}
