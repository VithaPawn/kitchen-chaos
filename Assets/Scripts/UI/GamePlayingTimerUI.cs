using UnityEngine;
using UnityEngine.UI;

public class GamePlayingTimerUI : MonoBehaviour {

    [SerializeField] private Image gamePlayingTimer;

    private void Update()
    {
        gamePlayingTimer.fillAmount = GameHandler.Instance.GetPlayingTimerNormalized();
    }
}
