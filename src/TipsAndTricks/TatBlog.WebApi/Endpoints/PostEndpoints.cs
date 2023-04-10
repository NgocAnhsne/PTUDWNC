using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net;
using TatBlog.Core.Collections;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;
using TatBlog.Services.Blogs;
using TatBlog.Services.Extensions;
using TatBlog.Services.Media;
using TatBlog.WebApi.Models;

namespace TatBlog.WebApi.Endpoints
{
    public static class PostEndpoints
    {
        public static WebApplication MapPostEndpoints(this WebApplication app)
        {
            var routeGroupBuilder = app.MapGroup("/api/posts");

            routeGroupBuilder.MapGet("/", GetPosts)
                         .WithName("GetPosts")
                         .Produces<ApiResponse<PaginationResult<PostItem>>>();

            routeGroupBuilder.MapGet("/get-posts-filter", GetFilteredPosts)
            .WithName("GetFilteredPost")
            .Produces<ApiResponse<PostDto>>();

            routeGroupBuilder.MapGet("/get-filter", GetFilter)
            .WithName("GetFilter")
            .Produces<ApiResponse<PostFilterModel>>();

            routeGroupBuilder.MapPost("/", AddPost)
            .WithName("AddNewPost")
            .Accepts<PostEditModel>("multipart/form-data")
            .Produces(401)
            .Produces<ApiResponse<PostItem>>();

            return app;

        }
        private static async Task<IResult> GetPosts(
            [AsParameters] PostFilterModel model, 
            IBlogRepository blogRepository, 
            IMapper mapper)
        {
            var postQuery = mapper.Map<PostQuery>(model);
            var postList = await blogRepository.GetPostByQueryAsync(postQuery, model, post => post.ProjectToType<PostItem>());

            var paginationResult = new PaginationResult<PostItem>(postList);

            return Results.Ok(paginationResult);
        }
        private static async Task<IResult> GetFilter(
            IAuthorRepository authorRepository,
            ICategoryRepository categoryRepository)
        {
            var model = new PostFilterModel()
            {
                AuthorList = (await authorRepository.GetAuthorsAsync())
                .Select(a => new SelectListItem()
                { 
                    Text = a.FullName,
                    Value = a.Id.ToString()
                }),
                CategoryList = (await categoryRepository.GetCategoriesAsync())
                .Select(c => new SelectListItem()
                {
                    Text = c.Name,
                    Value = c.Id.ToString()
                })
            };
            return Results.Ok(ApiResponse.Success(model));
        }
        private static async Task<IResult> GetFilteredPosts(
        [AsParameters] PostFilterModel model,
        [AsParameters] PagingModel pagingModel,
        IBlogRepository blogRepository)
        {
            var postQuery = new PostQuery()
            {
                KeyWord = model.Keyword,

                CategoryId = model.CategoryId,
                AuthorId = model.AuthorId,
                Year = model.Year,
                Month = model.Month,
            };
            var postsList = await blogRepository.GetPagedPostsAsync(
            postQuery, pagingModel, posts =>
           posts.ProjectToType<PostDto>());
            var paginationResult = new PaginationResult<PostDto>(postsList);
            return Results.Ok(ApiResponse.Success(paginationResult));
        }
        private static async Task<IResult> AddPost(
            HttpContext context,
            IBlogRepository blogRepository,
            IMapper mapper,
            IMediaManager mediaManager)
        {
            var model = await PostEditModel.BindAsync(context);
            var slug = model.Title.GenerateSlug();
            if (await blogRepository.IsPostSlugExistedAsync(model.Id, slug))
            {
                return Results.Ok(ApiResponse.Fail(
                HttpStatusCode.Conflict, $"Slug '{slug}' đã được sử dụng cho bài viết khác"));
            }
            var post = model.Id > 0 ? await
                blogRepository.GetPostByIdAsync(model.Id) : null;
            if (post == null)
            {
                post = new Post()
                {
                    PostedDate = DateTime.Now
                };
            }
            post.Title = model.Title;
            post.AuthorId = model.AuthorId;
            post.CategoryId = model.CategoryId;
            post.ShortDescription = model.ShortDescription;
            post.Description = model.Description;
            post.Meta = model.Meta;
            post.Published = model.Published;
            post.ModifiedDate = DateTime.Now;
            post.UrlSlug = model.Title.GenerateSlug();
            if (model.ImageFile?.Length > 0)
            {
                string hostname =
               $"{context.Request.Scheme}://{context.Request.Host}{context.Request.PathBase}/",
                uploadedPath = await
               mediaManager.SaveFileAsync(model.ImageFile.OpenReadStream(),
                model.ImageFile.FileName,
                model.ImageFile.ContentType);
                if (!string.IsNullOrWhiteSpace(uploadedPath))
                {
                    post.ImageUrl = uploadedPath;
                }
            }
            await blogRepository.CreateOrUpdatePostAsync(post,
           model.GetSelectedTags());
            return Results.Ok(ApiResponse.Success(
            mapper.Map<PostItem>(post), HttpStatusCode.Created));
        }
    }
}
