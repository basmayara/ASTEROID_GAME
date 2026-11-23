using TMPro;
using UnityEngine;

// The VIEW: it only DISPLAYS things (texts and panels). No game rule here.
// So if we change the look of the UI, the game logic is not touched (SOLID: S).
public class HudView : MonoBehaviour
{
    [Header("Texts")]
    [SerializeField] TMP_Text scoreText;
    [SerializeField] TMP_Text highScoreText;
    [SerializeField] TMP_Text livesText;
    [SerializeField] TMP_Text waveText;
    [SerializeField] TMP_Text finalScoreText; // on the Game Over panel

    [Header("Panels")]
    [SerializeField] GameObject menuPanel;
    [SerializeField] GameObject pausePanel;
    [SerializeField] GameObject gameOverPanel;

    // $"..." = a string with values inside { }.
    public void ShowScore(int score, int highScore)
    {
        scoreText.text = $"Score: {score}";
        highScoreText.text = $"Best: {highScore}";
    }

    public void ShowLives(int lives) { livesText.text = $"Lives: {lives}"; }

    public void ShowWave(int wave) { waveText.text = $"Wave {wave}"; }

    public void ShowFinalScore(int score, int highScore)
    {
        finalScoreText.text = $"Score: {score}\nBest: {highScore}"; // \n = new line
    }

    // Shows ONLY the panel that matches the state, hides the others.
    public void ShowState(GameState state)
    {
        menuPanel.SetActive(state == GameState.Menu);
        pausePanel.SetActive(state == GameState.Paused);
        gameOverPanel.SetActive(state == GameState.GameOver);
    }
}
