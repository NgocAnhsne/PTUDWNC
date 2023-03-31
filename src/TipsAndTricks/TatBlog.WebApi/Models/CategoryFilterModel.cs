namespace TatBlog.WebApi.Models
{
    public class CategoryFilterModel : PagingModel
    {
        public string Name { get; set; }
        public bool ShowMenu { get; set; }
    }
}