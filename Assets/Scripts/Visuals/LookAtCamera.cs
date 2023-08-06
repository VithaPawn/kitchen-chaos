using UnityEngine;

public class LookAtCamera : MonoBehaviour {
    private void Update()
    {
        transform.forward = -Camera.main.transform.forward;
    }
}
