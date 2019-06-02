using VirtoCommerce.LiquidThemeEngine.Objects;

namespace VirtoCommerce.LiquidThemeEngine.Converters
{
    using CustomerReviewStorefront = VirtoCommerce.Storefront.Model.CustomerReviews.CustomerReview;

    public static class CustomerReviewStaticConverter
    {
        public static CustomerReview ToShopifyModel(this CustomerReviewStorefront item)
        {
            return new ShopifyModelConverter().ToLiquidCustomerReview(item);
        }
    }

    public partial class ShopifyModelConverter
    {
        public virtual CustomerReview ToLiquidCustomerReview(CustomerReviewStorefront item)
        {
            return new CustomerReview
            {
                AuthorNickname = item.AuthorNickname,
                Content = item.Content,
                CreatedDate = item.CreatedDate,
                IsActive = item.IsActive,
                ProductId = item.ProductId
            };
        }
    }
}
