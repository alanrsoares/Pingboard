using System.Collections.Generic;
using System.Linq;

namespace Pingboard.Model.Bundling
{
    public class SquishItFile
    {
        public string Url { get; set; }

        public bool Minify { get; set; }

        public SquishItFile(string url, bool minify = true)
        {
            Url = url;
            Minify = minify;
        }
    }

    public static class Bundles
    {
        public static IEnumerable<SquishItFile> BundleFiles(bool minify, params string[] fileUrls)
        {
            return fileUrls.Select(url => new SquishItFile(url, minify));
        }

        public static IEnumerable<SquishItFile> CommonJavascript = BundleFiles(true, "~/scripts/jquery-2.0.3.js", "~/scripts/bootstrap.js", "~/scripts/angular.js", "~/app/app.js");

        public static IEnumerable<SquishItFile> CommonCss = BundleFiles(true, "~/content/bootstrap.css", "~/content/bootstrap-theme.css", "~/content/main.css");
    }
}