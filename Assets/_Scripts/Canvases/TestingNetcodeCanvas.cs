using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class TestingNetcodeCanvas : MonoBehaviour
{
    [SerializeField] Button hostBtn;
    [SerializeField] Button clientBtn;

    private void Start()
    {
        Show();

        hostBtn.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartHost();
            Hide();
        });
        clientBtn.onClick.AddListener(() => { 
            NetworkManager.Singleton.StartClient();
            Hide();
        });
    }

    private void Show()
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
}
