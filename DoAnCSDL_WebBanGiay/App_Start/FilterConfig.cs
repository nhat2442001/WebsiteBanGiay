using System.Web;
using System.Web.Mvc;

namespace DoAnCSDL_WebBanGiay
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
