public class RunStats {
    
    public static RunStats Current = new RunStats();

    public int Score = 0;
    public float Duration = 0f;
    public float DamageTaken = 0;
    public int CargoShipsLooted = 0;
    public int CargoShipsDestroyed = 0;
    public int PoliceShipsDestroyed = 0;
    public int PowerupsCollected = 0;

    public static void ResetCurrent() {
        Current = new RunStats();
    }

}