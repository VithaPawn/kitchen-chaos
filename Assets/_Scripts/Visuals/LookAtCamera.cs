using UnityEngine;

public class LookAtCamera : MonoBehaviour {

    private enum CameraMode {
        CameraForward,
        CameraForwardInverted,
    }

    [SerializeField] private CameraMode cameraMode = CameraMode.CameraForward;
    private void Update()
    {
        switch (cameraMode)
        {
            case CameraMode.CameraForward:
                transform.forward = Camera.main.transform.forward;
                break;
            case CameraMode.CameraForwardInverted:
                transform.forward = -Camera.main.transform.forward;
                break;
        }

    }
}
