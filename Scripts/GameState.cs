// The 4 states of the game. The GameManager is always in exactly one of them.
// An enum is just a named list of values: it is easier to read than numbers (0, 1, 2, 3).
public enum GameState
{
    Menu,      // main menu is displayed 
    Playing,   // the game is running
    Paused,    // the game is frozen (Time.timeScale = 0) 
    GameOver   // the player has no more lives 
} 
