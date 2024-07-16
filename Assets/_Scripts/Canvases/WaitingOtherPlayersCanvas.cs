using UnityEngine;

public class WaitingOtherPlayersCanvas : MonoBehaviour
{
    private void Start()
    {
        GameHandler.Instance.OnStateChanged += GameHandler_OnStateChanged;
        GameHandler.Instance.OnLocalPlayerReadyChanged += Instance_OnLocalPlayerReadyChanged;

        Hide();
    }

    private void Instance_OnLocalPlayerReadyChanged()
    {
        if (GameHandler.Instance.IsLocalPlayerReady())
        {
            Show();
        }
    }

    private void GameHandler_OnStateChanged(object sender, System.EventArgs e)
    {
        if (GameHandler.Instance.IsCountdownForStartTimerActive())
        {
            Hide();
        }
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
