using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialUI : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI moveUpKeyText;
    [SerializeField] private TextMeshProUGUI moveDownKeyText;
    [SerializeField] private TextMeshProUGUI moveLeftKeyText;
    [SerializeField] private TextMeshProUGUI moveRightKeyText;
    [SerializeField] private TextMeshProUGUI interactKeyText;
    [SerializeField] private TextMeshProUGUI interactAlternateKeyText;
    [SerializeField] private TextMeshProUGUI pauseKeyText;
    [SerializeField] private List<Transform> childrenTranformList;

    private void Start()
    {
        GameInput.Instance.OnRebindBinding += GameInput_OnRebindBinding;
        GameHandler.Instance.OnStateChanged += GameHandler_OnStateChanged;

        UpdateVisual();

        Hide();
    }

    private void GameHandler_OnStateChanged(object sender, System.EventArgs e)
    {
        if (GameHandler.Instance.IsCountdownForStartTimerActive())
        {
            Hide();
        }
    }

    private void GameInput_OnRebindBinding(object sender, System.EventArgs e)
    {
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        moveUpKeyText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Up);
        moveDownKeyText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Down);
        moveLeftKeyText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Left);
        moveRightKeyText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Right);
        interactKeyText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Interact);
        interactAlternateKeyText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Interact_Alternate);
        pauseKeyText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Pause);
    }

    private void Show()
    {
        foreach (Transform child in childrenTranformList)
        {
            child.gameObject.SetActive(true);
        }
    }

    private void Hide()
    {
        foreach (Transform child in childrenTranformList)
        {
            child.gameObject.SetActive(false);
        }
    }
}
