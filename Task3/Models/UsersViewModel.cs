using Microsoft.AspNetCore.Mvc.Rendering;
using Task3.Areas.Identity.Data;

namespace Task3.Models
{
    public class UsersViewModel
    {
        public SelectListItem? IsSelected { get; set; }

        public String? Id { get; set; }

        public string? FullName { get; set; }

        public string? Email { get; set; }

        public DateTime RegistrationDate { get; set; }

        public DateTime LoginDate { get; set; }

        public Status Status { get; set; }
    }
}
