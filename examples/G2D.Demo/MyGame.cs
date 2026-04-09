using System.Numerics;
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
        Console.WriteLine($"EnableDebug = {EnableDebug}");
        _texture = LoadTexture(Embedded.GetBytes("Assets/Images/background-pattern.png"));

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


        // for (var i = 0; i < 50000; i++)
        // {
        //     var x1 = random.Next(width);
        //     var x2 = random.Next(width);
        //     var y1 = random.Next(height);
        //     var y2 = random.Next(height);
        //     if (x1 > x2) (x1, x2) = (x2, x1);
        //     if (y1 > y2) (y1, y2) = (y2, y1);
        //
        //     var r = random.Next(256);
        //     var g = random.Next(256);
        //     var b = random.Next(256);
        //
        //     Graphics.Draw(new Rect(x1, y1, x2 - x1, y2 - y1), new Color(r, g, b).ToColor4());
        // }

        Graphics.Draw(_texture, new Vector2(200, 200));
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