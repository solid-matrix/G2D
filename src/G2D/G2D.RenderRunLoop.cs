using G2D.Mathematics;

namespace G2D;

public partial class G2D
{
    private VulkanContext _vulkanContext = null!;

    private Graphics _graphics = null!;

    private AssetsManager _assetsManager = null!;

    private EventsManager _eventsManager = null!;

    private StepTimer _timer = null!;

    private void RenderRunLoop()
    {
        // wait window creating
        _initBarrier.SignalAndWait();

        // create vulkan instance
        _vulkanContext = new VulkanContext(
            _config.ApplicationName, _config.ApplicationVersion,
            _config.EngineName, _config.EngineVersion,
            [],
            _requiredVulkanInstanceExtensions,
            _config.DebugMode
        );
        _vkInstanceHandle = _vulkanContext.Instance;

        _initBarrier.SignalAndWait();
        // wait surface creating & window size
        _initBarrier.SignalAndWait();

        // initialize vulkan context
        _vulkanContext.Initialize(_vkSurfaceHandle, () => new Size2I(_windowWidth, _windowHeight));

        _initBarrier.SignalAndWait();

        _graphics = new Graphics(_vulkanContext);
        _assetsManager = new AssetsManager(_vulkanContext._textureCollection);
        _timer = new StepTimer(_config.UpdateFrequency, _config.VSync ? _displayRefreshRate : _config.MaxRenderFrequency);
        _eventsManager = new EventsManager();

        _game.Load();
        _timer.Start();
        while (!ShouldClose)
        {
            while (_eventChannel.Reader.TryRead(out var e)) _eventsManager.ProcessEvent(e);

            _timer.Step();

            while (_timer.RequireUpdate)
            {
                _game.Update(_timer.UpdateDeltaTime);
                _timer.SignalUpdated();
            }

            if (_timer.RequireRender && _vulkanContext.RenderFrame(_graphics.ClearColor, session =>
                {
                    _graphics.BeginSession(session);

                    _graphics.Uniform.MousePosition = default; //TODO Mouse.GetPosition();
                    _graphics.Uniform.Time = _timer.Time;

                    _game.Draw(_timer.RenderAlpha);

                    _graphics.EndSession();
                }))
                _timer.SignalRendered();


            Thread.Sleep(1);
        }


        _game.Unload();
        _vulkanContext.Cleanup();
    }
}