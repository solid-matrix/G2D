namespace G2D;

public class AsyncValue<T>
{
    private readonly TaskCompletionSource<T> _tcs = new();

    public void SetValue(T value)
    {
        _tcs.TrySetResult(value);
    }

    public T AwaitValue()
    {
        return _tcs.Task.Result;
    }
}