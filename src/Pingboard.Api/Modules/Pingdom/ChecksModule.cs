﻿using Nancy;

namespace Pingboard.Api.Modules.Pingdom
{
    public class ChecksModule : NancyModule
    {
        public ChecksModule()
            : base("/api/checks")
        {
            Get["/", true] = async (_, ctx) =>
            {
                var response = await PingdomClient.Pingdom.Client.Checks.GetChecksList();
                return Response.AsJson(response);
            };

            Get["/{id}", true] = async (_, ctx) =>
            {
                var response = await PingdomClient.Pingdom.Client.Checks.GetDetailedCheckInformation((int)_.id);
                return Response.AsJson(response);
            };
        }
    }
}