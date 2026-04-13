namespace G2D.Mathematics;

public readonly struct Mat4X4
{
    private readonly Vec4 _c0;
    private readonly Vec4 _c1;
    private readonly Vec4 _c2;
    private readonly Vec4 _c3;

    public Vec4 Col0 => _c0;
    public Vec4 Col1 => _c1;
    public Vec4 Col2 => _c2;
    public Vec4 Col3 => _c3;

    public Mat4X4()
    {
    }

    public Mat4X4(Vec4 c0, Vec4 c1, Vec4 c2, Vec4 c3)
    {
        _c0 = c0;
        _c1 = c1;
        _c2 = c2;
        _c3 = c3;
    }

    // TODO
}