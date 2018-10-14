using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Resources;

namespace Calculator
{
    public class LocalizationManager
    {
        private static ResourceManager rsManager;

        private static readonly string[] supportedCultures = new string[] { "en", "it" };

        private const string DEFAULT_CULTURE = "en";

        private static bool initialized = false;

        /// <summary>
        /// Intialialize LocalizationManager
        /// </summary>
        public static void Init()
        {
            if (!initialized)
            {
                var currentCulture = CultureInfo.CurrentCulture.TwoLetterISOLanguageName;
                if (!supportedCultures.Contains(currentCulture))
                {
                    CultureInfo.CurrentCulture = new CultureInfo(DEFAULT_CULTURE);
                }
                rsManager = new ResourceManager("Calculator.Resources.Generic", Assembly.GetExecutingAssembly());
                initialized = true;
            }            
        }

        /// <summary>
        /// Return localized string othrwise return key as default
        /// </summary>
        /// <param name="key">key of localized string to be found</param>
        /// <returns>Localized string</returns>
        public static string GetString(string key)
        {
            if (!initialized)
            {
                throw new Exception("LocalizationManager not initialized");
            }
            try
            {
                return rsManager.GetString(key, CultureInfo.CurrentCulture) ?? key;                
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return key;
            }

        }
    }
}
