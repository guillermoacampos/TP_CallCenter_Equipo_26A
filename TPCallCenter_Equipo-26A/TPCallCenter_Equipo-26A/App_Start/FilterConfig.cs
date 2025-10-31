using System.Web;
using System.Web.Mvc;

namespace TPCallCenter_Equipo_26A
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
