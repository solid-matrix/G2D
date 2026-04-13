using G2D.Mathematics;

namespace G2D.Examples.Demo1;

public partial class MyGame : IGame
{
    private readonly FpsCounter _updateFps = new();

    private readonly FpsCounter _renderFps = new();

    private Texture _texture;

    private float _r;

    private readonly EmbeddedResource _embeddedResource = new(typeof(MyGame).Assembly);

    public void Load()
    {
        // _updateFps.OnAverageFpsUpdate += fps => Console.WriteLine($"Update Average FPS: {fps:0}");
        // _renderFps.OnAverageFpsUpdate += fps => Console.WriteLine($"Render Average FPS: {fps:0}");


        _texture = G2D.Assets.LoadTexture(_embeddedResource.GetBytes("Assets/texture.jpg"));
        G2D.Graphics.ClearColor = Colors.Transparent;
    }

    public void Update(float dt)
    {
        _updateFps.Update();

        _r += dt * MathF.PI / 2;
    }

    public void Draw(float alpha)
    {
        _renderFps.Update();

        //NewG2D.Graphics.Draw(new Rect(0, 0, 200, 200), Colors.Red);
        G2D.Graphics.Draw(
            _texture,
            //new Rect(Vec2.Zero, G2D.Graphics.Viewport.Size / 2),
            Sampler.LinearClamp,
            G2D.Graphics.Viewport.Size / 2,
            _r,
            new Vec2(0.5f, 0.5f),
            Vec2.Zero,
            new Vec2(0.0F, 0.0F));
    }

    public void Unload()
    {
        // No need, unload automatically
        // G2D.Assets.UnloadTexture(_texture);
    }
}