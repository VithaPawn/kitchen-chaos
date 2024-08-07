using UnityEngine;

public class StoveCounterSound : MonoBehaviour {
    [SerializeField] private StoveCounter stoveCounter;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        stoveCounter.OnStoveRunning += StoveCounter_OnStoveRunning;
    }

    private void StoveCounter_OnStoveRunning(StoveCounter.State state)
    {
        bool PlaySound = state != StoveCounter.State.Idle;
        if (PlaySound)
        {
            audioSource.Play();
        }
        else
        {
            audioSource.Stop();
        }
    }
}
