namespace Naukri.Moltk.Fusion
{
    public record ProviderKey()
    {
        public static ProviderKey Empty { get; } = new();
    }

    public record ProviderKey<T>(T Key) : ProviderKey
    {
    }
}
