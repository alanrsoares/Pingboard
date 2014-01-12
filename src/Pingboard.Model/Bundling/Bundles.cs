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
        private string Extension { get; set; }

        private string BasePath { get; set; }

        private bool Minify { get; set; }

        public ISquishItFileCollection WithBasePath(string basePath)
        {
            BasePath = basePath;
            return this;
        }

        public ISquishItFileCollection WithExtension(string extension)
        {
            Extension = extension;
            return this;
        }

        public ISquishItFileCollection WithMinification(bool minifyFiles)
        {
            Minify = minifyFiles;
            return this;
        }

        public ISquishItFileCollection WithFiles(params string[] filePaths)
        {
            return WithFiles(Minify, BasePath, Extension, filePaths);
        }

        private ISquishItFileCollection WithFiles(bool minify, string basePath, string extension, params string[] filePaths)
        {
            AddRange(Bundles.BundleFiles(minify, basePath, extension, filePaths));
            return this;
        }
    }

    public interface ISquishItFileCollection : IEnumerable<SquishItFile>
    {
        ISquishItFileCollection WithBasePath(string basePath);
        ISquishItFileCollection WithMinification(bool minifyFiles);
        ISquishItFileCollection WithFiles(params string[] filePaths);
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
                .WithExtension(".js")
                .WithMinification(true)
                .WithBasePath(ComponentsRoot)
                .WithFiles(
                    "jquery/jquery",
                    "bootstrap/dist/js/bootstrap",
                    "angular/angular",
                    "angular-route/angular-route")
                .WithBasePath(AppRoot)
                .WithFiles(
                    "js/controllers/controllers",
                    "js/app");

        public static readonly IEnumerable<SquishItFile> CommonStyles = new SquishItFileCollection()
                .WithExtension(".css")
                .WithMinification(true)
                .WithBasePath(ComponentsRoot)
                .WithFiles(
                    "bootstrap/dist/css/bootstrap")
                .WithBasePath(AppRoot)
                .WithFiles("css/main", "css/main", "css/main", "css/main");
    }
}