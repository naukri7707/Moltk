namespace Naukri.Moltk.Fusion
{
    public record ProviderKey;

    public record ProviderKey<T>(T Key) : ProviderKey;
}
