namespace EshopByRaduz.AppHost
{
    internal static class Extensions
    {
        public static IResourceBuilder<T> MapUrlsToScalar<T>(this IResourceBuilder<T> builder)
            where T : IResourceWithEnvironment
        {
            builder.WithUrls(c =>
            {
                foreach (var url in c.Urls)
                {
                    if (url.Endpoint is not null)
                    {
                        var uri = new UriBuilder(url.Url) { Path = "scalar" };
                        url.Url = uri.ToString();
                    }
                }
            });

            return builder;
        }
    }
}
