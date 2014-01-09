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
    public class Bundler
    {
        public static string AssembleScriptBundle(string bundle)
        {
            return StaticConfiguration.IsRunningDebug
                ? Bundle.JavaScript().RenderNamed(string.Format("{0}-js-debug", bundle))
                : Bundle.JavaScript().RenderCachedAssetTag(string.Format("{0}-js", bundle));
        }

        public static string AssembleStyleBundle(string bundle)
        {
            return StaticConfiguration.IsRunningDebug
                ? Bundle.Css().RenderNamed(string.Format("{0}-css-debug", bundle))
                : Bundle.Css().RenderCachedAssetTag(string.Format("{0}-css", bundle));
        }

        public static string AssembleScriptBundles(IEnumerable<string> bundles)
        {
            return string.Join("\r\n", bundles.Select(AssembleScriptBundle));
        }

        public static string AssembleStyleBundles(IEnumerable<string> bundles)
        {
            return string.Join("\r\n", bundles.Select(AssembleStyleBundle));
        }

        protected static string BasePathForTesting = "";

        protected static JavaScriptBundle BuildJavaScriptBundle(IEnumerable<SquishItFile> files)
        {
            var bundle = Bundle.JavaScript();

            foreach (var item in files)
            {
                var url = item.Url;

                if (!string.IsNullOrWhiteSpace(BasePathForTesting))
                {
                    url = BasePathForTesting + item.Url.Replace("~", "");
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

        protected static CSSBundle BuildCssBundle(IEnumerable<SquishItFile> files)
        {
            var bundle = Bundle.Css();

            foreach (var item in files)
            {
                var url = item.Url;

                if (!string.IsNullOrWhiteSpace(BasePathForTesting))
                {
                    url = BasePathForTesting + item.Url.Replace("~", "");
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
            BasePathForTesting = basePathForTesting;

            // CSS
            BuildCssBundle(Bundles.CommonCss).ForceRelease().AsCached("common-css", "~/assets/css/common-css");
            BuildCssBundle(Bundles.CommonCss).ForceDebug().AsNamed("common-css-debug", "");

            // JS
            BuildJavaScriptBundle(Bundles.CommonJavascript).ForceRelease().AsCached("common-js", "~/assets/js/common-js");
            BuildJavaScriptBundle(Bundles.CommonJavascript).ForceDebug().AsNamed("common-js-debug", "");
        }

        public static dynamic CreateJavascriptResponse(IResponseFormatter response, dynamic parameters)
        {
            return IResponseFormatterExtensions.CreateJavascriptResponse(response, parameters);
        }

        public static dynamic CreateCssResponse(IResponseFormatter response, dynamic parameters)
        {
            return IResponseFormatterExtensions.CreateCssResponse(response, parameters);
        }
    }

    public static class IResponseFormatterExtensions
    {
        public static Response CreateCssResponse(this IResponseFormatter response, dynamic parameters)
        {
            return response.CreateResponse(Bundle.Css().RenderCached((string)parameters.name), Configuration.Instance.CssMimeType);
        }

        public static Response CreateJavascriptResponse(this IResponseFormatter response, dynamic parameters)
        {
            return response.CreateResponse(Bundle.JavaScript().RenderCached((string)parameters.name), Configuration.Instance.JavascriptMimeType);
        }

        public static Response CreateResponse(this IResponseFormatter response, string content, string contentType)
        {
            return response
                .FromStream(() => new MemoryStream(Encoding.UTF8.GetBytes(content)), contentType)
                .WithHeader("Cache-Control", "max-age=45");
        }
    }
}