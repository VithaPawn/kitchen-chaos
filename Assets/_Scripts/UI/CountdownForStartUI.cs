using TMPro;
using UnityEngine;

public class CountdownForStartUI : MonoBehaviour {
    private const string NUMBER_POPUP = "NumberPopup";

    [SerializeField]
    private TextMeshProUGUI countdownTimerText;

    private Animator animator;
    private int previousCountdownNumber;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        Hide();
    }

    private void Start()
    {
        GameHandler.Instance.OnStateChanged += GameHandler_OnStateChanged;
    }

    private void Update()
    {
        int countdownNumber = Mathf.CeilToInt(GameHandler.Instance.GetCountdownForStartTimer());
        countdownTimerText.text = countdownNumber.ToString();

        if (previousCountdownNumber != countdownNumber)
        {
            previousCountdownNumber = countdownNumber;
            animator.SetTrigger(NUMBER_POPUP);
            SoundManager.Instance.PlayNumberPopup();
        }
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
