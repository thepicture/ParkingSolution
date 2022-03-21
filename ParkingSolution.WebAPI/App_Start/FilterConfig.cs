using ParkingSolution.WebAPI.Models.Security;
using System.Web.Mvc;

namespace ParkingSolution.WebAPI
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
