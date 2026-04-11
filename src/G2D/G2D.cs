using SDL;

namespace G2D;

public static unsafe class G2D
{
    private static bool _running;

    private static GraphicsContext? _graphicsContext;

    private static readonly EmbeddedResource _internalEmbedded = new(typeof(G2D).Assembly);

    private static Keyboard? _keyboard;

    private static Mouse? _mouse;

    private static GamePad? _gamePad;

    private static EmbeddedResource? _embedded;

    private static Assets? _assets;

    private static StepTimer? _timer;

    private static Events? _events;

    private static Window? _window;

    private static Graphics? _graphics;

    private static IGame? _game;

    private static Type? _gameType;

    internal static bool Running => _running;

    internal static GraphicsContext GraphicsContext => _graphicsContext ?? throw new InvalidOperationException("GraphicsContext is not ready");

    internal static EmbeddedResource InternalEmbedded => _internalEmbedded ?? throw new InvalidOperationException("InternalEmbedded is not ready");

    public static Keyboard Keyboard => _keyboard ?? throw new InvalidOperationException("Keyboard is not ready");

    public static Mouse Mouse => _mouse ?? throw new InvalidOperationException("Mouse is not ready");

    public static GamePad GamePad => _gamePad ?? throw new InvalidOperationException("GamePad is not ready");

    public static EmbeddedResource Embedded => _embedded ?? throw new InvalidOperationException("Embedded is not ready");

    public static Assets Assets => _assets ?? throw new InvalidOperationException("Assets is not ready");

    public static StepTimer Timer => _timer ?? throw new InvalidOperationException("Timer is not ready");

    public static Events Events => _events ?? throw new InvalidOperationException("Events is not ready");

    public static Window Window => _window ?? throw new InvalidOperationException("Window is not ready");

    public static Graphics Graphics => _graphics ?? throw new InvalidOperationException("Graphics is not ready");

    private static IGame Game => _game ?? throw new InvalidOperationException("Game is not ready");

    private static Type GameType => _gameType ?? throw new InvalidOperationException("Game is not ready");

    public static void Launch<T>(T game) where T : IGame
    {
        _game = game;
        _gameType = typeof(T);
        Initialize();
        RunLoop();
        Cleanup();
    }

    private static void Initialize()
    {
        _running = false;

        var config = new Config();
        Game.Config(config);

        _embedded = new EmbeddedResource(GameType.Assembly);

        _window = new Window(
            config.WindowTitle, config.WindowWidth, config.WindowHeight,
            (config.WindowResizable ? WindowFlags.Resizable : WindowFlags.None)
            | (config.WindowBorderless ? WindowFlags.Borderless : WindowFlags.None)
            | (config.WindowFullscreen ? WindowFlags.Fullscreen : WindowFlags.None)
        );

        if (config.VSync)
            _timer = new StepTimer(config.UpdateFrequency, Window.GetDisplayRefreshRate());
        else if (config.MaxRenderFrequency <= 0)
            _timer = new StepTimer(config.UpdateFrequency, float.MaxValue);
        else
            _timer = new StepTimer(config.UpdateFrequency, config.MaxRenderFrequency);

        _graphicsContext = new GraphicsContext(
            Window,
            config.ApplicationName, config.ApplicationVersion,
            config.EngineName, config.EngineVersion,
            config.DebugMode);
        _graphics = new Graphics(_graphicsContext);
        _assets = new Assets(_graphicsContext._textureManager);
        _keyboard = new Keyboard();
        _mouse = new Mouse();
        _gamePad = new GamePad();
        _events = new Events();

        Events.OnQuitEvent += (ref _) => _running = false;
    }

    private static void RunLoop()
    {
        Game.Load();
        Timer.Start();

        Window.Show();
        _running = true;
        SDL_Event e = new();
        while (_running)
        {
            while (SDL3.SDL_PollEvent(&e)) Events.Process(ref e);

            if (!_running) break;

            Timer.Step();

            while (Timer.RequireUpdate)
            {
                Game.Update(Timer.UpdateDeltaTime);
                Timer.NotifyUpdated();
            }

            if (!Window.IsMinimized() && Timer.RequireRender)
            {
                var res = GraphicsContext.RenderFrame(session =>
                {
                    Graphics.BeginSession(session);

                    Graphics.Uniform.MousePosition = Mouse.GetPosition();

                    Graphics.Uniform.Time = Timer.Time;

                    Game.Draw(Timer.RenderAlpha);

                    Graphics.EndSession();
                });

                if (res) Timer.NotifyRendered();
            }
        }

        Game.Unload();
    }

    private static void Cleanup()
    {
        ((IDisposable)GraphicsContext).Dispose();
        ((IDisposable)Window).Dispose();
    }

    public static void Exit()
    {
        _running = false;
    }
}