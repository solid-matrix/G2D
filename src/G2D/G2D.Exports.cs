namespace G2D;

public partial class G2D
{
    private static G2D? _instance;

    public static KeyboardManager Keyboard => _instance?._keyboardManager ?? throw new Exception("G2D: KeyboardManager not initialized");

    public static MouseManager Mouse => _instance?._mouseManager ?? throw new Exception("G2D: MouseManager not initialized");

    public static AssetsManager Assets => _instance?._assetsManager ?? throw new Exception("G2D: AssetsManager not initialized");

    public static EventsManager Events => _instance?._eventsManager ?? throw new Exception("G2D: EventsManager not initialized");

    public static Graphics Graphics => _instance?._graphics ?? throw new Exception("G2D: Graphics not initialized");

    public static void Launch<T>(T game) where T : IGame
    {
        var config = new Config();
        game.Config(config);

        _instance = new G2D(config, game);
        _instance.Run();
        _instance.Cleanup();
        _instance = null!;
    }

    public static void Exit(bool sure = true)
    {
        if (sure) _instance?._cts.Cancel();
    }
}