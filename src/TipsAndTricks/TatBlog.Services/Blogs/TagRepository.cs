using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TatBlog.Core.Constracts;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;
using TatBlog.Data.Contexts;
using TatBlog.Services.Extentions;

namespace TatBlog.Services.Blogs
{
    public class TagRepository:ITagRepository
    {
        private readonly BlogDbContext _context;
        private readonly IMemoryCache _memoryCache;

        public TagRepository(BlogDbContext context, IMemoryCache memoryCache)
        {
            _context = context;
            _memoryCache = memoryCache;
        }
        public async Task<Tag> GetTagBySlugAsync(
        string slug, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Tag>()
                .FirstOrDefaultAsync(a => a.UrlSlug == slug, cancellationToken);
        }
        public async Task<Tag> GetCachedTagBySlugAsync(
        string slug, CancellationToken cancellationToken = default)
        {
            return await _memoryCache.GetOrCreateAsync(
                $"tag.by-slug.{slug}",
                async (entry) =>
                {
                    entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30);
                    return await GetTagBySlugAsync(slug, cancellationToken);
                });
        }
        public async Task<Tag> GetTagByIdAsync(int tagId)
        {
            return await _context.Set<Tag>().FindAsync(tagId);
        }
        public async Task<Tag> GetCachedTagByIdAsync(int tagId)
        {
            return await _memoryCache.GetOrCreateAsync(
                $"tag.by-id.{tagId}",
                async (entry) =>
                {
                    entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30);
                    return await GetTagByIdAsync(tagId);
                });
        }
        public async Task<IList<TagItem>> GetTagsAsync(
        CancellationToken cancellationToken = default)
        {
            return await _context.Set<Tag>()
                .OrderBy(a => a.Name)
                .Select(a => new TagItem()
                {
                    Id = a.Id,
                    Name = a.Name,
                    Description = a.Description,
                    UrlSlug = a.UrlSlug,
                    PostCount = a.Posts.Count(p => p.Published)
                })
                .ToListAsync(cancellationToken);
        }
        public async Task<IPagedList<TagItem>> GetPagedTagsAsync(
        IPagingParams pagingParams,
        string name = null,
        CancellationToken cancellationToken = default)
        {
            return await _context.Set<Tag>()
                .AsNoTracking()
                .WhereIf(!string.IsNullOrWhiteSpace(name),
                    x => x.Name.Contains(name))
                .Select(a => new TagItem()
                {
                    Id = a.Id,
                    Name = a.Name,
                    Description = a.Description,
                    UrlSlug = a.UrlSlug,
                    PostCount = a.Posts.Count(p => p.Published)
                })
                .ToPagedListAsync(pagingParams, cancellationToken);
        }
        public async Task<IPagedList<T>> GetPagedTagsAsync<T>(
        Func<IQueryable<Tag>, IQueryable<T>> mapper,
        IPagingParams pagingParams,
        string name = null,
        CancellationToken cancellationToken = default)
        {
            var tagQuery = _context.Set<Tag>().AsNoTracking();

            if (!string.IsNullOrEmpty(name))
            {
                tagQuery = tagQuery.Where(x => x.Name.Contains(name));
            }

            return await mapper(tagQuery)
                .ToPagedListAsync(pagingParams, cancellationToken);
        }
        public async Task<bool> AddOrUpdateTagAsync(
        Tag tag, CancellationToken cancellationToken = default)
        {
            if (tag.Id > 0)
            {
                _context.Tags.Update(tag);
                _memoryCache.Remove($"tag.by-id.{tag.Id}");
            }
            else
            {
                _context.Tags.Add(tag);
            }

            return await _context.SaveChangesAsync(cancellationToken) > 0;
        }
        public async Task<bool> DeleteTagAsync(
        int tagId, CancellationToken cancellationToken = default)
        {
            return await _context.Tags
                .Where(x => x.Id == tagId)
                .ExecuteDeleteAsync(cancellationToken) > 0;
        }
        public async Task<bool> IsTagSlugExistedAsync(
        int tagId,
        string slug,
        CancellationToken cancellationToken = default)
        {
            return await _context.Tags
                .AnyAsync(x => x.Id != tagId && x.UrlSlug == slug, cancellationToken);
        }
        public async Task<IList<Tag>> GetPopularTagAsync(
        int numTags, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Tag>()
                .Include(x => x.Name)
                .Include(x => x.Posts)
                .Include(x => x.Description)
                .Include(p => p.UrlSlug)
                .Take(numTags)
                .ToListAsync(cancellationToken);
        }
    }
}
