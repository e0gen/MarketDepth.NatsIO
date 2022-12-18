namespace MarketDepth.Infrastructure.Utils
{
    public static class ConfigurationExtension
    {
        public static string GetVarFromEnvironment(this IConfiguration configuration, string key)
        {
            return configuration.GetValue<string>(key);
        }
    }
}
