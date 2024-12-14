using ECommerce.Application.Wrappers;

namespace ECommerce.Application.Features.Products;

public static class ProductErrors
{
    public static Error ProductNotFound => new("Product.NotFound", "Product not found");
    public static Error ProductNameNotEmpty => new("Product.NameNotEmpty", "Product name must not be empty");
    public static Error ProductNotAdded => new("Product.NotAdded", "Product could not be added");
    public static Error ProductNotUpdated => new("Product.NotUpdated", "Product could not be updated");
    public static Error ProductNameMinLength => new("Product.NameMinLength", $"Product name must be at least {ProductConsts.NameMinLength} characters");
    public static Error ProductNameMaxLength => new("Product.NameMaxLength", $"Product name must not exceed {ProductConsts.NameMaxLength} characters");
    public static Error ProductDescriptionMaxLength => new("Product.DescriptionMaxLength", $"Product description must not exceed {ProductConsts.DescriptionMaxLength} characters");
    public static Error ProductPriceMustBeGreaterThanZero => new("Product.PriceMustBeGreaterThanZero", $"Product price must be greater than {ProductConsts.MinPrice}");
    public static Error ProductStockQuantityCannotBeNegative => new("Product.StockQuantityCannotBeNegative", "Product stock quantity cannot be negative");
    public static Error CategoryNotFound => new("Product.CategoryNotFound", "Category not found");
    public static Error ProductStockNotEnough(Guid productId, int stockQuantity) => new("Product.StockNotEnough", $"Product {productId} stock is not enough. Current stock: {stockQuantity}");
}
