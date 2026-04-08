namespace G2D.Demo;

internal class MyGame : Game
{
#if DEBUG
    private const bool EnableDebug = true;
#else
    private const bool EnableDebug = false;
#endif

    private Texture _texture;

    protected override void Config(Config config)
    {
        config.DebugMode = EnableDebug;
        config.ApplicationName = "G2D Game";
        config.WindowTitle = "G2D Game";
        config.WindowResizable = true;
    }

    protected override void Load()
    {
        Window.HideCursor();

        GraphicsContext.ClearColor = Colors.CornflowerBlue;

        _texture = LoadTexture(Embedded.GetBytes("Assets/Images/background-pattern.png"));
    }

    protected override void Unload()
    {
        Console.WriteLine("Unloaded");
    }

    protected override void Update(double dt)
    {
    }

    protected override void Draw(Graphics g)
    {
        g.DrawRect(new Rect(0, 0, 40, 40), Colors.Black);

        g.DrawTexture(_texture, g.Extent / 2 - _texture.Extent / 2);

        var pos = Mouse.GetPosition();
        g.DrawRect(new Rect(pos.X - 20, pos.Y - 20, 40, 40), Colors.DarkRed);
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