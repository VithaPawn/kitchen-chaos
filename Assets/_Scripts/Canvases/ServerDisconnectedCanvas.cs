using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class ServerDisconnectedCanvas : MonoBehaviour {
    [SerializeField] private Button returnHomeButton;

    private void Start()
    {
        returnHomeButton.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.Shutdown();
            Loader.Load(Loader.Scene.MainMenuScene);
        });

        NetworkManager.Singleton.OnClientDisconnectCallback += NetworkManager_OnClientDisconnectedCallback;

        Hide();
    }


    private void OnDestroy()
    {
        NetworkManager.Singleton.OnClientDisconnectCallback -= NetworkManager_OnClientDisconnectedCallback;
    }

    private void NetworkManager_OnClientDisconnectedCallback(ulong clientId)
    {
        Debug.Log("clientId: " + clientId);
        Debug.Log("ServerClientId: " + NetworkManager.ServerClientId);
        if (clientId == NetworkManager.ServerClientId)
        {
            Debug.Log("ServerDisconnected!!!");
            Show();
        }
    }

    private void Hide()
    {
        foreach (Transform childTransform in transform)
        {
            childTransform.gameObject.SetActive(false);
        }
    }

    private void Show()
    {
        foreach (Transform childTransform in transform)
        {
            childTransform.gameObject.SetActive(true);
        }
    }
}
