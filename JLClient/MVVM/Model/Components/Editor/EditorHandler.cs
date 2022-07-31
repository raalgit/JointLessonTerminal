using JLClient.Core.Manual;
using JLClient.Core.PersistModels;
using JLClient.Core.Utility;
using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows.Forms;
using Page = JLClient.Core.Manual.Page;

namespace JLClient.MVVM.Model.Components.Editor
{
    public class EditorHandler
    {
        private Microsoft.Office.Interop.Word.Application wordApplication;
        private readonly ManualUtility manualUtility;
        private readonly FileUtility fileUtility;

        public EditorHandler()
        {
            manualUtility = new ManualUtility();
            fileUtility = new FileUtility();
        }

        private List<Page> GetPages(ManualData manual)
        {
            return manualUtility.GetPages(manual);
        }

        public ManualData ImportManual()
        {
            using (var fileDialog = new OpenFileDialog())
            {
                fileDialog.DefaultExt = ".jl";
                fileDialog.Filter = "Joint lesson manual|*.jl";
                var result = fileDialog.ShowDialog();
                if (result == DialogResult.OK)
                {
                    using (Stream stream = new FileStream(fileDialog.FileName, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite))
                    {
                        if (File.Exists(fileDialog.FileName) && stream.Length > 0)
                        {
                            string fileContents;
                            using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                            {
                                fileContents = reader.ReadToEnd();
                            }

                            var manualData = JsonSerializer.Deserialize<ManualData>(fileContents);
                            System.Windows.Forms.MessageBox.Show("Материал импортирован", "Готово!");
                            return manualData;
                        }

                        throw new Exception();
                    }
                }

                return null;
            }
        }

        public ManualData ImportManual(string path)
        {
            using (Stream stream = new FileStream(path, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite))
            {
                if (File.Exists(path) && stream.Length > 0)
                {
                    string fileContents;
                    using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                    {
                        fileContents = reader.ReadToEnd();
                    }

                    var manualData = JsonSerializer.Deserialize<ManualData>(fileContents);
                    System.Windows.Forms.MessageBox.Show("Материал импортирован", "Готово!");
                    return manualData;
                }

                throw new Exception();
            }
        }

        private async Task<bool> UploadManualFilesAsync(ManualData manualData)
        {
            var pages = GetPages(manualData);

            if (pages == null || pages.Count == 0) return true;
            foreach (var page in pages)
            {
                if (string.IsNullOrEmpty(page.DirPath) || string.IsNullOrEmpty(page.FileName)) continue;
                string source = Path.Combine(page.DirPath, page.FileName);

                using (var reader = new System.IO.StreamReader(source))
                {
                    using (var memstream = new MemoryStream())
                    {
                        reader.BaseStream.CopyTo(memstream);
                        var bytes = memstream.ToArray();
                        var uploadStatus = await fileUtility.UploadFile(bytes, page.FileName);
                        page.FileDataId = uploadStatus.FileId;
                    }
                }
            }

            return true;
        }

        public async Task<bool> UploadManualAsync(ManualData manualData)
        {
            if (manualData.GetStep() < 2) return false;

            var uploadFilesRes = await UploadManualFilesAsync(manualData);
            if (!uploadFilesRes) return false;

            manualData.SetStep(3);
            var uploadManualRes = await manualUtility.SaveAtDataBase(manualData);
            return uploadManualRes;
        }

        public async Task<bool> UpdateManualAsync(ManualData manualData, int manualId)
        {
            var uploadFilesRes = await UploadManualFilesAsync(manualData);
            if (!uploadFilesRes) return false;

            var uploadManualRes = await manualUtility.UpdateAtDataBase(manualData, manualId);
            return uploadManualRes;
        }

        public async Task<List<Manual>> LoadMyManualsAsync()
        {
            return await manualUtility.GetMyManuals();
        }

        public async Task<ManualData> GetManualDataAsync(Manual manual)
        {
            if (manual == null) throw new ArgumentNullException(nameof(manual));
            return await manualUtility.LoadManualDataByFileId(manual.fileDataId);
        }

        public string ExportManual(ManualData manualData)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();
                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    if (manualData == null) throw new Exception();
                    if (string.IsNullOrEmpty(fbd.SelectedPath)) throw new Exception();

                    manualData.SetStep(2);
                    SaveExportedFiles(manualData, fbd.SelectedPath);
                    string jsonPost = JsonSerializer.Serialize<ManualData>(manualData);
                    var manualName = Path.Combine(fbd.SelectedPath, "Manual.jl");
                    using (var file = File.Open(manualName, FileMode.Create))
                    {
                        file.Seek(0, SeekOrigin.Begin);
                        using (var stream = new StreamWriter(file, Encoding.UTF8))
                            stream.Write(jsonPost);

                        return manualName;
                    }
                }

                return null;
            }
        }

        private void SaveExportedFiles(ManualData manualData, string targetDir)
        {
            if (manualData == null || manualData.ManualRoot == null || manualData.ManualRoot.Length == 0) 
                return;
            
            InitOfficeApplication();
            SaveFilesForNode(manualData.ManualRoot.First(), targetDir);
            CloseOfficeApplication();
        }

        private void SaveFilesForNode(ManualNode node, string targetDir)
        {
            if (node.BlockType == BlockType.BLOCK_WITH_NODES)
            {
                if (node.Children != null && node.Children.Count > 0)
                {
                    foreach (var child in node.Children)
                    {
                        SaveFilesForNode(child, targetDir);
                    }
                }
            }
            else if (node.BlockType == BlockType.BLOCK_WITH_PAGES)
            {
                if (node.Pages != null && node.Pages.Count > 0)
                {
                    foreach (var page in node.Pages)
                    {
                        ReplaceFileForPage(page, targetDir);
                    }
                }
            }
        }

        private void ReplaceFileForPage(Page page, string targetDir)
        {
            if (!string.IsNullOrEmpty(page.FileName) && !string.IsNullOrEmpty(page.DirPath))
            {
                string wordFullPath = Path.Combine(page.DirPath, page.FileName);
                string xpsName = Path.ChangeExtension(page.FileName, "xps");
                var xpsFullPath = Path.Combine(targetDir, xpsName);

                if (!File.Exists(xpsFullPath))
                {
                    try
                    {
                        SaveXPSDoc(wordFullPath, xpsFullPath, page.OptimizeMemory);
                    }
                    catch (Exception er)
                    {
                        System.Windows.Forms.MessageBox.Show(er.Message,
                            "Error",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                        return;
                    }
                }

                page.DirPath = targetDir;
                page.FileName = xpsName;
                page.PageTitle = xpsName;
            }
        }

        private void InitOfficeApplication()
        {
            try
            {
                wordApplication = new Microsoft.Office.Interop.Word.Application();
            }
            catch (Exception er)
            {
                System.Windows.Forms.MessageBox.Show(er.Message,
                            "Error",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                return;
            }
        }

        private void CloseOfficeApplication()
        {
            if (wordApplication != null)
            {
                wordApplication.Quit();
            }
        }

        private void RemoveResourcesFromXps(string xpsFileName)
        {
            var partsForDeleting = new List<Uri>();
            using (var pack = ZipPackage.Open(xpsFileName, FileMode.Open, FileAccess.ReadWrite))
            {
                foreach (var part in pack.GetParts())
                {
                    if (part.Uri.OriginalString.ToLower().Contains("res"))
                    {
                        partsForDeleting.Add(part.Uri);
                    }
                }

                foreach (var part in partsForDeleting)
                {
                    pack.DeletePart(part);
                }
            }
        }

        private void SaveXPSDoc(string wordDocPath, string xpsDocPath, bool optimize = false)
        {
            try
            {
                if (wordApplication == null) throw new Exception();

                wordApplication.Documents.Add(wordDocPath);
                Document doc = wordApplication.ActiveDocument;
                doc.SaveAs(FileName: xpsDocPath, FileFormat: WdExportFormat.wdExportFormatXPS);

                wordApplication.Documents.Close(WdSaveOptions.wdDoNotSaveChanges);
                if (optimize)  RemoveResourcesFromXps(xpsDocPath);
            }
            catch (Exception er)
            {
                throw er;
            }
        }

        private IEnumerable<Range> SplitByPage(Document doc)
        {
            // Получение количества страниц
            int pageCount = (int)doc.Range().Information[WdInformation.wdNumberOfPagesInDocument];
            
            // Начало документа
            int pageStart = 0;

            for (int currentPageIndex = 1; currentPageIndex <= pageCount; currentPageIndex++)
            {
                // Получение Range очередной сраницы
                var currentPageRange = doc.Range(pageStart);
                if (currentPageIndex < pageCount)
                    currentPageRange.End = currentPageRange.GoTo(
                    What: WdGoToItem.wdGoToPage,
                    Which: WdGoToDirection.wdGoToAbsolute,
                    Count: currentPageIndex + 1
                    ).Start - 1;
                else
                    currentPageRange.End = doc.Range().End;

                pageStart = currentPageRange.End + 1;
                yield return currentPageRange;
            }
        }

        public ManualData AutoGenerateManual(WordSplitType wordSplit)
        {
            using (var fileDialog = new OpenFileDialog())
            {
                var manual = new ManualData();
                manual.Init();

                fileDialog.DefaultExt = ".jl";
                fileDialog.Filter = "Word Documents (.docx)|*.docx";
                var result = fileDialog.ShowDialog();
                if (result == DialogResult.OK)
                {
                    try
                    {
                        wordApplication = new Microsoft.Office.Interop.Word.Application();
                        IEnumerable<Range> batches = null;
                        object missing = System.Reflection.Missing.Value;
                        Document doc = wordApplication.Documents.Open(fileDialog.FileName);

                        switch (wordSplit)
                        {
                            case WordSplitType.BY_PAGE:
                                batches = SplitByPage(doc);
                                break;
                        }

                        if (batches == null) throw new Exception();
                        var sourceDir = Path.GetDirectoryName(fileDialog.FileName);
                        var targetDir = Path.Combine(sourceDir, "Промежуточные_файлы_материала");

                        if (Directory.Exists(targetDir)) Directory.Delete(targetDir);
                        Directory.CreateDirectory(targetDir);

                        foreach (var batch in batches)
                        {
                            batch.Copy();
                            var pagedDocument = wordApplication.Documents.Add(ref missing, ref missing, ref missing, ref missing);
                            pagedDocument.Range().Paste();
                            
                            string newDocName = Path.ChangeExtension(Guid.NewGuid().ToString(), ".docx");  
                            string newDocPath = Path.Combine(targetDir, newDocName);
                            pagedDocument.SaveAs2(newDocPath, WdSaveFormat.wdFormatXMLDocument, CompatibilityMode: WdCompatibilityMode.wdWord2013);
                            manual.ManualRoot[0].AddPage(targetDir, newDocName);
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        wordApplication.Quit();
                    }
                }

                return manual;
            }
        }
    }
}
