using System.Threading.Channels;

namespace G2D;

public sealed partial class G2D
{
    // shared resources
    private volatile Config _config;

    private volatile IGame _game;

    private volatile Barrier _initBarrier;

    private volatile Channel<Event> _eventChannel;

    private volatile CancellationTokenSource _cts;

    private volatile string[] _requiredVulkanInstanceExtensions = null!;

    private volatile nint _vkInstanceHandle;

    private volatile nint _vkSurfaceHandle;

    private volatile int _windowWidth;

    private volatile int _windowHeight;

    private volatile float _displayRefreshRate;

    private G2D(Config config, IGame game)
    {
        _game = game;
        _config = config;
        _initBarrier = new Barrier(2);
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
        _initBarrier.Dispose();
    }
}