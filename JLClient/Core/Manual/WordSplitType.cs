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
    public enum WordSplitType
    {
        [Description("Разбиение по страницам")]
        BY_PAGE,
    }
}
