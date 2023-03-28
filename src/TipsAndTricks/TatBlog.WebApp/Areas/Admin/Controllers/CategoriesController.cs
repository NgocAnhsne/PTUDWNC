using FluentValidation;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using TatBlog.Core.Entities;
using TatBlog.Services.Blogs;
using TatBlog.Services.Media;
using TatBlog.WebApp.Areas.Admin.Models;
using TatBlog.Core.DTO;

namespace TatBlog.WebApp.Areas.Admin.Controllers
{
    public class CategoriesController:Controller
    {
        private readonly IBlogRepository _blogRepository;
        private readonly IMediaManager _mediaManager;
        private readonly IMapper _mapper;

        public CategoriesController(
            IBlogRepository blogRepository,
            IMediaManager mediaManager,
            IMapper mapper)
        {
            _blogRepository = blogRepository;
            _mediaManager = mediaManager;
            _mapper = mapper;
        }
        //[HttpGet]
        //public async Task<IActionResult> Index(
        //CategoryFilterModel model,
        //[FromQuery(Name = "p")] int pageNumber = 1,
        //[FromQuery(Name = "ps")] int pageSize = 2)
        //{
        //}
  }
}