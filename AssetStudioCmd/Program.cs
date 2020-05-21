//using CommandLine;
using System.IO;
using System.Collections.Generic;
using AssetStudio;
using System.Drawing.Imaging;
using System.ComponentModel.DataAnnotations;
using McMaster.Extensions.CommandLineUtils;

namespace AssetStudioCmd
{
    class Program
    {
        [Argument(0)]
        [Required]
        public string InputPath { get; set; }

        [Argument(1)]
        [Required]
        public string OutputPath { get; set; }

        [Option(CommandOptionType.MultipleValue, Description ="Set the type would be extracted")]
        public IEnumerable<int> Type { get; set; }

        static void Main(string[] args) => CommandLineApplication.Execute<Program>(args);

        private void OnExecute()
        {
            var assetsManager = new AssetsManager();
            if (InputPath.EndsWith(".apk"))
            {
                assetsManager.LoadAPK(this.InputPath);
            }
            else
            {
                assetsManager.LoadFolder(this.InputPath);
            }

            System.Console.WriteLine("Load {0}", InputPath);

            foreach (var assetFile in assetsManager.assetsFileList)
            {
                foreach (var asset in assetFile.Objects)
                {
                    if (asset.type == ClassIDType.Sprite ||
                    asset.type == ClassIDType.SpriteAtlas)
                    {
                        var sprite = (NamedObject)asset;
                        var name = sprite.m_Name;
                        var exportFullName = Path.Combine(this.OutputPath , name + ".png");
                        try
                        {
                            var bitmap = (sprite as Sprite).GetImage();
                            bitmap.Save(exportFullName, ImageFormat.Png);
                            bitmap.Dispose();
                        }
                        catch
                        {
                            System.Console.WriteLine("catch exception.");
                        }
                    }
                }
            }
        }
    }
}
