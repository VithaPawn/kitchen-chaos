using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionsUI : MonoBehaviour {

    public static OptionsUI Instance { get; private set; }

    // Sound and music options
    [SerializeField] private Button soundEffectsButton;
    [SerializeField] private Button musicButton;
    [SerializeField] private TextMeshProUGUI soundEffectsButtonTitle;
    [SerializeField] private TextMeshProUGUI musicButtonTitle;
    // Close option
    [SerializeField] private Button closeButton;
    // Key binding options
    [SerializeField] private Button moveUpButton;
    [SerializeField] private Button moveDownButton;
    [SerializeField] private Button moveLeftButton;
    [SerializeField] private Button moveRightButton;
    [SerializeField] private Button interactButton;
    [SerializeField] private Button interactAlternateButton;
    [SerializeField] private Button pauseButton;
    [SerializeField] private TextMeshProUGUI moveUpButtonTitle;
    [SerializeField] private TextMeshProUGUI moveDownButtonTitle;
    [SerializeField] private TextMeshProUGUI moveLeftButtonTitle;
    [SerializeField] private TextMeshProUGUI moveRightButtonTitle;
    [SerializeField] private TextMeshProUGUI interactButtonTitle;
    [SerializeField] private TextMeshProUGUI interactAlternateButtonTitle;
    [SerializeField] private TextMeshProUGUI pauseButtonTitle;
    // Rebind Notification UI
    [SerializeField] private Transform rebindNotificationTransform;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There are more than one Option UI at the same time.");
        }
        else
        {
            Instance = this;
        }
        // Hide rebind notification UI
        HideRebindNotificationUI();

        // Set on click event for music and sound effects button
        soundEffectsButton.onClick.AddListener(() =>
        {
            SoundManager.Instance.ChangeVolume();
            UpdateSoundEffectsButtonUI();
        });
        musicButton.onClick.AddListener(() =>
        {
            MusicManager.Instance.ChangeVolume();
            UpdateMusicButtonUI();
        });

        // Set on click event for close button
        closeButton.onClick.AddListener(() =>
        {
            Hide();
        });

        // Set OnClick event for key binding button
        moveUpButton.onClick.AddListener(() =>
        {
            RebindBinding(GameInput.Binding.Move_Up);
        });
        moveDownButton.onClick.AddListener(() =>
        {
            RebindBinding(GameInput.Binding.Move_Down);
        });
        moveLeftButton.onClick.AddListener(() =>
        {
            RebindBinding(GameInput.Binding.Move_Left);
        });
        moveRightButton.onClick.AddListener(() =>
        {
            RebindBinding(GameInput.Binding.Move_Right);
        });
        interactButton.onClick.AddListener(() =>
        {
            RebindBinding(GameInput.Binding.Interact);
        });
        interactAlternateButton.onClick.AddListener(() =>
        {
            RebindBinding(GameInput.Binding.Interact_Alternate);
        });
        pauseButton.onClick.AddListener(() =>
        {
            RebindBinding(GameInput.Binding.Pause);
        });
    }

    private void Start()
    {
        UpdateSoundEffectsButtonUI();
        UpdateMusicButtonUI();
        UpdateBindingsUI();
        GameHandler.Instance.OnLocalUnpauseGame += GameHandler_OnUnpauseGame;
        Hide();
    }

    private void GameHandler_OnUnpauseGame()
    {
        Hide();
    }

    private void UpdateSoundEffectsButtonUI()
    {
        soundEffectsButtonTitle.text = "Sound Effects: " + Mathf.Round(SoundManager.Instance.GetVolumeLevel() * 10f);
    }

    private void UpdateMusicButtonUI()
    {
        musicButtonTitle.text = "Music: " + Mathf.Round(MusicManager.Instance.GetVolumeLevel() * 10f);
    }

    private void UpdateBindingsUI()
    {
        moveUpButtonTitle.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Up);
        moveDownButtonTitle.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Down);
        moveLeftButtonTitle.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Left);
        moveRightButtonTitle.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Right);
        interactButtonTitle.text = GameInput.Instance.GetBindingText(GameInput.Binding.Interact);
        interactAlternateButtonTitle.text = GameInput.Instance.GetBindingText(GameInput.Binding.Interact_Alternate);
        pauseButtonTitle.text = GameInput.Instance.GetBindingText(GameInput.Binding.Pause);
    }

    public void Show()
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

    private void ShowRebindNotificationUI()
    {
        rebindNotificationTransform.gameObject.SetActive(true);
    }

    private void HideRebindNotificationUI()
    {
        rebindNotificationTransform.gameObject.SetActive(false);
    }

    private void RebindBinding(GameInput.Binding binding)
    {
        ShowRebindNotificationUI();
        GameInput.Instance.RebindBinding(binding, () =>
        {
            HideRebindNotificationUI();
            UpdateBindingsUI();
        });
    }
}
