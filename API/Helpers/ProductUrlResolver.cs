using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Dtos;
using AutoMapper;
using Core.Entities;
using Microsoft.Extensions.Configuration;

namespace API.Helpers
{
    public class ProductUrlResolver : IValueResolver<Product, ProductToReturnDTO, string>
    {
        public IConfiguration _config { get; }
        public ProductUrlResolver(IConfiguration config)
        {
            _config= config;
        }

        public string Resolve(Product source, ProductToReturnDTO destination, string destMember, ResolutionContext context)
        {
           if(!string.IsNullOrEmpty(source.PictureURL))
           {
              return _config["ApiUrl"] + source.PictureURL;           
            }
            return null;
        }
        }
    }
