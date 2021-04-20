
public class GameState {

    public static GameState shared = new GameState();

    public bool paused = false;

    public static void Reset() {
        shared = new GameState();
    }

    public GameState() {
        paused = false;
    }

}
