using UnityEngine;
using UnityEngine.UI;

public class ProgressBarUI : MonoBehaviour {
    [SerializeField]
    private Image imgBar;
    [SerializeField]
    private GameObject gameObjectHasProgress;

    private IHasProgress hasProgress;

    private void Start()
    {
        hasProgress = gameObjectHasProgress.GetComponent<IHasProgress>();

        hasProgress.OnProgressBarChanged += HasProgress_OnProgressBarChanged;

        imgBar.fillAmount = 0f;

        Hide();
    }

    private void HasProgress_OnProgressBarChanged(object sender, IHasProgress.OnProgressBarChangedEventArgs e)
    {
        imgBar.fillAmount = e.progressBarPercentage;

        if (e.progressBarPercentage == 0f || e.progressBarPercentage == 1f)
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
