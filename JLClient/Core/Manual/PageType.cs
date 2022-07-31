using JLClient.Core.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JLClient.Core.Manual
{
    [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum PageType
    {
        [Description("Word документ")]
        WORDFILE,

        [Description("Word документ с тестом")]
        WORDFILE_WITH_TEST
    }
}
