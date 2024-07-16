using System.Collections.Generic;
using Unity.Netcode;
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
            NetworkManager.Singleton.Shutdown();
            Loader.Load(Loader.Scene.MainMenuScene);
        });
        Hide();
    }

    private void Start()
    {
        GameHandler.Instance.OnLocalPauseGame += GameHandler_OnPauseGame;
        GameHandler.Instance.OnLocalUnpauseGame += GameHandler_OnUnpauseGame;
    }

    private void GameHandler_OnUnpauseGame()
    {
        Hide();
    }

    private void GameHandler_OnPauseGame()
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
