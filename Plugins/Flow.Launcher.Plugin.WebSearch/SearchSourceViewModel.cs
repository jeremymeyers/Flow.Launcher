using System;
using System.IO;

namespace Flow.Launcher.Plugin.WebSearch
{
    public class SearchSourceViewModel : BaseModel
    {
        public SearchSource SearchSource { get; set; }

        public void UpdateIconPath(SearchSource selectedSearchSource, string fullpathToSelectedImage)
        {
            var parentDirectorySelectedImg = Directory.GetParent(fullpathToSelectedImage).ToString();

            selectedSearchSource.CustomIcon = parentDirectorySelectedImg != Main.DefaultImagesDirectory;

            var iconFileName = Path.GetFileName(fullpathToSelectedImage);
            selectedSearchSource.Icon = iconFileName;
        }

        public void CopyNewImageToUserDataDirectoryIfRequired(
            SearchSource selectedSearchSource, string fullpathToSelectedImage, string fullPathToOriginalImage)
        {
            var destinationFileNameFullPath = Path.Combine(Main.CustomImagesDirectory, Path.GetFileName(fullpathToSelectedImage));

            var parentDirectorySelectedImg = Directory.GetParent(fullpathToSelectedImage).ToString();

            if (parentDirectorySelectedImg != Main.CustomImagesDirectory && parentDirectorySelectedImg != Main.DefaultImagesDirectory)
            {
                try
                {
                    File.Copy(fullpathToSelectedImage, destinationFileNameFullPath);
                }
                catch (Exception e)
                {
#if DEBUG
                    throw e;
#else
                MessageBox.Show(string.Format("Copying the selected image file to {0} has failed, changes will now be reverted", destinationFileNameFullPath));
                UpdateIconPath(selectedSearchSource, fullPathToOriginalImage);
#endif
                }
            }

            selectedSearchSource.NotifyImageChange();
        }

        internal void SetupCustomImagesDirectory()
        {
            if (!Directory.Exists(Main.CustomImagesDirectory))
                Directory.CreateDirectory(Main.CustomImagesDirectory);
        }

        internal bool ShouldProvideHint(string fullPathToSelectedImage)
        {
            return Directory.GetParent(fullPathToSelectedImage).ToString() == Main.DefaultImagesDirectory;
        }
    }
}