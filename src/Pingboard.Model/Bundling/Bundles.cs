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

        public static SquishItFileCollection CreateNew()
        {
            return new SquishItFileCollection();
        }

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

        public ISquishItFileCollection WithFilesInDirectory(string directory, params string[] filePaths)
        {
            return WithFiles(Minify, BasePath + directory, Extension, filePaths);
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
        ISquishItFileCollection WithFilesInDirectory(string directory, params string[] filePaths);
    }

    public static class Bundles
    {
        private const string AppRoot = "~/app/";
        private const string ComponentsRoot = AppRoot + "lib/";

        public static IEnumerable<SquishItFile> BundleFiles(bool minify, string basePath, string extension, params string[] fileUrls)
        {
            return fileUrls.Select(url => new SquishItFile(string.Concat(basePath, url, extension), minify));
        }

        private static IEnumerable<SquishItFile> _commonScripts;

        public static IEnumerable<SquishItFile> CommonScripts
        {
            get { return _commonScripts ?? (_commonScripts = LoadCommonScripts()); }
        }

        private static IEnumerable<SquishItFile> _commonStyles;

        public static IEnumerable<SquishItFile> CommonStyles
        {
            get { return _commonStyles ?? (_commonStyles = LoadCommonStyles()); }
        }

        private static IEnumerable<SquishItFile> LoadCommonScripts()
        {
            return SquishItFileCollection
                .CreateNew()
                .WithExtension(".js")
                .WithMinification(true)
                .WithBasePath(ComponentsRoot)
                .WithFiles(
                    "jquery/jquery",
                    "bootstrap/dist/js/bootstrap",
                    "angular/angular",
                    "lazy.js/lazy",
                    "angular-route/angular-route",
                    "angular-resource/angular-resource")
                .WithBasePath(AppRoot)
                .WithFiles(
                    "js/services/services",
                    "js/controllers/controllers",
                    "js/app");
        }

        private static IEnumerable<SquishItFile> LoadCommonStyles()
        {
            return SquishItFileCollection
                .CreateNew()
                .WithExtension(".css")
                .WithMinification(true)
                .WithBasePath(ComponentsRoot)
                .WithFilesInDirectory("bootstrap/dist/css/",
                    "bootstrap",
                    "bootstrap-theme")
                .WithBasePath(AppRoot)
                .WithFiles("css/main");
        }
    }
}