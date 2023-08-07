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
        closeButton.onClick.AddListener(() =>
        {
            Hide();
        });
    }

    private void Start()
    {
        UpdateSoundEffectsButtonUI();
        UpdateMusicButtonUI();
        GameHandler.Instance.OnUnpauseGame += GameHandler_OnUnpauseGame;
        Hide();
    }

    private void GameHandler_OnUnpauseGame(object sender, System.EventArgs e)
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

    public void Show()
    {
        gameObject.SetActive(true);

    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
