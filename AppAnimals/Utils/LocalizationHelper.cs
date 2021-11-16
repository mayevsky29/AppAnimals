using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppAnimals.Utils
{
    // клас дає можливість отримати список мов 
    public static class LocalizationHelper
    {
        // список мов, які підтримуються
        private readonly static IList<string>
            _supportedLocalesList = new List<string> { "en", "uk"};

        public static IList<string> GetSupportedLocales()
        {
            return _supportedLocalesList;
        }

    }
}
