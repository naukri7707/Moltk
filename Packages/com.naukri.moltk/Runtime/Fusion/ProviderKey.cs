namespace Naukri.Moltk.Fusion
{
    public record ProviderKey()
    {
        public static ProviderKey Empty => new();
    }

    public record ProviderKey<T>(T Key) : ProviderKey;
}
