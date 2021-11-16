using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AppAnimals.ActionFilters
{
    public class InternationalizationAttribute : ActionFilterAttribute
    {
        private readonly IList<string> _supportedLocales;
        private readonly string _defaultLang;
        public InternationalizationAttribute()
        {

            // отримання списку мов, які підтримуються
            _supportedLocales = Utils.LocalizationHelper.GetSupportedLocales();

            // встановлення локала за замовчуванням
            _defaultLang = _supportedLocales[0];
        }
        /// <summary>
        /// Застосування локалі до поточного потоку
        /// </summary>
        /// <param name="lang">locale name</param>
        private void SetLang(string lang)
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo(lang);
            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(lang);
        }
        // метод відпрацьовує перед віиконанням екшена контролера
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            
            // отримання локалі із значень маршруту
            string lang = (string)filterContext.RouteData.Values["lang"] ?? _defaultLang;

            // Якщо ми не знайшли відповідну культуру - тоді вибераєтьс локаль за замовчуванням
            if (!_supportedLocales.Contains(lang))
                lang = _defaultLang;

            filterContext.HttpContext.Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(lang)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );
            string currentLang = Thread.CurrentThread.CurrentCulture.Name;
            SetLang(lang);
            if (currentLang != lang)
            {
                var request = filterContext.HttpContext.Request;
                var returnUrl = string.IsNullOrEmpty(request.Path) ? "~/" : $"~{request.Path.Value}";
                filterContext.Result = new RedirectResult(returnUrl);
                return;
            }
        }

    }
}
