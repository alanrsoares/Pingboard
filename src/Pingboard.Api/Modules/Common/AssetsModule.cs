using Nancy;
using Pingboard.Model.Bundling;

namespace Pingboard.Api.Modules.Common
{
    public class AssetModule : NancyModule
    {
        public AssetModule()
            : base("/assets")
        {
            Get["/js/{name}"] = _ => Bundler.CreateJavascriptResponse(Response, _);
            Get["/css/{name}"] = _ => Bundler.CreateCssResponse(Response, _);
        }
    }
}