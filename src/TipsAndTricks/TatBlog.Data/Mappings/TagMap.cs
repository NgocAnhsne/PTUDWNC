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
    public class TagMap : IEntityTypeConfiguration<Tag>
    {
        public void Configure(EntityTypeBuilder<Tag> builder)
        {
            builder.ToTable("Tags");
            builder.HasKey(a => a.Id);
            builder.Property(a => a.Name)
                .HasMaxLength(50)
                .IsRequired();
            builder.Property(a => a.Description)
                 .HasMaxLength(500);
            builder.Property(a => a.UrlSlug)
                .HasMaxLength(50)
                .IsRequired();
        }
    }
}
