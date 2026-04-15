using System.Threading.Channels;

namespace G2D;

public sealed partial class G2D
{
    // shared resources
    private volatile Config _config;

    private volatile IGame _game;

    private volatile Channel<Event> _eventChannel;

    private volatile CancellationTokenSource _cts;

    private volatile AsyncValue<string[]> _requiredVulkanInstanceExtensions = new();

    private volatile AsyncValue<nint> _vkInstanceHandle = new();

    private volatile AsyncValue<nint> _vkSurfaceHandle = new();

    private volatile int _windowWidth;

    private volatile int _windowHeight;

    private volatile float _displayRefreshRate;

    private G2D(Config config, IGame game)
    {
        _game = game;
        _config = config;
        _eventChannel = Channel.CreateUnbounded<Event>(new UnboundedChannelOptions { SingleReader = true, SingleWriter = true });
        _cts = new CancellationTokenSource();
    }

    private bool ShouldClose => _cts.Token.IsCancellationRequested;

    private void Run()
    {
        var renderThread = new Thread(RenderRunLoop)
        {
            IsBackground = true
        };
        renderThread.Start();

        WindowRunLoop();

        renderThread.Join();
    }

    private void Cleanup()
    {
        _cts.Dispose();
    }
}