using JLClient.Core.ApiModels.Response.User;
using JLClient.Core.Manual;
using JLClient.Core.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Threading;
using System.Windows.Xps.Packaging;

namespace JLClient.MVVM.Model.Components.Base
{
    public class PdfViewerHandler
    {
        private readonly string applicationPath;
        private const string cacheDir = "cache";
        private const string xmsExtension = ".xps";
        private readonly ManualUtility manualUtility;
        private readonly FileUtility fileUtility;
        private readonly CacheUtility cacheUtility;

        public PdfViewerHandler()
        {
            manualUtility = new ManualUtility();
            fileUtility = new FileUtility();
            cacheUtility = new CacheUtility();
            applicationPath = AppDomain.CurrentDomain.BaseDirectory;
        }

        public List<Page> GetPages(ManualData manual)
        {
            return manualUtility.GetPages(manual);
        }

        /// <summary>
        /// Проверка на наличие файла на локальном устройстве.
        /// Путь к файлу берется из значений DirName/FileName страницы
        /// </summary>
        /// <param name="page">Страница</param>
        /// <param name="pagePath">Полученный путь к файлу</param>
        /// <returns>True если файл существует. False если файла не существует</returns>
        public bool TryGetPageFilePath(Page page, out string pagePath)
        {
            pagePath = null;

            // Проверка на наличие в странице пути к файлу
            if (page == null) return false;
            if (string.IsNullOrEmpty(page.DirPath) || string.IsNullOrEmpty(page.FileName)) return false;

            // Проверка файла на расширение и существование на локальном устройстве
            string fullPath = Path.Combine(page.DirPath, page.FileName);
            if (!File.Exists(fullPath)) return false;
            if (Path.GetExtension(fullPath).ToLower() != xmsExtension) return false;

            pagePath = fullPath;
            return true;
        }

        /// <summary>
        /// Проверка на наличие файла в кэше на локальном устройстве.
        /// Путь к файлу берется из значений FileName страницы и пути к кэш директории
        /// </summary>
        /// <param name="page">страница</param>
        /// <param name="pagePath">Полученный путь к файлу</param>
        /// <returns>True если файл существует. False если файла не существует</returns>
        public bool TryGetPageFilePathFromCache(Page page, out string pagePath)
        {
            return cacheUtility.TryGetFromCache(page.FileName, Core.Utility.InnerModels.CacheType.MANUAL_XPS, out pagePath);
        }

        public async Task<string> DownloadFileWithCachingAndGetPathAsync(Page page)
        {
            var response = await fileUtility.DownloadFile(page.FileDataId);
            if (!response.isSuccess) throw new Exception(response.message);

            var memStream = new MemoryStream(response.file);
            var cachedFilePath = cacheUtility.CacheFile(memStream, page.FileName, Core.Utility.InnerModels.CacheType.MANUAL_XPS);

            return cachedFilePath;
        }

        public bool TryGetFixedDoc(string xpsFilePath, out FixedDocumentSequence doc)
        {
            try 
            {
                doc = GetFixedDoc(xpsFilePath);
                return true;
            }
            catch (Exception ex)
            {
                doc = null;
                return false;
            }
        }

        public bool TryGetNextPage(List<Page> pages, Page currentPage, out Page nextPage)
        {
            nextPage = null;
            var currentPageIndex = pages.IndexOf(currentPage);
            if (currentPageIndex == -1) return false;
            if (currentPageIndex == pages.Count - 1) return false;

            nextPage = pages[++currentPageIndex];
            return true;
        }

        public bool TryGetPrevPage(List<Page> pages, Page currentPage, out Page nextPage)
        {
            nextPage = null;
            var currentPageIndex = pages.IndexOf(currentPage);
            if (currentPageIndex == -1) return false;
            if (currentPageIndex <= 0) return false;

            nextPage = pages[--currentPageIndex];
            return true;
        }

        public FixedDocumentSequence GetFixedDoc(string xpsFilePath)
        {
            try
            {
                var fixedDoc = new FixedDocumentSequence();
                var xpsPackage = new XpsDocument(xpsFilePath, FileAccess.Read);
                fixedDoc = xpsPackage.GetFixedDocumentSequence();
                return fixedDoc;
            }
            catch (Exception ex)
            { 
                throw new Exception(); 
            }
        }
    }
}
