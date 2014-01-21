namespace Pingboard.Api.Modules.Pingdom
{
    using System;
    using Nancy;
    using Nancy.LightningCache.Extensions;

    public class ChecksModule : NancyModule
    {
        public ChecksModule()
            : base("/api/checks")
        {
            Get["/", true] = async (_, ctx) =>
            {
                var response = await PingdomClient.Pingdom.Client.Checks.GetChecksList();
                return Response.AsJson(response)
                               .AsCacheable(DateTime.Now.AddSeconds(60));
            };

            Get["/{id}", true] = async (_, ctx) =>
            {
                var response = await PingdomClient.Pingdom.Client.Checks.GetDetailedCheckInformation((int)_.id);
                return Response.AsJson(response)
                               .AsCacheable(DateTime.Now.AddSeconds(60));
            };
        }
    }
}
