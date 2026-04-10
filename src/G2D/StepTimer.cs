using System.Diagnostics;

namespace G2D;

public class StepTimer
{
    private readonly Stopwatch _sw;

    private long _lastTicks;

    private long _deltaTicks;

    private readonly long _updateFixedDeltaTicks;

    private long _updateAccumulator;

    private readonly long _renderFixedDeltaTicks;

    private long _renderAccumulator;

    internal StepTimer(double updateFrequency, double maxRenderFrequency)
    {
        _sw = new Stopwatch();
        _lastTicks = _sw.ElapsedTicks;
        _deltaTicks = 0;
        _updateFixedDeltaTicks = (long)(Stopwatch.Frequency / updateFrequency);
        _renderFixedDeltaTicks = (long)(Stopwatch.Frequency / maxRenderFrequency);
        _updateAccumulator = 0;
        _renderAccumulator = 0;
    }

    public float UpdateDeltaTime => 1.0f * _updateFixedDeltaTicks / Stopwatch.Frequency;

    public float RenderAlpha => 1.0f * _updateAccumulator / _updateFixedDeltaTicks;

    public float Time => 1.0f * _lastTicks / Stopwatch.Frequency;

    internal bool RequireUpdate => _updateAccumulator >= _updateFixedDeltaTicks;

    internal bool RequireRender => _renderAccumulator >= _renderFixedDeltaTicks;

    internal void NotifyUpdated()
    {
        _updateAccumulator -= _updateFixedDeltaTicks;
    }


    internal void NotifyRendered()
    {
        _renderAccumulator %= _renderFixedDeltaTicks;
    }

    public void Step()
    {
        var current = _sw.ElapsedTicks;
        _deltaTicks = current - _lastTicks;
        _lastTicks = current;

        _updateAccumulator += _deltaTicks;
        _renderAccumulator += _deltaTicks;
    }

    public void Start()
    {
        _sw.Start();
        _lastTicks = _sw.ElapsedTicks;
        _deltaTicks = 0;
    }

    public void Stop()
    {
        _sw.Stop();
    }
}