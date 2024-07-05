using UnityEngine;
using UnityEngine.UI;

public class ProgressBarUI : MonoBehaviour {
    [SerializeField]
    private Image imgBar;
    [SerializeField]
    private GameObject gameObjectHasProgress;

    private void Start()
    {
        IHasProgress hasProgress = gameObjectHasProgress.GetComponent<IHasProgress>();

        hasProgress.OnProgressBarChanged += HasProgress_OnProgressBarChanged;

        imgBar.fillAmount = 0f;

        Hide();
    }

    private void HasProgress_OnProgressBarChanged(float progressBarPercentage)
    {
        imgBar.fillAmount = progressBarPercentage;

        if (progressBarPercentage <= 0f || progressBarPercentage >= 1f)
        {
            Hide();
        }
        else
        {
            Show();
        }
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
