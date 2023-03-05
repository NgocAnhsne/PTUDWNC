using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TatBlog.Core.Entities;

namespace TatBlog.Data.Mappings
{
    public class PostMap : IEntityTypeConfiguration<Post>
    {
        public void Configure(EntityTypeBuilder<Post> builder)
        {
            builder.ToTable("Posts");
            builder.HasKey(a => a.Id);

            builder.Property(a => a.Title)
                 .HasMaxLength(500)
                 .IsRequired();

            builder.Property(a => a.ShortDescription)
                .HasMaxLength(5000)
                .IsRequired();

            builder.Property(a => a.Description)
                 .HasMaxLength(5000)
                 .IsRequired();

            builder.Property(a => a.UrlSlug)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(a => a.Meta)
                .HasMaxLength(1000)
                .IsRequired();

            builder.Property(a => a.ImageUrl)
                .HasMaxLength(1000);

            builder.Property(a => a.ViewCount)
                .IsRequired()
                .HasDefaultValue(0);

            builder.Property(a => a.Published)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(a => a.PostedDate)
                .HasColumnType("datetime");

            builder.Property(a => a.ModifiedDate)
                .HasColumnType("datetime");

            builder.HasOne(p => p.Category)
                .WithMany(c => c.Posts)
                .HasForeignKey(p => p.CategoryId)
                .HasConstraintName("FK_Posts_Categories")
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(p => p.Author)
                .WithMany(c => c.Posts)
                .HasForeignKey(p => p.AuthorId)
                .HasConstraintName("FK_Posts_Authors")
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(p => p.Tags)
                .WithMany(c => c.Posts)
                .UsingEntity(pt => pt.ToTable("PostTags"));
        }
    }
}
