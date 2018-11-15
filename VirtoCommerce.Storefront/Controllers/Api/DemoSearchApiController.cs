using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VirtoCommerce.Storefront.AutoRestClients.CatalogModuleApi;
using VirtoCommerce.Storefront.AutoRestClients.CatalogModuleApi.Models;
using VirtoCommerce.Storefront.Infrastructure;

namespace VirtoCommerce.Storefront.Controllers.Api
{
    // storefrontapi/demosearch
    [StorefrontApiRoute("demosearch")]
    public class DemoSearchApiController : Controller
    {
        private readonly ICatalogModuleSearch _searchApi;

        public DemoSearchApiController(ICatalogModuleSearch searchApi)
        {
            _searchApi = searchApi;
        }

        /// <summary>
        /// Test url: responseGroup/ItemLarge
        /// <param name="responseGroup">
        ///     None = 0,
        ///     ItemInfo = 1,
        ///     ItemAssets = 2,
        ///     ItemProperties = 4,
        ///     ItemAssociations = 8,
        ///     ItemEditorialReviews = 16,
        ///     Variations = 32,
        ///     Seo = 64,
        ///     ItemSmall = 83,
        ///     Links = 128,
        ///     Inventory = 256,
        ///     Outlines = 512,
        ///     ReferencedAssociations = 1024,
        ///     ItemMedium = 1119,
        ///     ItemLarge = 2047
        /// </param>
        /// <returns></returns>
        [HttpGet("responseGroup/{responseGroup}")]
        public async Task<ActionResult> ResponseGroup(string responseGroup)
        {
            var searchCriteria = new ProductSearchCriteria
            {
                ResponseGroup = responseGroup,
                Take = 1
            };

            var result = await _searchApi.SearchProductsAsync(searchCriteria);

            return Json(result);
        }
        
        /// <summary>
        /// Test url: priceRange/90/100
        /// Expected result: 2 products including "ASUS ZenFone 2 ZE551ML 16GB Smartphone"
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        [HttpGet("priceRange/{from}/{to}")]
        public async Task<ActionResult> SearchByPriceRange(double from, double to)
        {
            var searchCriteria = new ProductSearchCriteria
            {
                Currency = "USD",
                PriceRange = new NumericRange(from, to)
            };

            var result = await _searchApi.SearchProductsAsync(searchCriteria);

            return Json(result);
        }

        /// <summary>
        /// Test url: date/2015/2016
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns>Products having "startDate" property withing the specified range</returns>
        [HttpGet("date/{from}/{to}")]
        public async Task<ActionResult> SearchByStartDate(int from, int to)
        {
            var searchCriteria = new ProductSearchCriteria
            {
                StartDateFrom = new DateTime(from, 1, 1),
                StartDate = new DateTime(to, 12, 31)
            };

            var result = await _searchApi.SearchProductsAsync(searchCriteria);

            return Json(result);
        }

        /// <summary>
        /// Test url: category/4974648a41df4e6ea67ef2ad76d7bbd4_0d4ad9bab9184d69a6e586effdf9c2ea
        /// Expected result: 6 products from Electronics catalog Cell phones category
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        [HttpGet("category/{path?}")]
        public async Task<ActionResult> SearchInCategory(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                // filter Electronics / Cell phones by default
                path = "4974648a41df4e6ea67ef2ad76d7bbd4/0d4ad9bab9184d69a6e586effdf9c2ea";
            }
            else
            {
                // routing issue workaround
                path = path.Replace('_', '/');
            }

            var searchCriteria = new ProductSearchCriteria
            {
                Take = 5
            };

            // Outline = {catalogId}/{ctegoryId}
            searchCriteria.Outline = path;

            var result = await _searchApi.SearchProductsAsync(searchCriteria);

            return Json(result);
        }

        /// <summary>
        /// Test url: code/ASZF216GBSL,MIL640X4GLWH
        /// Expected result: 2 cell phones
        /// </summary>
        /// <param name="codes"></param>
        /// <returns></returns>
        [HttpGet("code/{codes?}")]
        public async Task<ActionResult> SearchByCode(string codes)
        {
            var searchCriteria = new ProductSearchCriteria
            {
                Terms = new[] { "code:" + (codes ?? "ASZF216GBSL,MIL640X4GLWH") },
                WithHidden = true
            };

            var result = await _searchApi.SearchProductsAsync(searchCriteria);

            return Json(result);
        }

        /// <summary>
        /// Search by keyword reference: https://github.com/VirtoCommerce/vc-module-search
        /// Test URLs:
        ///    Attribute filter
        ///         color:Red,Silver,Gray
        ///         "display%20size":5.5,6                 
        ///
        ///    Range filter
        ///        "display size":(5.5 TO 6]
        ///        "year released":[2015 TO )
        ///
        ///    Price range filter
        ///        price:[TO 100)
        ///        price:[TO 100),[500 TO]
        ///        price_usd:[99 TO 100)
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        [HttpGet("{keyword}")]
        public async Task<ActionResult> SearchByKeyword(string keyword)
        {
            keyword = System.Net.WebUtility.UrlDecode(keyword);

            var searchCriteria = new ProductSearchCriteria
            {
                SearchPhrase = keyword,
                IsFuzzySearch = true
            };

            var result = await _searchApi.SearchProductsAsync(searchCriteria);

            return Json(result);
        }

        /// <summary>
        /// Test url: custom/aB,c
        /// Expected result: products having "name" starting with "A", "B" or "C"
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        [HttpGet("custom/{values}")]
        public async Task<ActionResult> CustomTerm(string values)
        {
            var termValue = string.Join(',', values.Replace(",", "").ToArray());
            var searchCriteria = new ProductSearchCriteria
            {
                Terms = new[] { "starts_with:" + termValue },
                ResponseGroup = "None"
            };

            var result = await _searchApi.SearchProductsAsync(searchCriteria);

            return Json(result);
        }
    }
}
