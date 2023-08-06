using UnityEngine;

public class PlayerSounds : MonoBehaviour {

    private float footstepTimerMax = .1f;
    private float footstepTimer;
    private float volume = .8f;

    private void Update()
    {
        if (Player.Instance.GetIsWalking())
        {
            footstepTimer -= Time.deltaTime;
            if (footstepTimer < 0)
            {
                footstepTimer = footstepTimerMax;
                SoundManager.Instance.PlayFootstepSounds(Player.Instance.transform.position, volume);
            }
        }
    }
}
