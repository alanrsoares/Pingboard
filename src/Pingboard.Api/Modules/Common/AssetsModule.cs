using Nancy;
using Pingboard.Model.Bundling;

namespace Pingboard.Api.Modules.Common
{
    public class AssetModule : NancyModule
    {
        public AssetModule()
            : base("/assets")
        {
            Get["/js/{name}"] = parameters => Bundler.CreateJavascriptResponse(Response, parameters);
            Get["/css/{name}"] = parameters => Bundler.CreateCssResponse(Response, parameters);
        }
    }
}