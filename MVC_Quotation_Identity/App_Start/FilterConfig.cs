using System.Web;
using System.Web.Mvc;

namespace MVC_Quotation_Identity
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
