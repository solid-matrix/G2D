using G2D.Mathematics;

namespace G2D.Examples.Demo1;

public partial class MyGame : IGame
{
    private readonly FpsCounter _updateFps = new();

    private readonly FpsCounter _renderFps = new();

    private Texture _texture;

    public void Load()
    {
        G2D.Events.OnKeyboardEvent += (ref e) => G2D.Exit(e.Key == Keys.Escape && e.IsDown);
        _updateFps.OnAverageFpsUpdate += fps => Console.WriteLine($"Update Average FPS: {fps:0}");
        _renderFps.OnAverageFpsUpdate += fps => Console.WriteLine($"Render Average FPS: {fps:0}");


        _texture = G2D.Assets.LoadTexture(G2D.Embedded.GetBytes("Assets/texture.jpg"));
        G2D.Graphics.ClearColor4 = Colors.White;
    }

    public void Update(float dt)
    {
        _updateFps.Update();
    }

    public void Draw(float alpha)
    {
        _renderFps.Update();
        G2D.Graphics.Draw(
            _texture,
            //new Rect(Vec2.Zero, G2D.Graphics.Viewport.Size / 2),
            Sampler.NearestRepeat,
            G2D.Graphics.Viewport.Size / 2,
            0,
            new Vec2(1, 1),
            Vec2.Zero,
            new Vec2(0.0F, 0.0F));
    }

    public void Unload()
    {
        // No need, unload automatically
        // G2D.Assets.UnloadTexture(_texture);
    }
}