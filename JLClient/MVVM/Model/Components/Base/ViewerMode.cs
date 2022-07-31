using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JLClient.MVVM.Model.Components.Base
{
    public enum ViewerMode
    {
        /// <summary>
        /// В режиме просмотра доступно переключение страниц без огрничений. 
        /// Доступны 2 кнопки для всех пользователей: вперед и назад
        /// </summary>
        PREVIEW,

        /// <summary>
        /// В режиме полного синхронизированного просмотра доступны кнопки: вперед и назад. 
        /// При переходе на новую страницу происходит выполение запроса на синхронизацию
        /// страницы для занятия
        /// </summary>
        SYNCHRONIZATION_FULL,

        /// <summary>
        /// В режиме ручной синхронизации доступны 3 кнопки: вперед, назад и синхронизация. 
        /// Переходы между страницами осуществляются без синхронизации. При нажатии на кнопку
        /// синхронизации происходит выполнение запроса на синхронизацию страницы для занятия
        /// </summary>
        SYNCHRONIZATION_MANUAL
    }
}
