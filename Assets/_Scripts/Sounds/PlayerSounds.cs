using UnityEngine;

public class PlayerSounds : MonoBehaviour {

    private readonly float volume = .8f;
    private readonly float footstepTimerMax = .1f;
    private float footstepTimer;
    private Player player;

    private void Awake()
    {
        player = GetComponent<Player>();
    }

    private void Update()
    {
        if (player.GetIsWalking())
        {
            footstepTimer -= Time.deltaTime;
            if (footstepTimer < 0)
            {
                footstepTimer = footstepTimerMax;
                SoundManager.Instance.PlayFootstepSounds(transform.position, volume);
            }
        }
    }
}
