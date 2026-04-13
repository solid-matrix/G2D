**Still Under Development**



# G2D

Features:

- Pure C# Code Development

  No editor required - build entirely with code.

- Next-generation, designing for the feature

  Supports only Vulkan 1.3+ capable devices; no OpenGL backend

- Lightweight

  Empty published project: ~8MB, compressed: ~4MB

- Ultra-high-performant

  Leverages powerful Vulkan features (bindless, dynamic rendering), outperforming OpenGL-based engines

- Cross-platform

  Windows, Linux, macOS, iOS, Android, PlayStation, Xbox, and more

- Native AOT compilation

  No virtual machine / CLR needed, instant startup

- Easy-to-use API

  Zero GC during runtime loop; APIs encourage zero-GC coding for users

## Usage

```c#
public class MyGame : IGame
{
    private static readonly EmbeddedResource Embedded = new(typeof(MyGame).Assembly);

    private Texture _texture;

    private float _rotate;

    private float _speed;

    private float _scale = 1.0f;

    public void Config(Config config)
    {
        config.WindowTitle = "MyGame";
    }

    public void Load()
    {
        _texture = G2D.Assets.LoadTexture(Embedded.GetBytes("Assets/texture.jpg"));
        
        G2D.Events.OnMouseWheel += (in e) => _scale += e.Scroll.Y * 0.02f;
        
        G2D.Graphics.ClearColor = Colors.White;
    }

    public void Update(float dt)
    {
        if (G2D.Mouse.IsButtonDown(MouseButtons.Left)) _speed -= 0.05f;
        
        if (G2D.Mouse.IsButtonDown(MouseButtons.Right)) _speed += 0.05f;
        
        _rotate += dt * MathF.PI / 2 * _speed;
    }

    public void Draw(float alpha)
    {
        G2D.Graphics.Draw(_texture, G2D.Graphics.Viewport / 2, _rotate, _scale);
    }

    public void Unload()
    {
    }

    private static void Main(string[] args) => G2D.Launch(new MyGame());
}
```



## Structure

- `G2D`: Core library providing window management, event handling, rendering, audio, and video functionality.
- `G2D.Ecs`: ECS architecture framework built on top of G2D.
- `G2D.GameObject`: Node-tree architecture framework based on G2D.

**Tech Stack**

- SDL3: Windowing and event handling, utilizing the `ppy.SDL3-CS` binding library.

- Vulkan: Graphics API, utilizing the `Vortice.Vulkan` binding library.



