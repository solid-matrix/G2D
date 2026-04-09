using System.Diagnostics;

namespace G2D.Demo;

public class FpsCounter
{
    private const float UpdateInterval = 1.0f;

    private readonly Stopwatch _stopwatch;

    private long _previousTimestamp;

    private float _frameCount;

    private float _elapsedTime;

    private float _instantFps;

    private float _averageFps;

    public FpsCounter()
    {
        _stopwatch = Stopwatch.StartNew();
        _previousTimestamp = _stopwatch.ElapsedTicks;
    }

    public float InstantFps => MathF.Round(_instantFps, 2);

    public float AverageFps => MathF.Round(_averageFps, 2);

    public event Action<float>? OnAverageFpsUpdate;

    public void Update()
    {
        var currentTimestamp = _stopwatch.ElapsedTicks;

        var deltaTime = (currentTimestamp - _previousTimestamp) / (float)Stopwatch.Frequency;

        _previousTimestamp = currentTimestamp;

        if (deltaTime > 0)
            _instantFps = 1.0f / deltaTime;

        _frameCount++;
        _elapsedTime += deltaTime;

        if (_elapsedTime >= UpdateInterval)
        {
            _averageFps = _frameCount / _elapsedTime;
            OnAverageFpsUpdate?.Invoke(_averageFps);
            _frameCount = 0;
            _elapsedTime = 0;
        }
    }

    public void Reset()
    {
        _instantFps = 0;
        _averageFps = 0;
        _frameCount = 0;
        _elapsedTime = 0;
        _previousTimestamp = _stopwatch.ElapsedTicks;
    }
}