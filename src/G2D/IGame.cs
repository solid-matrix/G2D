namespace G2D;

public interface IGame
{
    void Config(Config config);

    void Load();

    void Update(float dt);

    void Draw(float alpha);

    void Unload();
}