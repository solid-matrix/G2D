using System.Diagnostics;

namespace G2D;

public class StepTimer
{
    internal readonly Stopwatch _sw = new();
    private double _delta;
    private long _frameIntervalTicks;
    private double _last;
    private long _lastFrameTicks;
    private int _targetFps;

    public StepTimer(int targetFps = 0)
    {
        _sw = new Stopwatch();
        SetTargetFps(targetFps);
    }

    public float NowF => (float)_sw.Elapsed.TotalSeconds;

    public double Now => _sw.Elapsed.TotalSeconds;

    public int TargetFps
    {
        get => _targetFps;
        set => SetTargetFps(value);
    }

    public void Start()
    {
        _sw.Start();
        _last = 0;
        _lastFrameTicks = _sw.ElapsedTicks;
    }

    public void Stop()
    {
        _sw.Stop();
    }

    public void Restart()
    {
        _sw.Restart();
        _last = 0;
        _lastFrameTicks = _sw.ElapsedTicks;
    }

    public void Step()
    {
        var current = NowF;
        _delta = current - _last;
        _last = current;
    }

    internal void WaitTargetFps()
    {
        if (_frameIntervalTicks == 0) return;

        var currentTicks = _sw.ElapsedTicks;
        var elapsedTicks = currentTicks - _lastFrameTicks;
        var waitTicks = _frameIntervalTicks - elapsedTicks;

        if (waitTicks > 0)
        {
            var waitMs = (double)waitTicks / Stopwatch.Frequency * 1000;
            if (waitMs > 1)
            {
                var sleepMs = (int)Math.Floor(waitMs - 0.5);
                Thread.Sleep(sleepMs);
            }

            while (_sw.ElapsedTicks - _lastFrameTicks < _frameIntervalTicks)
            {
            }
        }

        _lastFrameTicks = _sw.ElapsedTicks;
    }

    public float GetTimeF()
    {
        return (float)_last;
    }

    internal float GetDeltaTimeF()
    {
        return (float)_delta;
    }

    public double GetTime()
    {
        return _last;
    }

    internal double GetDeltaTime()
    {
        return _delta;
    }

    private void SetTargetFps(int fps)
    {
        _targetFps = Math.Clamp(fps, 0, 1000);
        _frameIntervalTicks = _targetFps <= 0 ? 0 : Stopwatch.Frequency / _targetFps;
    }
}