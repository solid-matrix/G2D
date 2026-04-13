namespace G2D;

public interface IGame : IUpdatable, IDrawable, ILifecycle
{
    void Config(Config config);
}