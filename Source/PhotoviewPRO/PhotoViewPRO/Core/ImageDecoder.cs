using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace PhotoViewPRO.Core
{
    public class ImageDecoder
    {
        private const string WICDecoderCategory = "{7ED96837-96F0-4812-B211-F13C24117ED3}";

        public static IEnumerable<SupportedExtension>? GetDecoders()
        {
            IEnumerable<SupportedExtension>? decoders32 = GetDecodersByBaseKeyPath("CLSID") ?? Enumerable.Empty<SupportedExtension>();
            IEnumerable<SupportedExtension>? decoders64 = Enumerable.Empty<SupportedExtension>();

            if (Environment.Is64BitOperatingSystem && !Environment.Is64BitProcess)
            {
                decoders64 = GetDecodersByBaseKeyPath("Wow6432Node\\CLSID") ?? Enumerable.Empty<SupportedExtension>();
            }

            return decoders32.Concat(decoders64);
        }

        public static IEnumerable<SupportedExtension>? GetDecodersByBaseKeyPath(String baseKeyPath)
        {
            List<SupportedExtension> decoderInfos = new List<SupportedExtension>();

            RegistryKey? baseKey = Registry.ClassesRoot.OpenSubKey(baseKeyPath, false);
            if (baseKey == null) return null;

            RegistryKey? categoryKey = baseKey.OpenSubKey(WICDecoderCategory + "\\instance", false);
            if (categoryKey == null) return null;

            String[] codecIds = categoryKey.GetSubKeyNames();
            foreach (String codecId in codecIds)
            {
                RegistryKey? codecKey = baseKey.OpenSubKey(codecId);
                if (codecKey != null)
                {
                    String? extensions = Convert.ToString(codecKey.GetValue("FileExtensions", ""));
                    if (extensions == null) continue;

                    foreach (String extension in extensions.Split(','))
                    {
                        SupportedExtension supportedExtension = new SupportedExtension();
                        supportedExtension.Name = extension;
                        decoderInfos.Add(supportedExtension);
                    }
                }
            }
            return decoderInfos;
        }
    }
}
