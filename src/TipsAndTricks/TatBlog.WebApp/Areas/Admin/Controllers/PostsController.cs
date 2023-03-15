using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TatBlog.Core.Entities;
using TatBlog.Services.Blogs;
using TatBlog.WebApp.Areas.Admin.Models;

namespace TatBlog.WebApp.Areas.Admin.Controllers
{
    public class PostsController : Controller
    {
        private readonly IBlogRepository _blogRepository;
        private readonly IMapper _mapper;

        public PostsController(IBlogRepository blogRepository, IMapper mapper)
        {
            _blogRepository = blogRepository;
            _mapper = mapper;
        }
        private async Task PopulatePostFilterModelAsync(PostFilterModel model)
        {
            var authors = await _blogRepository.GetAuthorItemsAsync();
            var categories = await _blogRepository.GetCategoryItemsAsync();

            model.AuthorList = authors.Select(a => new SelectListItem()
            {
                Text = a.FullName,
                Value = a.Id.ToString()
            });
            model.CategoryList = categories.Select(c => new SelectListItem()
            {
                Text = c.Name,
                Value = c.Id.ToString()
            });
        }
        private async Task PopulatePostEditModelAsync(PostEditModel model)
        {
            var authors = await _blogRepository.GetAuthorItemsAsync();
            var categories = await _blogRepository.GetCategoryItemsAsync();

            model.AuthorList = authors.Select(a => new SelectListItem()
            {
                Text = a.FullName,
                Value = a.Id.ToString()
            });
            model.CategoryList = categories.Select(c => new SelectListItem()
            {
                Text = c.Name,
                Value = c.Id.ToString()
            });
        }
        public async Task<IActionResult> Index(PostFilterModel model)
        {
            var postQuery = _mapper.Map<PostQuery>(model);

            ViewBag.PostsList = await _blogRepository
                .GetPagedPostsAsync(postQuery, 1, 10);

            await PopulatePostFilterModelAsync(model);

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(PostEditModel model)
        {
            if (!ModelState.IsValid) 
            {
                await PopulatePostEditModelAsync(model);
                return View(model);
            }
            var post = model.Id > 0
                ? await _blogRepository.GetPostByIdAsync(model.Id)
                : null;
            if(post == null)
            {
                post = _mapper.Map<Post>(model);

                post.Id = 0;
                post.PostedDate = DateTime.Now;
            }
            else
            {
                _mapper.Map(model, post);

                post.Category = null;
                post.ModifiedDate = DateTime.Now;
            }

            await _blogRepository.CreateOrUpdatePostAsync(post, model.GetSelectedTags());
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public async Task<IActionResult> VerifyPostSlug(int id, string urlSlug)
        {
            var slugExisted = await _blogRepository.IsPostSlugExistedAsync(id, urlSlug);
            return slugExisted
                ? Json($"Slug '{urlSlug}' đã được sử dụng")
                : Json(true);
        }
    }
}
