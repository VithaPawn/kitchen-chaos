using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeliveryResultUI : MonoBehaviour {

    private const string POPUP = "Popup";

    [SerializeField] private Image background;
    [SerializeField] private TextMeshProUGUI resultText;
    [SerializeField] private Image resultIcon;
    [SerializeField] private Color successfulColor;
    [SerializeField] private Color failedColor;
    [SerializeField] private Sprite successfulIcon;
    [SerializeField] private Sprite failedIcon;
    [SerializeField] private List<Transform> childrenTransformList;

    private Animator animator;

    private string successfulText = "YUMMY!\nHOW A NICE \nRECIPE";
    private string failedText = "NOPE!\nNO ONE \nNEEDS THIS";

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        DeliveryManager.Instance.OnDeliverySuccess += DeliveryManager_OnDeliverySuccess;
        DeliveryManager.Instance.OnDeliveryFail += DeliveryManager_OnDeliveryFail;

        Hide();
    }

    private void DeliveryManager_OnDeliverySuccess(object sender, System.EventArgs e)
    {
        Show();
        animator.SetTrigger(POPUP);
        background.color = successfulColor;
        resultText.text = successfulText;
        resultIcon.sprite = successfulIcon;
    }

    private void DeliveryManager_OnDeliveryFail(object sender, System.EventArgs e)
    {
        Show();
        animator.SetTrigger(POPUP);
        background.color = failedColor;
        resultText.text = failedText;
        resultIcon.sprite = failedIcon;
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
