using UnityEngine;

public class PauseMultiplayerUI : MonoBehaviour {
    private void Start()
    {
        Hide();

        GameHandler.Instance.OnMultiplayerPauseGame += GameHandler_OnMultiplayerPauseGame;
        GameHandler.Instance.OnMultiplayerUnpauseGame += GameHandler_OnMultiplayerUnpauseGame;
        GameHandler.Instance.OnLocalPauseGame += GameHandler_OnLocalPauseGame;
        GameHandler.Instance.OnLocalUnpauseGame += Instance_OnLocalUnpauseGame;
    }
    private void OnDestroy()
    {
        GameHandler.Instance.OnMultiplayerPauseGame -= GameHandler_OnMultiplayerPauseGame;
        GameHandler.Instance.OnMultiplayerUnpauseGame -= GameHandler_OnMultiplayerUnpauseGame;

    }

    private void Instance_OnLocalUnpauseGame()
    {
        if (GameHandler.Instance.IsMultiplayerPauseGame())
        {
            Show();
        }
    }

    private void GameHandler_OnLocalPauseGame()
    {
        Hide();
    }

    private void GameHandler_OnMultiplayerPauseGame()
    {
        if (!GameHandler.Instance.IsLocalPauseGame())
        {
            Show();
        }
    }

    private void GameHandler_OnMultiplayerUnpauseGame()
    {
        Hide();
    }

    private void Show()
    {
        foreach (Transform childTransform in transform)
        {
            childTransform.gameObject.SetActive(true);
        }
    }

    private void Hide()
    {
        foreach (Transform childTransform in transform)
        {
            childTransform.gameObject.SetActive(false);
        }
    }
}
