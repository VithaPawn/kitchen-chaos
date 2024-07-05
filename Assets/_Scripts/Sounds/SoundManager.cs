using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

    private const string PLAYER_PREFS_SOUND_EFFECTS_VOLUME = "SoundEffectsVolume";
    public static SoundManager Instance { get; private set; }

    [SerializeField] private AudioClipRefsSO audioClipRefsSO;

    private float generalVolume;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There are more than one Sound Manager at the same time.");
        }
        else
        {
            Instance = this;
        }
        generalVolume = PlayerPrefs.GetFloat(PLAYER_PREFS_SOUND_EFFECTS_VOLUME, 1f);
    }

    private void Start()
    {
        DeliveryManager.Instance.OnDeliverySuccess += DeliveryManager_OnDeliverySuccess;
        DeliveryManager.Instance.OnDeliveryFail += DeliveryManager_OnDeliveryFail;
        CuttingCounter.OnAnyCut += CuttingCounter_OnAnyCut;
        //Player.Instance.OnPickupSomething += BaseCounter_OnPickupSomething;
        BaseCounter.OnDropSomething += BaseCounter_OnDropSomething;
        TrashCounter.OnAnyObjectTrashed += TrashCounter_OnAnyObjectTrashed;
    }

    private void TrashCounter_OnAnyObjectTrashed(BaseCounter baseCounter)
    {
        PlaySound(audioClipRefsSO.trash, baseCounter.transform.position);
    }

    private void BaseCounter_OnDropSomething(object sender, System.EventArgs e)
    {
        BaseCounter baseCounter = sender as BaseCounter;
        PlaySound(audioClipRefsSO.objectDrop, baseCounter.transform.position);
    }

    //private void BaseCounter_OnPickupSomething(object sender, System.EventArgs e)
    //{
    //    PlaySound(audioClipRefsSO.objectPickup, Player.Instance.transform.position);
    //}

    private void CuttingCounter_OnAnyCut(BaseCounter baseCounter)
    {
        PlaySound(audioClipRefsSO.chop, baseCounter.transform.position);
    }

    private void DeliveryManager_OnDeliveryFail(object sender, System.EventArgs e)
    {
        PlaySound(audioClipRefsSO.deliveryFail, DeliveryCounter.Instance.transform.position);
    }

    private void DeliveryManager_OnDeliverySuccess(object sender, System.EventArgs e)
    {
        PlaySound(audioClipRefsSO.deliverySuccess, DeliveryCounter.Instance.transform.position);
    }

    private void PlaySound(List<AudioClip> audioClipList, Vector3 position, float volume = 1f)
    {
        PlaySound(audioClipList[Random.Range(0, audioClipList.Count)], position, volume);
    }

    private void PlaySound(AudioClip audioClip, Vector3 position, float volumeMultiplier = 1f)
    {
        AudioSource.PlayClipAtPoint(audioClip, position, volumeMultiplier * generalVolume);
    }

    public void PlayFootstepSounds(Vector3 position, float volume = 1f)
    {
        PlaySound(audioClipRefsSO.footstep, position, volume);
    }

    public void PlayNumberPopup()
    {
        PlaySound(audioClipRefsSO.warning, Vector3.zero);
    }

    public void ChangeVolume()
    {
        generalVolume += .1f;
        if (generalVolume > 1f)
        {
            generalVolume = 0f;
        }
        PlayerPrefs.SetFloat(PLAYER_PREFS_SOUND_EFFECTS_VOLUME, generalVolume);
        PlayerPrefs.Save();
    }

    public float GetVolumeLevel()
    {
        return generalVolume;
    }
}
