[System.Flags]
public enum GAME_STATE { 
    TITLE = 1 << 1, 
    MAIN_MENU = 1 << 2, 
    GAME_ACTIVE = 1 << 3, 
    GAME_PAUSED = 1 << 4, 
    GAME_WON = 1 << 5,
    GAME_LOST = 1 << 6
} 
