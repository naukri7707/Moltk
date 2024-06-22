using System;

public static class Parse<T>
{
    static Parse()
    {
        Parse<bool>.parser = obj => bool.Parse(obj);
        Parse<byte>.parser = obj => byte.Parse(obj);
        Parse<sbyte>.parser = obj => sbyte.Parse(obj);
        Parse<char>.parser = obj => char.Parse(obj);
        Parse<decimal>.parser = obj => decimal.Parse(obj);
        Parse<double>.parser = obj => double.Parse(obj);
        Parse<float>.parser = obj => float.Parse(obj);
        Parse<int>.parser = obj => int.Parse(obj);
        Parse<uint>.parser = obj => uint.Parse(obj);
        Parse<long>.parser = obj => long.Parse(obj);
        Parse<ulong>.parser = obj => ulong.Parse(obj);
        Parse<short>.parser = obj => short.Parse(obj);
        Parse<ushort>.parser = obj => ushort.Parse(obj);
        Parse<string>.parser = obj => obj;
    }

    public static Func<string, T> parser = obj => throw new NotImplementedException();

    public static T From(string obj)
    {
        return parser(obj);
    }
}
