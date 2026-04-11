namespace G2D.Demo;

public partial class DemoGame
{
    public void Config(Config config)
    {
#if DEBUG
        config.DebugMode = true;
#else
        config.DebugMode = false;
#endif
        config.ApplicationName = "Demo Game";
        config.WindowTitle = "Demo Game";
    }

    public static void Main(string[] args)
    {
        G2D.Launch(new DemoGame());
    }
}