using System.Collections;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    [SerializeField] Ship ship;                 // drag "ship"
    [SerializeField] AsteroidSpawner spawner;   // drag "AsteroidSpawner"
    [SerializeField] HudView hud;               // drag "Canvas"
    [SerializeField] MonoBehaviour inputSource; // drag "Input"

    [Header("Rules")]
    [SerializeField] int startingLives = 3;
    [SerializeField] float respawnDelay = 1.5f;   // seconds before the ship comes back
    [SerializeField] float nextWaveDelay = 1.5f;  // seconds between two waves

    IInputProvider input;
    ScoreManager score;
    GameState state;
    int lives;
    int wave;

    void Awake()
    {
        input = (IInputProvider)inputSource;
        // Dependency injection: we GIVE the storage to the ScoreManager.
        score = new ScoreManager(new PlayerPrefsScoreStorage());
    }

    // We listen to the events of the others (OBSERVER).
    void OnEnable()
    {
        Asteroid.Destroyed += OnAsteroidDestroyed;
        ship.Destroyed += OnShipDestroyed;
        spawner.WaveCleared += OnWaveCleared;
        score.Changed += hud.ShowScore; // when the score changes, update the text
    }

    // ...and we stop listening when we are disabled.
    void OnDisable()
    {
        Asteroid.Destroyed -= OnAsteroidDestroyed;
        ship.Destroyed -= OnShipDestroyed;
        spawner.WaveCleared -= OnWaveCleared;
        score.Changed -= hud.ShowScore;
    }

    void Start()
    {
        ReturnToMenu(); // the game begins on the menu
    }

    // Every frame: react to keys depending on the current state.
    void Update()
    {
        switch (state)
        {
            case GameState.Menu:
            case GameState.GameOver:
                if (input.ConfirmPressed) StartGame(); // Enter
                break;
            case GameState.Playing:
                if (input.PausePressed) Pause();
                break;
            case GameState.Paused:
                if (input.PausePressed) Resume();
                break;
        }
    }

   

    public void StartGame()
    {
        StopAllCoroutines();   // cancel pending respawn / next wave timers
        Time.timeScale = 1f;   // 1 = normal speed, 0 = frozen

        score.Reset();
        lives = startingLives;
        wave = 1;

        spawner.ClearAll();
        ship.Respawn();
        spawner.SpawnWave(wave);

        hud.ShowLives(lives);
        hud.ShowWave(wave);
        SetState(GameState.Playing);
    }

    public void ReturnToMenu()
    {
        StopAllCoroutines();
        Time.timeScale = 1f;

        score.SaveIfBest();
        score.Reset();
        spawner.ClearAll();
        ship.gameObject.SetActive(false); // hide the ship

        SetState(GameState.Menu);
    }

    public void Resume()
    {
        Time.timeScale = 1f;
        SetState(GameState.Playing);
    }

    void Pause()
    {
        Time.timeScale = 0f; // everything that uses Time.deltaTime stops
        SetState(GameState.Paused);
    }

 

    void OnAsteroidDestroyed(Asteroid asteroid)
    {
        score.Add(asteroid.Points);
    }

    void OnShipDestroyed()
    {
        lives--;
        hud.ShowLives(lives);

        if (lives <= 0) GameOver();
        else StartCoroutine(RespawnRoutine()); // come back after a short delay
    }

    void OnWaveCleared()
    {
        StartCoroutine(NextWaveRoutine());
    }

    // A coroutine can "wait" without freezing the game: yield return = pause here, continue later.
    IEnumerator RespawnRoutine()
    {
        yield return new WaitForSeconds(respawnDelay);
        ship.Respawn();
    }

    IEnumerator NextWaveRoutine()
    {
        yield return new WaitForSeconds(nextWaveDelay);
        wave++;
        hud.ShowWave(wave);
        spawner.SpawnWave(wave);
    }

    void GameOver()
    {
        score.SaveIfBest();
        hud.ShowFinalScore(score.Score, score.HighScore);
        ship.gameObject.SetActive(false);
        SetState(GameState.GameOver);
    }

    // One single place changes the state AND updates the panels, so they never disagree.
    void SetState(GameState newState)
    {
        state = newState;
        hud.ShowState(newState);
    }
}
