namespace G2D;

public readonly struct QueueType
{
    private readonly int _value;

    private QueueType(int value)
    {
        _value = value;
    }


    public static readonly QueueType Graphics = new(0);

    public static readonly QueueType Compute = new(1);

    public static readonly QueueType Transfer = new(2);

    public static int GetTypeCount()
    {
        return 3;
    }

    private static readonly QueueType[] _allTypes = [Graphics, Compute, Transfer];

    public static ReadOnlySpan<QueueType> GetAllTypes()
    {
        return _allTypes;
    }

    private static string GetName(QueueType type)
    {
        if (type == Graphics)
            return nameof(Graphics);
        if (type == Compute)
            return nameof(Compute);
        if (type == Transfer)
            return nameof(Transfer);

        return string.Empty;
    }

    public override string ToString()
    {
        return GetName(this);
    }

    // public static readonly QueueType VideoDecode = new(3);
    //
    // public static readonly QueueType VideoEncode = new(4);

    public static implicit operator int(QueueType value)
    {
        return value._value;
    }
}