using FluentValidation;
using TatBlog.WebApi.Models;

namespace TatBlog.WebApi.Validations
{
    public class PostValidator: AbstractValidator<PostEditModel>
    {
        public PostValidator()
        {
            RuleFor(p => p.Title)
            .NotEmpty()
            .WithMessage("Tiêu đề bài viết không được để trống")
            .MaximumLength(500)
            .WithMessage("Tiêu đề dài tối đa 500");

            RuleFor(p => p.ShortDescription)
            .NotEmpty()
            .WithMessage("Giới thiệu về bài viết không được để trống");

            RuleFor(p => p.Description)
            .NotEmpty()
            .WithMessage("Mô tả bài viết không được để trống");

            RuleFor(p => p.Meta)
            .NotEmpty()
            .WithMessage("Meta bài viết không được để trống")
            .MaximumLength(1000)
            .WithMessage("Meta dài tối đa 1000");

            RuleFor(p => p.ShortDescription)
            .NotEmpty()
            .WithMessage("Slug bài viết không được để trống")
            .MaximumLength(1000)
            .WithMessage("Slug dài tối đa 1000");

            RuleFor(p => p.CategoryId)
            .NotEmpty()
            .WithMessage("Bạn phải chọn chủ đề cho bài viết");

            RuleFor(p => p.AuthorId)
            .NotEmpty()
            .WithMessage("Bạn phải chọn tác giả của bài viết");

        }
    }
}
