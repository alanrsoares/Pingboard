using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Nancy;
using SquishIt.Framework;
using SquishIt.Framework.CSS;
using SquishIt.Framework.JavaScript;

namespace Pingboard.Model.Bundling
{
    public static class Bundler
    {
        public static string AssembleScriptBundle(string bundle)
        {
            return StaticConfiguration.IsRunningDebug
                ? Bundle.JavaScript().RenderNamed(string.Format("{0}-scripts-debug", bundle))
                : Bundle.JavaScript().RenderCachedAssetTag(string.Format("{0}-scripts", bundle));
        }

        public static string AssembleStyleBundle(string bundle)
        {
            return StaticConfiguration.IsRunningDebug
                ? Bundle.Css().RenderNamed(string.Format("{0}-styles-debug", bundle))
                : Bundle.Css().RenderCachedAssetTag(string.Format("{0}-styles", bundle));
        }

        public static string AssembleScriptBundles(params string[] bundles)
        {
            return string.Join("\r\n", bundles.Select(AssembleScriptBundle));
        }

        public static string AssembleStyleBundles(params string[] bundles)
        {
            return string.Join("\r\n", bundles.Select(AssembleStyleBundle));
        }

        private static string _basePathForTesting = "";

        private static JavaScriptBundle BuildJavaScriptBundle(IEnumerable<SquishItFile> files)
        {
            var bundle = Bundle.JavaScript();

            foreach (var item in files)
            {
                var url = item.Url;

                if (!string.IsNullOrWhiteSpace(_basePathForTesting))
                {
                    url = _basePathForTesting + item.Url.Replace("~", "");
                }

                if (item.Minify)
                {
                    bundle.Add(url);
                }
                else
                {
                    bundle.AddMinified(url);
                }
            }

            return bundle;
        }

        private static CSSBundle BuildCssBundle(IEnumerable<SquishItFile> files)
        {
            var bundle = Bundle.Css();

            foreach (var item in files)
            {
                var url = item.Url;

                if (!string.IsNullOrWhiteSpace(_basePathForTesting))
                {
                    url = _basePathForTesting + item.Url.Replace("~", "");
                }

                if (item.Minify)
                {
                    bundle.Add(url);
                }
                else
                {
                    bundle.AddMinified(url);
                }
            }

            return bundle;
        }

        public static void Setup(string basePathForTesting = "")
        {
            _basePathForTesting = basePathForTesting;

            // CSS
            BuildCssBundle(Bundles.CommonStyles).ForceRelease().AsCached("common-styles", "~/assets/css/common-styles");
            BuildCssBundle(Bundles.CommonStyles).ForceDebug().AsNamed("common-styles-debug", "");

            // JS
            BuildJavaScriptBundle(Bundles.CommonScripts).ForceRelease().AsCached("common-scripts", "~/assets/js/common-scripts");
            BuildJavaScriptBundle(Bundles.CommonScripts).ForceDebug().AsNamed("common-scripts-debug", "");
        }

        public static dynamic CreateJavascriptResponse(IResponseFormatter response, dynamic parameters)
        {
            return ResponseFormatterExtensions.CreateJavascriptResponse(response, parameters);
        }

        public static dynamic CreateCssResponse(IResponseFormatter response, dynamic parameters)
        {
            return ResponseFormatterExtensions.CreateCssResponse(response, parameters);
        }
    }

    public static class ResponseFormatterExtensions
    {
        public static Response CreateCssResponse(this IResponseFormatter response, dynamic parameters)
        {
            var cacheRendered = Bundle.Css().RenderCached((string)parameters.name);
            return response.CreateResponse(cacheRendered, Configuration.Instance.CssMimeType);
        }

        public static Response CreateJavascriptResponse(this IResponseFormatter response, dynamic parameters)
        {
            var cacheRendered = Bundle.JavaScript().RenderCached((string)parameters.name);
            return response.CreateResponse(cacheRendered, Configuration.Instance.JavascriptMimeType);
        }

        private static Response CreateResponse(this IResponseFormatter response, string content, string contentType)
        {
            return response
                .FromStream(() => new MemoryStream(Encoding.UTF8.GetBytes(content)), contentType)
                .WithHeader("Cache-Control", "max-age=45");
        }
    }
}