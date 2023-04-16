using Microsoft.AspNetCore.Mvc.Rendering;

namespace AppointmentScheduler.Utility
{
	public class Helper
	{
		public static string Admin = "Admin";
		public static string Manager = "Manager";
		public static string Associate = "Associate";

        public static List<SelectListItem> GetRolesForDropDown(bool isAdmin)
        {
            if (isAdmin)
            {
                return new List<SelectListItem>
                {
                    new SelectListItem{Value=Helper.Admin,Text=Helper.Admin}
                };
            }
            else
            {
                return new List<SelectListItem>
                {
                    new SelectListItem{Value=Helper.Manager,Text=Helper.Manager},
                    new SelectListItem{Value=Helper.Associate,Text=Helper.Associate}
                };
            }
        }
    }
}
