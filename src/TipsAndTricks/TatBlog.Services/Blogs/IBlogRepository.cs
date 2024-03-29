﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TatBlog.Core.Entities;
using TatBlog.Core.DTO;
using TatBlog.Core.Constracts;


namespace TatBlog.Services.Blogs
{
    public interface IBlogRepository
    {
        Task<Post> GetPostAsync(
            int year,
            int month,
            string slug,
            CancellationToken cancellationToken = default);
        Task<Post> GetPostByIdAsync(
        int postId, bool includeDetails = false,
        CancellationToken cancellationToken = default);
        Task<Tag> GetTagAsync(
        string slug, CancellationToken cancellationToken = default);
        Task<IList<Post>> GetPopularArticlesAsync(
            int numPost,
            CancellationToken cancellationToken= default);
        Task<bool> IsPostSlugExistedAsync(
            int postId, string slug,
            CancellationToken cancellationToken=default);
        Task IncreaseViewCountAsync(
            int postId,
            CancellationToken cancellationToken);
        Task<IList<CategoryItem>> GetCategoryItemsAsync(
            bool showOnMenu = false,
            CancellationToken cancellationToken =default);
        Task<IPagedList<TagItem>> GetPagedTagAsync(
            IPagingParams pagingParams,
            CancellationToken cancellationToken = default);
        public Task<Tag> GetTagBySlugAsync(
            string slug, 
            CancellationToken cancellationToken = default);
        Task<IPagedList<Post>> GetPagedPostsAsync(
		        PostQuery condition,
		        int pageNumber = 1,
		        int pageSize = 10,
		        CancellationToken cancellationToken = default);
        Task<IPagedList<T>> GetPagedPostsAsync<T>(
            PostQuery condition,
            IPagingParams pagingParams,
            Func<IQueryable<Post>, IQueryable<T>> mapper);
        Task<IPagedList<CategoryItem>> GetPagedCategoriesAsync(
            IPagingParams pagingParams,
            CancellationToken cancellationToken = default);
        Task<IPagedList<T>> GetPagedCategoriesAsync<T>(
            CategoryQuery query,
            int pageNumber,
            int pageSize,
            Func<IQueryable<Category>, IQueryable<T>> mapper,
            string sortColumn = "Id",
            string sortOrder = "ASC",
            CancellationToken cancellationToken = default);
        Task<Post> CreateOrUpdatePostAsync(
            Post post, IEnumerable<string> tags,
            CancellationToken cancellationToken = default);
        Task<IPagedList<Post>> GetPostByQueryAsync(
            PostQuery query, int pageNumber = 1, int pageSize = 10,
            CancellationToken cancellationToken = default);
        //Task<IPagedList<Post>> GetPostByQueryAsync(
        //    PostQuery query, IPagingParams pagingParams,
        //    CancellationToken cancellationToken = default);
        Task<IPagedList<T>> GetPostByQueryAsync<T>(
            PostQuery query,
            IPagingParams pagingParams,
            Func<IQueryable<Post>, IQueryable<T>> mapper,
            CancellationToken cancellationToken = default);
        Task<Post> GetCachedPostByIdAsync(
            int id, bool published = false,
            CancellationToken cancellationToken = default);
        Task<bool> DeletePostByIdAsync(
            int id, CancellationToken cancellationToken = default);
    }
}