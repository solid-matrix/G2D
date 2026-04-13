namespace G2D;

public partial class G2D
{
    private static G2D? _instance;

    public static Graphics Graphics => _instance?._graphics ?? throw new Exception("G2D: not initialized");

    public static Assets Assets => _instance?._assets ?? throw new Exception("G2D: not initialized");

    public static void Launch<T>(T game) where T : IGame
    {
        var config = new Config();
        game.Config(config);

        _instance = new G2D(config, game);
        _instance.Run();
        _instance.Cleanup();
        _instance = null!;
    }

    public static void Exit()
    {
        _instance?._cts.Cancel();
    }
}