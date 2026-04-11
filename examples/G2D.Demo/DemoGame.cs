using G2D.Mathematics;

namespace G2D.Demo;

public partial class DemoGame : IGame
{
    private Texture _texture;

    private int _count;

    private readonly FpsCounter _updateFps = new();

    private readonly FpsCounter _renderFps = new();

    public void Load()
    {
        _texture = G2D.Assets.LoadTexture(G2D.Embedded.GetBytes("Assets/Images/atlas.png"));

        G2D.Window.HideCursor();
        G2D.Graphics.ClearColor4 = Colors.White;

        _updateFps.OnAverageFpsUpdate += fps => Console.WriteLine($"Update Average FPS: {fps:0}");
        _renderFps.OnAverageFpsUpdate += fps => Console.WriteLine($"Render Average FPS: {fps:0}");
    }

    public void Update(float dt)
    {
        _updateFps.Update();
        _count = (_count + 1) % 60;
    }

    public void Draw(float alpha)
    {
        _renderFps.Update();

        var pos = G2D.Graphics.Viewport / 2 - new Vec2(80, 80) / 2;

        if (_count < 20)
            G2D.Graphics.Draw(_texture, new Rect(20, 0, 20, 20), Sampler.NearestRepeat, pos.X, pos.Y, 0, 4, 4, 0, 0, 0, 0);
        else if (_count < 40)
            G2D.Graphics.Draw(_texture, new Rect(20, 20, 20, 20), Sampler.NearestRepeat, pos.X, pos.Y, 0, 4, 4, 0, 0, 0, 0);
        else
            G2D.Graphics.Draw(_texture, new Rect(40, 0, 20, 20), Sampler.NearestRepeat, pos.X, pos.Y, 0, 4, 4, 0, 0, 0, 0);
    }

    public void Unload()
    {
        G2D.Assets.UnloadTexture(_texture);
    }
}