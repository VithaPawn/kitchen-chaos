using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI deliveryRecipesTotalText;
    [SerializeField] private Button returnHomeButton;
    [SerializeField] private List<Transform> childList;
    private void Awake()
    {
        returnHomeButton.onClick.AddListener(() =>
        {
            Loader.Load(Loader.Scene.MainMenuScene);
        });

        Hide();
    }

    private void Start()
    {
        GameHandler.Instance.OnStateChanged += Instance_OnStateChanged;
    }

    private void Instance_OnStateChanged(object sender, System.EventArgs e)
    {
        if (GameHandler.Instance.IsGameOver())
        {
            deliveryRecipesTotalText.text = DeliveryManager.Instance.GetDeliveredRecipesAmount().ToString();
            Show();
        }
        else
        {
            Hide();
        }
    }

    private void Show()
    {
        foreach (Transform child in childList)
        {
            child.gameObject.SetActive(true);
        }
    }

    private void Hide()
    {
        foreach (Transform child in childList)
        {
            child.gameObject.SetActive(false);
        }
    }
}
