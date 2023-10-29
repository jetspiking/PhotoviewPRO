using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoViewPRO.Core
{
    public class PhotoSelect
    {
        private const String _defaultSupportedFormats = "*.PNG;*.JPG;*.JPEG;*.BMP;*.ICO";

        public static PhotoSelectResult? SelectFromDirectory()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            IEnumerable<SupportedExtension>? decoderInfosEnumerable = ImageDecoder.GetDecoders();
            if (decoderInfosEnumerable == null) return null;
            List<SupportedExtension> decoderInfos = decoderInfosEnumerable.ToList();

            String supportedFileFormats = String.Empty;

            for (int i = 0; i < decoderInfos.Count; i++)
            {
                supportedFileFormats += $"*{decoderInfos[i].Name}";
                if (i != decoderInfos.Count - 1) supportedFileFormats += ";";
            }

            openFileDialog.Multiselect = false;
            openFileDialog.Filter = $"Image file|{_defaultSupportedFormats};{supportedFileFormats}";

            if (openFileDialog.ShowDialog() == true)
            {
                PhotoSelectResult photoSelectResult = new();
                photoSelectResult.FilePath = openFileDialog.FileName;

                String? parentDirectory = Path.GetDirectoryName(openFileDialog.FileName);
                if (parentDirectory == null) return null;

                photoSelectResult.ParentDirectory = parentDirectory;

                String supportedFormatsAsString = _defaultSupportedFormats + ";" + supportedFileFormats;
                String[] buildFilterString = supportedFormatsAsString.Replace("*", String.Empty).Replace(".", String.Empty).Split(";");
                photoSelectResult.Extensions = buildFilterString;

                return photoSelectResult;
            }

            return null;
        }
    }
}
