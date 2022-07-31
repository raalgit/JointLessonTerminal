using JLClient.Core.Utility.InnerModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JLClient.Core.Utility
{
    public class CacheUtility
    {
        private const string cacheDir = "JointLessonCache";

        private const string manualDir = "manual";
        private const string profileDir = "profile";

        private static readonly string cachedManualDirPath;
        private static readonly string cachedProfileDirPath;
        
        static CacheUtility()
        {
            var applicationPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

            string cachedDirPath = Path.Combine(applicationPath, cacheDir);
            if (!Directory.Exists(cachedDirPath)) Directory.CreateDirectory(cachedDirPath);

            cachedManualDirPath = Path.Combine(cachedDirPath, manualDir);
            cachedProfileDirPath = Path.Combine(cachedDirPath, profileDir);

            if (!Directory.Exists(cachedManualDirPath)) Directory.CreateDirectory(cachedManualDirPath);
            if (!Directory.Exists(cachedProfileDirPath)) Directory.CreateDirectory(cachedProfileDirPath);
        }

        public string CacheFile(MemoryStream memStream, string fileName, CacheType type)
        {
            string cachedFilePath = string.Empty;

            switch (type)
            {
                case CacheType.MANUAL_XPS:
                    cachedFilePath = Path.Combine(cachedManualDirPath, fileName);
                    SaveFile(memStream, cachedFilePath);
                    break;

                case CacheType.PROFILE_IMG:
                    cachedFilePath = Path.Combine(cachedProfileDirPath, fileName);
                    SaveFile(memStream, cachedFilePath);
                    break;
            }

            return cachedFilePath;
        }

        public bool TryGetFromCache(string fileName, CacheType type, out string filePath)
        {
            filePath = string.Empty;
            switch (type)
            {
                case CacheType.MANUAL_XPS:
                    filePath = Path.Combine(cachedManualDirPath, fileName);
                    if (File.Exists(filePath)) return true;
                    break;

                case CacheType.PROFILE_IMG:
                    filePath = Path.Combine(cachedProfileDirPath, fileName);
                    if (File.Exists(filePath)) return true;
                    break;
            }

            return false;
        }

        private void SaveFile(MemoryStream memStream, string filePath)
        {
            using (FileStream fStream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write))
            {
                memStream.WriteTo(fStream);
            }
        }
    }
}
