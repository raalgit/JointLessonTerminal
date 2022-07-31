using JLClient.Core.Manual;
using JLClient.Core.PersistModels;
using JLClient.Core.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JLClient.MVVM.Model.Components.Lesson
{
    public class LessonHandler
    {
        private readonly ManualUtility manualUtility;

        public LessonHandler()
        {
            manualUtility = new ManualUtility();
        }

        public async Task<FullManualData> LoadManualData(int manualId)
        {
            var currentManual = await manualUtility.LoadManualById(manualId);
            var currentManualData = await manualUtility.LoadManualDataByFileId(currentManual.fileDataId);
            var resp = new FullManualData()
            {
                Manual = currentManual,
                ManualData = currentManualData
            };

            return resp;
        }

        public class FullManualData
        {
            public Manual Manual { get; set; }
            public ManualData ManualData { get; set; }
        }
    }
}
