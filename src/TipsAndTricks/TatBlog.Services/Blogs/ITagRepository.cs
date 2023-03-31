using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TatBlog.Core.Constracts;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;

namespace TatBlog.Services.Blogs
{
    public interface ITagRepository
    {
        Task<Tag> GetTagBySlugAsync(
        string slug,
        CancellationToken cancellationToken = default);

        Task<Tag> GetCachedTagBySlugAsync(
            string slug, CancellationToken cancellationToken = default);

        Task<Tag> GetTagByIdAsync(int tag);

        Task<Tag> GetCachedTagByIdAsync(int tagId);

        Task<IList<TagItem>> GetTagsAsync(
            CancellationToken cancellationToken = default);

        Task<IPagedList<TagItem>> GetPagedTagsAsync(
            IPagingParams pagingParams,
            string name = null,
            CancellationToken cancellationToken = default);

        Task<IPagedList<T>> GetPagedTagsAsync<T>(
            Func<IQueryable<Tag>, IQueryable<T>> mapper,
            IPagingParams pagingParams,
            string name = null,
            CancellationToken cancellationToken = default);

        Task<bool> AddOrUpdateTagAsync(
            Tag tag,
            CancellationToken cancellationToken = default);

        Task<bool> DeleteTagAsync(
            int TagId,
            CancellationToken cancellationToken = default);

        Task<bool> IsTagSlugExistedAsync(
            int TagId, string slug,
            CancellationToken cancellationToken = default);
        Task<IList<Tag>> GetPopularTagAsync(
            int numTags, CancellationToken cancellationToken = default);
    }
}
