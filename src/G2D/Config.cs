namespace G2D;

public class Config
{
    internal bool _locked = false;
    // TODO lock the config after configuration finished

    public string EngineName => "G2D";

    public Version EngineVersion { get; } = typeof(Config).Assembly.GetName().Version!;

    public string ApplicationName { get; set; } = "Untitled";

    public Version ApplicationVersion { get; set; } = new(0, 0, 0, 0);

    public string WindowTitle { get; set; } = "Untitled";

    public int WindowWidth { get; set; } = 800;

    public int WindowHeight { get; set; } = 600;

    public bool WindowBorderless { get; set; } = false;

    public bool WindowResizable { get; set; } = false;

    public bool WindowFullscreen { get; set; } = false;

    public bool WindowHideCursor { get; set; } = false;

    public bool EnableDebug { get; set; } = false;

    public bool VSync { get; set; } = true;

    public float UpdateFrequency { get; set; } = 60;

    public float MaxRenderFrequency { get; set; } = 60;
}