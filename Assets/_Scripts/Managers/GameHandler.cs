using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class GameHandler : NetworkBehaviour {

    public static GameHandler Instance { get; private set; }

    private const float COUNTDOWN_FOR_START_TIMER_MAX = 3f;
    private const float GAME_PLAYING_TIMER_MAX = 180f;

    public enum State {
        WaitingForStart,
        CountdownForStart,
        GamePlaying,
        GameOver
    }

    private NetworkVariable<State> state = new NetworkVariable<State>(State.WaitingForStart);
    private bool isLocalPlayerReady = false;
    private bool isLocalPauseGame = false;
    private NetworkVariable<bool> isMultiplayerPauseGame = new NetworkVariable<bool>(false);
    private Dictionary<ulong, bool> playerReadyDictionary;
    private Dictionary<ulong, bool> playerPauseDictionary;
    private NetworkVariable<float> countdownForStartTimer = new NetworkVariable<float>(COUNTDOWN_FOR_START_TIMER_MAX);
    private NetworkVariable<float> gamePlayingTimer = new NetworkVariable<float>(GAME_PLAYING_TIMER_MAX);
    private bool clientDisconnectedState = false;

    public event EventHandler OnStateChanged;
    public event Action OnLocalPauseGame;
    public event Action OnLocalUnpauseGame;
    public event Action OnMultiplayerPauseGame;
    public event Action OnMultiplayerUnpauseGame;
    public event Action OnLocalPlayerReadyChanged;

    private void Awake()
    {
        playerReadyDictionary = new Dictionary<ulong, bool>();
        playerPauseDictionary = new Dictionary<ulong, bool>();
        if (Instance != null)
        {
            Debug.LogError("There are more than one Game Handler Instance at the same time.");
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        GameInput.Instance.OnPauseGame += GameInput_OnPauseGame;
        GameInput.Instance.OnInteractAction += GameInput_OnInteractAction;
    }

    public override void OnDestroy()
    {
        GameInput.Instance.OnPauseGame -= GameInput_OnPauseGame;
        GameInput.Instance.OnInteractAction -= GameInput_OnInteractAction;
        base.OnDestroy();
    }

    public override void OnNetworkSpawn()
    {
        state.OnValueChanged += State_OnValueChanged;
        isMultiplayerPauseGame.OnValueChanged += isMultiplayerPauseGame_OnValueChanged;
        if (IsServer)
        {
            NetworkManager.Singleton.OnClientDisconnectCallback += NetworkManager_OnClientDisconnectCallback;
        }
    }

    private void NetworkManager_OnClientDisconnectCallback(ulong clientId)
    {
        clientDisconnectedState = true;
    }

    private void State_OnValueChanged(State previousValue, State newValue)
    {
        OnStateChanged?.Invoke(this, EventArgs.Empty);
    }

    private void isMultiplayerPauseGame_OnValueChanged(bool previousValue, bool newValue)
    {
        if (isMultiplayerPauseGame.Value)
        {
            Time.timeScale = 0f;
            OnMultiplayerPauseGame?.Invoke();
        }
        else
        {
            Time.timeScale = 1f;
            OnMultiplayerUnpauseGame?.Invoke();
        }
    }

    private void GameInput_OnInteractAction(object sender, EventArgs e)
    {
        // When press interact key in tutorial UI, state will switch to WaitingForStart
        if (state.Value == State.WaitingForStart)
        {
            isLocalPlayerReady = true;
            OnLocalPlayerReadyChanged?.Invoke();
            SetPlayerReadyServerRpc();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void SetPlayerReadyServerRpc(ServerRpcParams serverRpcParams = default)
    {
        playerReadyDictionary[serverRpcParams.Receive.SenderClientId] = true;

        bool isAllPlayerReady = true;
        foreach (ulong clientId in NetworkManager.Singleton.ConnectedClientsIds)
        {
            if (!playerReadyDictionary.ContainsKey(clientId) || !playerReadyDictionary[clientId])
            {
                isAllPlayerReady = false;
                break;
            }
        }

        if (isAllPlayerReady)
        {
            state.Value = State.CountdownForStart;
        }
    }

    private void GameInput_OnPauseGame(object sender, EventArgs e)
    {
        TogglePauseGame();
    }

    private void Update()
    {
        if (!IsServer) return;

        switch (state.Value)
        {
            case State.WaitingForStart:
                break;
            case State.CountdownForStart:
                countdownForStartTimer.Value -= Time.deltaTime;
                if (countdownForStartTimer.Value < 0)
                {
                    countdownForStartTimer.Value = COUNTDOWN_FOR_START_TIMER_MAX;
                    state.Value = State.GamePlaying;
                }
                break;
            case State.GamePlaying:
                gamePlayingTimer.Value -= Time.deltaTime;
                if (gamePlayingTimer.Value < 0)
                {
                    gamePlayingTimer.Value = GAME_PLAYING_TIMER_MAX;
                    state.Value = State.GameOver;
                }
                break;
            case State.GameOver:
                break;
        }
    }

    private void LateUpdate()
    {
        if (clientDisconnectedState)
        {
            clientDisconnectedState = false;
            CheckPauseMultiplayer();
        }
    }

    public bool IsWaitingForStart() { return state.Value == State.WaitingForStart; }

    public bool IsGamePlaying() { return state.Value == State.GamePlaying; }

    public bool IsCountdownForStartTimerActive() { return state.Value == State.CountdownForStart; }

    public bool IsGameOver() { return state.Value == State.GameOver; }

    public bool IsLocalPauseGame() { return isLocalPauseGame; }

    public bool IsMultiplayerPauseGame() { return isMultiplayerPauseGame.Value; }

    public bool IsLocalPlayerReady() { return isLocalPlayerReady; }

    public float GetCountdownForStartTimer() { return countdownForStartTimer.Value; }

    public float GetPlayingTimerNormalized()
    {
        if (IsGameOver())
        {
            return 1f;
        }
        else
        {
            return (1 - (gamePlayingTimer.Value / GAME_PLAYING_TIMER_MAX));
        }
    }

    public void TogglePauseGame()
    {
        isLocalPauseGame = !isLocalPauseGame;
        if (isLocalPauseGame)
        {
            OnLocalPauseGame?.Invoke();
            PauseGameServerRpc();
        }
        else
        {
            OnLocalUnpauseGame?.Invoke();
            UnpauseGameServerRpc();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void PauseGameServerRpc(ServerRpcParams serverRpcParams = default)
    {
        playerPauseDictionary[serverRpcParams.Receive.SenderClientId] = true;
        CheckPauseMultiplayer();
    }

    [ServerRpc(RequireOwnership = false)]
    private void UnpauseGameServerRpc(ServerRpcParams serverRpcParams = default)
    {
        playerPauseDictionary[serverRpcParams.Receive.SenderClientId] = false;
        CheckPauseMultiplayer();
    }

    private void CheckPauseMultiplayer()
    {
        foreach (ulong clientId in NetworkManager.Singleton.ConnectedClientsIds)
        {
            if (playerPauseDictionary.ContainsKey(clientId) && playerPauseDictionary[clientId])
            {
                //If any player pause game
                isMultiplayerPauseGame.Value = true;
                return;
            }
        }
        //If all player unpause game
        isMultiplayerPauseGame.Value = false;
    }
}
