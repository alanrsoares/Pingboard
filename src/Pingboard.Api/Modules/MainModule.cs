using Nancy;
using Pingboard.Model.Bundling;

namespace Pingboard.Api.Modules
{
    public class MainModule : NancyModule
    {
        public MainModule()
        {
            var viewModel = AssembleViewModel();
            Get["/"] = _ => View["index", viewModel];
        }

        private object AssembleViewModel()
        {
            return new
            {
                Message = "Hello, Nancy!",
                StyleBundle = Bundler.AssembleStyleBundle("common"),
                ScriptBundle = Bundler.AssembleScriptBundle("common")
            };
        }
    }
}
