using G2D.Mathematics;

namespace G2D;

public partial class G2D
{
    private VulkanContext _vulkanContext;

    internal Graphics _graphics;

    internal Assets _assets;

    internal StepTimer _timer;

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
        _assets = new Assets(_vulkanContext._textureCollection);
        _timer = new StepTimer(_config.UpdateFrequency, _config.VSync ? _displayRefreshRate : _config.MaxRenderFrequency);

        _game.Load();
        _timer.Start();
        while (!ShouldClose)
        {
            // TODO
            // process events

            _timer.Step();

            while (_timer.RequireUpdate)
            {
                _game.Update(_timer.UpdateDeltaTime);
                _timer.SignalUpdated();
            }

            if (_windowWidth * _windowHeight > 0 && _timer.RequireRender)
                if (_vulkanContext.RenderFrame(_graphics.ClearColor, session =>
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