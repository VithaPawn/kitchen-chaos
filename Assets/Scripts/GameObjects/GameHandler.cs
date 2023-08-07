using System;
using UnityEngine;

public class GameHandler : MonoBehaviour {

    public static GameHandler Instance { get; private set; }

    private const float COUNTDOWN_FOR_START_TIMER_MAX = 3f;
    private const float GAME_PLAYING_TIMER_MAX = 60f;

    public enum State {
        WaitingForStart,
        CountdownForStart,
        GamePlaying,
        GameOver
    }

    private State state;
    private float countdownForStartTimer = COUNTDOWN_FOR_START_TIMER_MAX;
    private float gamePlayingTimer = GAME_PLAYING_TIMER_MAX;
    private bool isPauseGame = false;

    public event EventHandler OnStateChanged;
    public event EventHandler OnPauseGame;
    public event EventHandler OnUnpauseGame;


    private void Awake()
    {
        state = State.WaitingForStart;
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

    private void GameInput_OnInteractAction(object sender, EventArgs e)
    {
        // When press interact key in tutorial UI, state will switch to WaitingForStart
        if (state == State.WaitingForStart)
        {
            state = State.CountdownForStart;
            OnStateChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    private void GameInput_OnPauseGame(object sender, EventArgs e)
    {
        TogglePauseGame();
    }

    private void Update()
    {
        switch (state)
        {
            case State.WaitingForStart:
                break;
            case State.CountdownForStart:
                countdownForStartTimer -= Time.deltaTime;
                if (countdownForStartTimer < 0)
                {
                    countdownForStartTimer = COUNTDOWN_FOR_START_TIMER_MAX;
                    state = State.GamePlaying;
                    OnStateChanged?.Invoke(this, EventArgs.Empty);
                }
                break;
            case State.GamePlaying:
                gamePlayingTimer -= Time.deltaTime;
                if (gamePlayingTimer < 0)
                {
                    gamePlayingTimer = GAME_PLAYING_TIMER_MAX;
                    state = State.GameOver;
                    OnStateChanged?.Invoke(this, EventArgs.Empty);
                }
                break;
            case State.GameOver:
                break;
        }
    }

    public bool IsGamePlaying()
    {
        return state == State.GamePlaying;
    }

    public bool IsCountdownForStartTimerActive()
    {
        return state == State.CountdownForStart;
    }

    public bool IsGameOver()
    {
        return state == State.GameOver;
    }

    public float GetCountdownForStartTimer()
    {
        return countdownForStartTimer;
    }

    public float GetPlayingTimerNormalized()
    {
        if (IsGameOver())
        {
            return 1f;
        }
        else
        {
            return (1 - (gamePlayingTimer / GAME_PLAYING_TIMER_MAX));
        }
    }

    public void TogglePauseGame()
    {
        isPauseGame = !isPauseGame;
        if (isPauseGame)
        {
            Time.timeScale = 0f;
            OnPauseGame?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            Time.timeScale = 1f;
            OnUnpauseGame?.Invoke(this, EventArgs.Empty);
        }
    }
}
