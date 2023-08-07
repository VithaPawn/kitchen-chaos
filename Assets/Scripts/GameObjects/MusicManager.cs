using UnityEngine;

public class MusicManager : MonoBehaviour {

    private const string PLAYER_PREFS_MUSIC_VOLUME = "MusicVolume";
    public static MusicManager Instance { get; private set; }

    private AudioSource audioSource;
    private float generalVolume;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There are more than one Music Manager at the same time.");
        }
        else
        {
            Instance = this;
        }
        audioSource = GetComponent<AudioSource>();
        generalVolume = PlayerPrefs.GetFloat(PLAYER_PREFS_MUSIC_VOLUME, 0.5f);
    }

    public void ChangeVolume()
    {
        generalVolume += .1f;
        if (generalVolume > 1f)
        {
            generalVolume = 0f;
        }
        audioSource.volume = generalVolume;
        PlayerPrefs.SetFloat(PLAYER_PREFS_MUSIC_VOLUME, generalVolume);
        PlayerPrefs.Save();
    }

    public float GetVolumeLevel()
    {
        return generalVolume;
    }
}
