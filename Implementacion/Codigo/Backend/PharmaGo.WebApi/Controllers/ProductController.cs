﻿using Microsoft.AspNetCore.Mvc;
using PharmaGo.BusinessLogic;
using PharmaGo.Domain.Entities;
using PharmaGo.Domain.SearchCriterias;
using PharmaGo.IBusinessLogic;
using PharmaGo.WebApi.Enums;
using PharmaGo.WebApi.Filters;
using PharmaGo.WebApi.Models.In;
using PharmaGo.WebApi.Models.Out;

namespace PharmaGo.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [TypeFilter(typeof(ExceptionFilter))]
    public class ProductController : Controller
    {
        private readonly IProductManager _productManager;

        public ProductController(IProductManager manager)
        {
            this._productManager = manager;
        }

        [HttpGet]
        public IActionResult GetAll([FromQuery] ProductSearchCriteria productSearchCriteria)
        {
            IEnumerable<Product> products = this._productManager.GetAll(productSearchCriteria);
            IEnumerable<ProductBasicModel> productsToReturn = products.Select(p => new ProductBasicModel(p));
            return this.Ok(productsToReturn);
        }

        [HttpPost]
        [AuthorizationFilter(new string[] { nameof(RoleType.Employee) })]
        public IActionResult Create([FromBody] ProductModel productModel)
        {
            string token = this.HttpContext.Request.Headers["Authorization"];
            Product productCreated = this._productManager.Create(productModel.ToEntity(), token);
            ProductDetailModel productResponse = new ProductDetailModel(productCreated);
            return this.Ok(productResponse);
        }

        [HttpPut("{id}")]
        [AuthorizationFilter(new string[] { nameof(RoleType.Employee) })]
        public IActionResult Update([FromRoute] int id, [FromBody] UpdateProductModel updatedProduct)
        {
            Product product = this._productManager.Update(id, updatedProduct.ToEntity());
            return this.Ok(new ProductDetailModel(product));
        }

        [HttpDelete("{id}")]
        [AuthorizationFilter(new string[] { nameof(RoleType.Employee) })]
        public IActionResult Delete([FromRoute] int id)
        {
            this._productManager.Delete(id);
            return this.Ok(true);
        }

        [HttpGet("{id}")]
        public IActionResult GetById([FromRoute] int id)
        {
            Product product = this._productManager.GetById(id);
            return Ok(new ProductDetailModel(product));
        }

        [HttpGet]
        [Route("[action]")]
        [AuthorizationFilter(new string[] { nameof(RoleType.Employee) })]
        public IActionResult User()
        {
            string token = HttpContext.Request.Headers["Authorization"];
            IEnumerable<Product> product = this._productManager.GetAllByUser(token);
            IEnumerable<ProductBasicModel> productToReturn = product.Select(d => new ProductBasicModel(d));
            return Ok(productToReturn);
        }
    }
}
