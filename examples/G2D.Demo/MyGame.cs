using Vortice.Mathematics;

namespace G2D.Demo;

internal class MyGame : Game
{
#if DEBUG
    private const bool EnableDebug = true;
#else
    private const bool EnableDebug = false;
#endif

    private Texture _texture;

    private readonly FpsCounter _fpsCounter = new();

    protected override void Config(Config config)
    {
        config.DebugMode = EnableDebug;
        config.ApplicationName = "G2D Game";
        config.WindowTitle = "G2D Game";
        config.WindowResizable = true;
        config.VSync = false;
        config.TargetFps = 0;
    }

    protected override void Load()
    {
        _texture = LoadTexture(Embedded.GetBytes("Assets/Images/logo.png"));

        Window.HideCursor();
        Graphics.ClearColor4 = Colors.White;
        _fpsCounter.OnAverageFpsUpdate += fps => Console.WriteLine($"Average FPS: {fps:0}");
    }

    protected override void Unload()
    {
    }

    protected override void Update(double dt)
    {
        _fpsCounter.Update();
    }

    protected override void Draw()
    {
        var random = new Random(DateTimeOffset.UtcNow.Microsecond);
        var width = (int)Graphics.Viewport.Width;
        var height = (int)Graphics.Viewport.Height;

        for (var j = 0; j < 1; j++)
        for (var i = 0; i < 40960; i++)
        {
            var x = random.Next(width);
            var y = random.Next(height);
            var r = random.NextSingle() * MathF.PI * 2;
            var s = random.NextSingle();

            Graphics.Draw(_texture, Sampler.NearestRepeat, x, y, r, s, s);
            Graphics.FlushUnitRectDraw();
        }
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