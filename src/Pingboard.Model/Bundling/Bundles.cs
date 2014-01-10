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

    public class SquishItFileCollection : List<SquishItFile>, ISquishItFileCollection
    {
        public ISquishItFileCollection AddFiles(bool minify, string basePath, string extension, params string[] fileUrls)
        {
            AddRange(Bundles.BundleFiles(minify, basePath, extension, fileUrls));
            return this;
        }

        public IEnumerable<SquishItFile> ToEnumerable()
        {
            return this.ToList();
        }
    }

    public interface ISquishItFileCollection
    {
        ISquishItFileCollection AddFiles(bool minify, string basePath, string extension, params string[] fileUrls);
        IEnumerable<SquishItFile> ToEnumerable();
    }

    public static class Bundles
    {
        private const string AppRoot = "~/app/";
        private const string ComponentsRoot = AppRoot + "bower_components/";

        public static IEnumerable<SquishItFile> BundleFiles(bool minify, string basePath, string extension, params string[] fileUrls)
        {
            return fileUrls.Select(url => new SquishItFile(string.Concat(basePath, url, extension), minify));
        }

        public static readonly IEnumerable<SquishItFile> CommonScripts = new SquishItFileCollection()
                .AddFiles(true, ComponentsRoot, ".js",
                    "jquery/jquery",
                    "bootstrap/dist/js/bootstrap",
                    "angular/angular",
                    "angular-route/angular-route")
                .AddFiles(true, AppRoot, ".js",
                    "js/controllers/controllers",
                    "js/app")
                .ToEnumerable();

        public static readonly IEnumerable<SquishItFile> CommonStyles = new SquishItFileCollection()
                .AddFiles(true, ComponentsRoot, ".css",
                    "bootstrap/dist/css/bootstrap")
                .AddFiles(true, AppRoot, ".css",
                    "css/main")
                .ToEnumerable();
    }
}