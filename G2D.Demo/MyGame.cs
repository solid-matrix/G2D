namespace G2D.Demo;

internal class MyGame : Game
{
#if DEBUG
    private const bool EnableDebug = true;
#else
    private const bool EnableDebug = false;
#endif

    protected override void Config(Config config)
    {
        config.DebugMode = EnableDebug;
        config.ApplicationName = "G2D Game";
        config.WindowTitle = "G2D Game";
        config.WindowResizable = true;
        // config.VSync = false;
    }

    protected override void Load()
    {
        Console.WriteLine("Loaded");
        Window.HideCursor();
    }

    protected override void Unload()
    {
        Console.WriteLine("Unloaded");
    }

    protected override void Update(double dt)
    {
    }

    protected override void Draw()
    {
        Graphics.SetClearColor(Colors.CornflowerBlue);

        // Graphics.SetColor(new Color(1, 0.0f, 0.0f));

        Graphics.DrawRect(new Rect(0, 0, 40, 40), Colors.Black);

        var pos = Mouse.GetPosition();
        Graphics.DrawRect(new Rect(pos.X - 20, pos.Y - 20, 40, 40), Colors.DarkRed);
    }

    protected override void Event(ref KeyboardEvent e)
    {
        if (e.Key == Keys.Escape && e.IsDown) Exit();
    }

    public static void Main(string[] args)
    {
        new MyGame().Launch(args);
    }
}