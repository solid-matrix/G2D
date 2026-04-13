namespace G2D.Examples.Demo1;

public partial class MyGame
{
    public void Config(Config config)
    {
#if DEBUG
        config.EnableDebug = true;
#else
        config.EnableDebug = false;
#endif
        config.ApplicationName = "G2D Demo 1";
        config.WindowTitle = "G2D Demo 1";
        config.WindowResizable = true;
    }

    private static void Main(string[] args)
    {
        G2D.Launch(new MyGame());
    }
}