using TMPro;
using UnityEngine;

public class CountdownForStartUI : MonoBehaviour {
    [SerializeField]
    private TextMeshProUGUI countdownTimerText;

    private void Awake()
    {
        Hide();
    }

    private void Start()
    {
        GameHandler.Instance.OnStateChanged += GameHandler_OnStateChanged;
    }

    private void Update()
    {
        countdownTimerText.text = Mathf.Ceil(GameHandler.Instance.GetCountdownForStartTimer()).ToString();
    }

    private void GameHandler_OnStateChanged(object sender, System.EventArgs e)
    {
        if (GameHandler.Instance.IsCountdownForStartTimerActive())
        {
            Show();
        }
        else
        {
            Hide();
        }
    }

    private void Show()
    {
        countdownTimerText.gameObject.SetActive(true);
    }

    private void Hide()
    {
        countdownTimerText.gameObject.SetActive(false);
    }
}
