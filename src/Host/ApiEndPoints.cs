
namespace StorageSpike.Host;

public static class ApiEndpoints
{
    private const string ApiBase = "api";

    public static class Storage
    {
        private const string Base = $"{ApiBase}/storage";
        private const string PathBase = $"{Base}/{{dealid}}";
        public const string PostGetConstraints =  $"{PathBase}/constraints";
    }
}
