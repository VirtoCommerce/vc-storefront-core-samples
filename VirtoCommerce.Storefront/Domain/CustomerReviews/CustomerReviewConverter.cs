using VirtoCommerce.Storefront.Model.CustomerReviews;

namespace VirtoCommerce.Storefront.Domain.CustomerReviews
{
    using CustomerReviewDTO = AutoRestClients.External.CustomerReviewsModuleModuleApi.Models.CustomerReview;
    using CustomerReviewSearchCriteriaDTO = AutoRestClients.External.CustomerReviewsModuleModuleApi.Models.CustomerReviewSearchCriteria;

    public static partial class CustomerReviewConverter
    {
        public static CustomerReview ToCustomerReview(this CustomerReviewDTO itemDto)
        {
            var result = new CustomerReview
            {
                Id = itemDto.Id,
                AuthorNickname = itemDto.AuthorNickname,
                Content = itemDto.Content,
                CreatedBy = itemDto.CreatedBy,
                CreatedDate = itemDto.CreatedDate,
                IsActive = itemDto.IsActive,
                ModifiedBy = itemDto.ModifiedBy,
                ModifiedDate = itemDto.ModifiedDate,
                ProductId = itemDto.ProductId
            };

            return result;
        }

        public static CustomerReviewSearchCriteriaDTO ToSearchCriteriaDto(this CustomerReviewSearchCriteria criteria)
        {
            var result = new CustomerReviewSearchCriteriaDTO
            {
                IsActive = criteria.IsActive,
                ProductIds = criteria.ProductIds,

                Skip = criteria.Start,
                Take = criteria.PageSize,
                Sort = criteria.Sort
            };

            return result;
        }
    }
}
