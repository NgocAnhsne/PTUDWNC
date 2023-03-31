using Azure;
using FluentValidation;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using TatBlog.Core.Collections;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;
using TatBlog.Services.Blogs;
using TatBlog.Services.Media;
using TatBlog.WebApi.Extensions;
using TatBlog.WebApi.Filters;
using TatBlog.WebApi.Models;

namespace TatBlog.WebApi.Endpoints
{
    public static class TagEndpoints
    {
        public static WebApplication MapTagEndpoints(
            this WebApplication app)
        {
            var routeGroupBuilder = app.MapGroup("/api/tags");

            routeGroupBuilder.MapGet("/", GetTags)
                         .WithName("GetTags")
                         .Produces<ApiResponse<PaginationResult<TagItem>>>();

            routeGroupBuilder.MapGet("/{id:int}", GetTagDetails)
                         .WithName("GetTagById")
                         .Produces<ApiResponse<TagItem>>();

            routeGroupBuilder.MapGet("/{slug::regex(^[a-z0-9_-]+$)}/posts", GetPostByTagSlug)
                         .WithName("GetPostByTagSlug")
                         .Produces<ApiResponse<PaginationResult<PostDto>>>();

            routeGroupBuilder.MapPost("/", AddTag)
                         .WithName("AddNewTag")
                         .AddEndpointFilter<ValidatorFilter<TagEditModel>>()
                         .RequireAuthorization()
                         .Produces(401)
                         .Produces<ApiResponse<TagItem>>();


            routeGroupBuilder.MapPut("/{id:int}", UpdateTag)
                         .WithName("UpdateTag")
                         .RequireAuthorization()
                         .Produces(401)
                         .Produces<ApiResponse<string>>();

            routeGroupBuilder.MapDelete("/{id:int}", DeleteTag)
                             .WithName("DeleteTag")
                             .RequireAuthorization()
                             .Produces(401)
                             .Produces<ApiResponse<string>>();
            return app;
        }
        private static async Task<IResult> GetTags(
            [AsParameters] TagFilterModel model, 
            ITagRepository tagRepository)
        {
            var tagList = await tagRepository.GetPagedTagsAsync(model, model.Name);

            var paginationResult = new PaginationResult<TagItem>(tagList);

            return Results.Ok(ApiResponse.Success(paginationResult));
        }
        private static async Task<IResult> GetTagDetails(
            int id,
            ITagRepository tagRepository, 
            IMapper mapper)
        {
            var tag = await tagRepository.
                GetCachedTagByIdAsync(id);

            return tag == null 
                ? Results.NotFound($"Không tìm thấy chuyên mục có mã số {id}") 
                : Results.Ok(mapper.Map<TagItem>(tag));
        }
        private static async Task<IResult> GetPostByTagId(
            int id, 
            [AsParameters] PagingModel pagingModel, 
            IBlogRepository blogRepository)
        {
            var postQuery = new PostQuery
            {
                TagId = id,
                PublishedOnly = true
            };

            var postsList = await blogRepository.GetPagedPostsAsync(
                postQuery, pagingModel, 
                posts => posts.ProjectToType<PostDto>());

            var paginationResult = new PaginationResult<PostDto>(postsList);

            return Results.Ok(paginationResult);
        }
        private static async Task<IResult> GetPostByTagSlug(
            [FromRoute] string slug, 
            [AsParameters] PagingModel pagingModel, 
            IBlogRepository blogRepository)
        {
            var postQuery = new PostQuery
            {
                TagSlug = slug,
                PublishedOnly = true
            };

            var postsList = await blogRepository.GetPagedPostsAsync(
                postQuery, pagingModel, 
                posts => posts.ProjectToType<PostDto>());

            var paginationResult = new PaginationResult<PostDto>(postsList);

            return Results.Ok(ApiResponse.Success(paginationResult));
        }
        private static async Task<IResult> AddTag(
            TagEditModel model, 
            IValidator<TagEditModel> validator,
            ITagRepository tagRepository, 
            IMapper mapper)
        {
            if (await tagRepository
                .IsTagSlugExistedAsync(0, model.UrlSlug))
            {
                return Results.Conflict(
                    $"Slug '{model.UrlSlug}' đã được sử dụng");
            }

            var tag = mapper.Map<Tag>(model);
            await tagRepository.AddOrUpdateTagAsync(tag);

            return Results.CreatedAtRoute(
                "GetTagById", new { tag.Id }, 
                mapper.Map<TagItem>(tag));
        }
        private static async Task<IResult> UpdateTag(
            int id, TagEditModel model, 
            IValidator<TagEditModel> validator,
            ITagRepository tagRepository, IMapper mapper)
        {
            if (await tagRepository
                .IsTagSlugExistedAsync(id, model.UrlSlug))
            {
                return Results.Conflict($"Slug '{model.UrlSlug}' đã được sử dụng :D");
            }

            var tag = mapper.Map<Tag>(model);
            tag.Id = id;

            return await tagRepository.AddOrUpdateTagAsync(tag) 
                ? Results.NoContent() 
                : Results.NotFound();
        }
        private static async Task<IResult> DeleteTag(int id, ITagRepository tagRepository)
        {
            return await tagRepository.DeleteTagAsync(id) 
                ? Results.NoContent() : 
                Results.NotFound($"Could not find tag with id = {id}");
        }
    }
}