using Microsoft.AspNetCore.Mvc;
using TatBlog.Core.Constracts;
using TatBlog.Services.Blogs;

namespace TatBlog.WebApp.Controllers
{
    public class BlogController: Controller
    {
        private readonly IBlogRepository _blogRepository;
        public BlogController(IBlogRepository blogRepository)
        {
            _blogRepository = blogRepository;
        }
        public async Task<IActionResult> Index(
            [FromQuery(Name = "k")] string keyword = "",
            [FromQuery(Name = "p")] int pageNumber = 1,
            [FromQuery(Name = "ps")] int pageSize = 10)
        {
            var postQuery = new PostQuery()
            {
                PublishedOnly = true,
                KeyWord = keyword
            };
            var postsList = await _blogRepository.GetPagedPostsAsync(postQuery,pageNumber,pageSize);
            ViewBag.PostQuery = postQuery;
            return View(postsList);
        }
        //public async Task<IActionResult> Category(
        //    string slug,
        //    [FromQuery(Name = "p")] int pageNumber = 1,
        //    [FromQuery(Name = "ps")] int pageSize = 5)
        //{

        //    var category = await _blogRepository
        //      .GetCategoryItemsAsync(slug);
        //    var postQuery = new PostQuery()
        //    {
        //        PublishedOnly = true,
        //        CategorySlug = slug,
        //        CategoryName = category.Name
        //    };



        //    IPagingParams pagingParams = CreatePagingParamsPost(pageNumber, pageSize);
        //    var posts = await _blogRepository
        //      .GetPagedPostsAsync(postQuery, pagingParams);


        //    ViewBag.PostQuery = postQuery;
        //    ViewBag.Title = $"Chủ đề {postQuery.CategoryName}";

        //    return View("Index", posts);
        //}
        public IActionResult About() => View();
        public IActionResult Contact() => View();
        public IActionResult Rss() => Content("Nội dung sẽ được cập nhật");
    }
}
