﻿namespace TatBlog.WebApi.Models
{
    public class PostDto
    {
        // Mã bài viết
        public int Id { get; set; }

        // Tiêu đề bài viết
        public string Title { get; set; }

        // Mô tả hay giới thiệu ngắn về nội dung
        public string ShortDescription { get; set; }

        // Tên định danh để tạo URL
        public string UrlSlug { get; set; }

        // Đường dấn đến tập tinh hình ảnh
        public string ImageUrl { get; set; }

        // Số lượt xem, đọc bài viết
        public int ViewCount { get; set; }

        // Ngày giờ đăng bài
        public DateTime PostedDate { get; set; }

        // Ngày giờ cập nhập lần cuối
        public DateTime? ModifiedDate { get; set; }

        // Chuyên mục của bài viết
        public CategoryDto Category { get; set; }

        // Tác giả của bài viết
        public AuthorDto Author { get; set; }

        // Danh sách các từ khóa của bài viết
        public IList<TagDto> Tags { get; set; }
        public bool Published { get; set; }
    }
}