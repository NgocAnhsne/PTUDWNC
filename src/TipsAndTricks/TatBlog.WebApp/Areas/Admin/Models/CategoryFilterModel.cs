using System.ComponentModel;

namespace TatBlog.WebApp.Areas.Admin.Models
{
    public class CategoryFilterModel
    {
        [DisplayName("Từ khóa")]
        public string Keyword { get; set; }

        [DisplayName("Không hiển thị")]
        public bool NotShowOnMenu { get; set; }
    }
}
