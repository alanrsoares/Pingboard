using Nancy;
using Nancy.Bootstrapper;
using Nancy.Conventions;
using Nancy.Routing;
using Nancy.TinyIoc;
using Nancy.LightningCache.Extensions;
using Pingboard.Model.Bundling;

namespace Pingboard.Api.Startup
{
    public class Bootstrapper : DefaultNancyBootstrapper
    {
        protected override void ApplicationStartup(TinyIoCContainer container, IPipelines pipelines)
        {
            base.ApplicationStartup(container, pipelines);

            // Setup bundles
            Bundler.Setup();
            this.EnableLightningCache(container.Resolve<IRouteResolver>(), ApplicationPipelines, new[] { "id", "query", "take", "skip" });
        }

        protected override void ConfigureConventions(NancyConventions conventions)
        {
            base.ConfigureConventions(conventions);
            conventions.StaticContentsConventions.AddDirectory("app");
        }
    }
}