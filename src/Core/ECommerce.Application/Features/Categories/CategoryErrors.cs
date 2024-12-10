
using ECommerce.Application.Wrappers;

namespace ECommerce.Application.Features.Categories;

public static class CategoryErrors
{
    public static Error CategoryNotAdded => new("Category.CategoryNotAdded", "Category not added");
    public static Error CategoryNotFound => new("Category.CategoryNotFound", "Category not found");
    public static Error CategoryAlreadyExists => new("Category.CategoryAlreadyExists", "Category already exists");
    public static Error CategoryNameMustBeUnique => new("Category.CategoryNameMustBeUnique", "Category name must be unique");
    public static Error CategoryNotUpdated => new("Category.CategoryNotUpdated", "Category not updated");
    public static Error CategoryNameMinLength => new("Category.CategoryNameMinLength", $"Category name at least {CategoryConsts.NameMinLength} characters.");
    public static Error CategoryNameMaxLength => new("Category.CategoryNameMaxLength", $"Category name must be less than {CategoryConsts.NameMaxLength} characters.");
}
