using G2D.Mathematics;

namespace G2D.Examples.Demo1;

public partial class MyGame : IGame
{
    private readonly FpsCounter _updateFps = new();

    private readonly FpsCounter _renderFps = new();

    private Texture _texture;

    private float _r;

    private float _speed;

    private float _scale = 1.0f;

    private readonly EmbeddedResource _embedded = new(typeof(MyGame).Assembly);

    public void Load()
    {
        // _updateFps.OnAverageFpsUpdate += fps => Console.WriteLine($"Update Average FPS: {fps:0}");
        // _renderFps.OnAverageFpsUpdate += fps => Console.WriteLine($"Render Average FPS: {fps:0}");

        _texture = G2D.Assets.LoadTexture(_embedded.GetBytes("Assets/texture.jpg"));

        G2D.Graphics.ClearColor = Colors.White;

        G2D.Events.OnKeyDown += (in e) => G2D.Exit(e.Key == Keys.Escape);

        G2D.Events.OnMouseWheel += (in e) =>
        {
            if (e.Direction == MouseWheelDirection.Normal)
                _scale += e.Scroll.Y * 0.02f;
            else
                _scale -= e.Scroll.Y * 0.02f;
        };
    }

    public void Update(float dt)
    {
        _updateFps.Update();

        if (G2D.Mouse.IsButtonDown(MouseButtons.Left)) _speed -= 0.05f;
        if (G2D.Mouse.IsButtonDown(MouseButtons.Right)) _speed += 0.05f;
        if (G2D.Mouse.IsButtonDown(MouseButtons.Middle)) _speed = 0;

        _r += dt * MathF.PI / 2 * _speed;
    }

    public void Draw(Graphics graphics, float alpha)
    {
        _renderFps.Update();

        G2D.Graphics.Draw(new Rect(0, 0, 200, 200), Colors.Blue);

        G2D.Graphics.Draw(
            _texture,
            Sampler.LinearClamp,
            G2D.Graphics.Viewport / 2,
            _r,
            new Vec2(_scale),
            Vec2.Zero,
            new Vec2(0.0F, 0.0F));
    }

    public void Unload()
    {
    }
}