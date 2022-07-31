using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace JLClient.MVVM.Model.Components.Profile.InnerModels
{
    public class CourseData
    {
        public int Id { get; set; }
        public int ManualId { get; set; }
        public int DisciplineId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public BitmapImage Image { get; set; }
    }
}
