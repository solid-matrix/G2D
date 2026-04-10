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

    private readonly FpsCounter _updateFps = new();
    private readonly FpsCounter _renderFps = new();

    protected override void Config(Config config)
    {
        config.DebugMode = EnableDebug;
        config.ApplicationName = "G2D Game";
        config.WindowTitle = "G2D Game";
        config.WindowResizable = true;
        config.VSync = true;
        // config.TargetFps = 0;
    }

    protected override void Load()
    {
        _texture = LoadTexture(Embedded.GetBytes("Assets/Images/atlas.png"));

        Window.HideCursor();
        Graphics.ClearColor4 = Colors.White;

        _updateFps.OnAverageFpsUpdate += fps => Console.WriteLine($"Update Average FPS: {fps:0}");
        _renderFps.OnAverageFpsUpdate += fps => Console.WriteLine($"Render Average FPS: {fps:0}");
    }

    protected override void Unload()
    {
    }

    private int _count;

    protected override void Update(float dt)
    {
        _updateFps.Update();

        _count = (_count + 1) % 60;
    }

    protected override void Draw(float alpha)
    {
        _renderFps.Update();

        var pos = Graphics.Viewport.ToVector2() / 2 - new Vector2(80, 80) / 2;

        if (_count < 20)
            Graphics.Draw(_texture, new Rect(20, 0, 20, 20), Sampler.NearestRepeat, pos.X, pos.Y, 0, 4, 4);
        else if (_count < 40)
            Graphics.Draw(_texture, new Rect(20, 20, 20, 20), Sampler.NearestRepeat, pos.X, pos.Y, 0, 4, 4);
        else
            Graphics.Draw(_texture, new Rect(40, 0, 20, 20), Sampler.NearestRepeat, pos.X, pos.Y, 0, 4, 4);
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