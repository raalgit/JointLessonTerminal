using JLClient.Core.ApiModels.Request.Editor;
using JLClient.Core.ApiModels.Response.Editor;
using JLClient.Core.Http;
using JLClient.Core.Manual;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace JLClient.Core.Utility
{
    public class ManualUtility
    {
        public List<Page> GetPages(ManualData manual)
        {
            var pages = new List<Page>();
            if (manual == null ||
                manual.ManualRoot == null ||
                manual.ManualRoot.Length < 1)
                return pages;

            return GetNodePages(manual.ManualRoot[0]);
        }

        public async Task<bool> SaveAtDataBase(ManualData manual)
        {
            if (manual == null) throw new NullReferenceException(nameof(manual));

            if (manual.MaterialDate == null) manual.MaterialDate = new MaterialDate();
            manual.MaterialDate.created = DateTime.Now;
            manual.MaterialDate.modified = DateTime.Now;

            if (manual.Author == null) manual.Author = new Author();
            if (string.IsNullOrEmpty(manual.Author.name))
            {
                manual.Author.name = "Неизвестный редактор";
            }

            var manualContentAsBytes = JsonSerializer.SerializeToUtf8Bytes<ManualData>(manual);
            var manualSaveRequest = new RequestModel<NewMaterialRequest>()
            {
                Method = RequestMethod.Post,
                Body = new NewMaterialRequest()
                {
                    ManualData = manualContentAsBytes,
                    OriginalName = manual.Name
                },
            };

            var sender = new RequestSender<NewMaterialRequest, NewMaterialResponse>();
            try
            {
                var responsePost = await sender.SendRequest(manualSaveRequest, "/editor/material");
                return responsePost.isSuccess;
            }
            catch (Exception er)
            {
                return false;
            }
        }

        public async Task<bool> UpdateAtDataBase(ManualData manual, int originalFileId)
        {
            if (manual == null) throw new NullReferenceException(nameof(manual));

            manual.MaterialDate.modified = DateTime.Now;
            var manualJsonBuffer = JsonSerializer.SerializeToUtf8Bytes<ManualData>(manual);

            var manualUpdateRequest = new RequestModel<UpdateMaterialRequest>()
            {
                Method = RequestMethod.Put,
                Body = new UpdateMaterialRequest()
                {
                    OriginalName = manual.Name,
                    OriginalFileDataId = originalFileId,
                    ManualData = manualJsonBuffer
                },
            };

            var sender = new RequestSender<UpdateMaterialRequest, UpdateMaterialResponse>();
            try
            {
                var responsePost = await sender.SendRequest(manualUpdateRequest, "/editor/material");
                return responsePost.isSuccess;
            }
            catch (Exception er)
            {
                return false;
            }
        }

        public async Task<List<PersistModels.Manual>> GetMyManuals()
        {
            var manualGetMyMaterialsRequest = new RequestModel<object>()
            {
                Method = RequestMethod.Get
            };

            var sender = new RequestSender<object, GetMyMaterialsResponse>();
            try
            {
                var responsePost = await sender.SendRequest(manualGetMyMaterialsRequest, "/editor/my-materials");
                return responsePost.manuals;
            }
            catch (Exception er)
            {
                return new List<PersistModels.Manual>();
            }
        }

        public async Task<PersistModels.Manual> LoadManualById(int id)
        {
            var getManualRequest = new RequestModel<object>()
            {
                Method = RequestMethod.Get,
                UrlFilter = $"/{id}"
            };

            var sender = new RequestSender<object, GetCourseManualResponse>();
            try
            {
                var responsePost = await sender.SendRequest(getManualRequest, "/editor/course-material");
                return responsePost.manual;
            }
            catch (Exception er)
            {
                return new PersistModels.Manual();
            }
        }

        public async Task<ManualData> LoadManualDataByFileId(int fileId)
        {
            var manualGetRequest = new RequestModel<object>()
            {
                Method = RequestMethod.Get,
                UrlFilter = $"/{fileId}"
            };

            var sender = new RequestSender<object, GetMaterialResponse>();
            try
            {
                var responsePost = await sender.SendRequest(manualGetRequest, "/editor/material");
                if (!responsePost.isSuccess) throw new Exception(responsePost.message);

                using (Stream stream = new MemoryStream(responsePost.manualData))
                {
                    try
                    {
                        var manual = await JsonSerializer.DeserializeAsync<ManualData>(stream);
                        return manual;
                    }
                    catch (Exception ex)
                    {
                        return new ManualData();
                    }
                }
            }
            catch (Exception er)
            {
                return new ManualData();
            }
        }

        private List<Page> GetNodePages(ManualNode node)
        {
            var pages = new List<Page>();
            if (node == null) return pages;


            switch (node.BlockType)
            {
                case BlockType.BLOCK_WITH_PAGES:
                    return node.Pages.ToList();

                case BlockType.BLOCK_WITH_NODES:
                    foreach (var child in node.Children)
                    {
                        var pagesOfChildNode = GetNodePages(child);
                        pages.AddRange(pagesOfChildNode);
                    }

                    return pages;
                default:
                    throw new InvalidOperationException("Неверный тип блока");
            }
        }
    }
}
