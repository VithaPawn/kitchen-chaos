using UnityEngine;
using UnityEngine.UI;

public class OptionsUI : MonoBehaviour {
    [SerializeField] private Button soundEffectsButton;
    [SerializeField] private Button musicButton;

    private void Awake()
    {
        soundEffectsButton.onClick.AddListener(() =>
        {

        });
        musicButton.onClick.AddListener(() =>
        {

        });
    }

    //private void Start()
    //{
    //    Hide();
    //}

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
