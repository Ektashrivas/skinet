using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Core.Specification;
using API.Dtos;
using System;
using System.Linq;
using AutoMapper;
using API.Errors;
using Microsoft.AspNetCore.Http;

namespace API.Controllers
{
    
    public class ProductsController : BaseApiController
    {
        private IGenericRepository<Product> _productRepo ;
        private IGenericRepository<ProductBrand> _productBrandRepo ;
        private IGenericRepository<ProductType> _productTypeRepo ;
        public IMapper _mapper ;
        
        public ProductsController(IGenericRepository<Product> productRepo, IGenericRepository<ProductBrand> productBrandRepo,IGenericRepository<ProductType> productTypeRepo, IMapper mapper)
        {
            _mapper = mapper;
            _productTypeRepo = productTypeRepo;
            _productBrandRepo = productBrandRepo;
            _productRepo = productRepo;
     }

        [HttpGet]
       public async Task<ActionResult<IReadOnlyList<ProductToReturnDTO>>> GetProducts()
        {
            var spec= new ProductWithTypesAndBrandsSpecification();
            var products=await _productRepo.ListAsync(spec);
            return Ok(_mapper.Map<IReadOnlyList<ProductToReturnDTO>>(products));
        } 
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        
         public async Task<ActionResult<ProductToReturnDTO>> GetProduct(int id)
        {
            var spec=new ProductWithTypesAndBrandsSpecification(id);
            var product= await _productRepo.GetEntityWithSpec(spec);
            if(product==null)
                 return NotFound(new ApiResponse(404));
              return _mapper.Map<ProductToReturnDTO>(product);
            }
         [HttpGet("brands")]
         public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetBrands()
        {
              return Ok(await _productBrandRepo.ListAllAsync());
        }
         [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetTypes()
        {
            return Ok(await _productTypeRepo.ListAllAsync());
        }
      
    }
}