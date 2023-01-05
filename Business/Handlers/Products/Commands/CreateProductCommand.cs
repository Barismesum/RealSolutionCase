﻿using Business.Constants;
using Core.Entities.Concrete;
using Core.Utilities.Results;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Business.Handlers.Products.Commands
{
    public class CreateProductCommand:IRequest<IResult>
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductColor { get; set; }
        public string ProductSize { get; set; }
        public int CreatedConsumerId { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool isDeleted { get; set; }



        public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, IResult>
        {
            private readonly IProductRepository _productRepository;

            public CreateProductCommandHandler(IProductRepository productRepository)
            {
                _productRepository = productRepository;
            }

            public async Task<IResult> Handle(CreateProductCommand request, CancellationToken cancellationToken)
            {
                var isThereAnyProduct = await _productRepository.GetAsync(p => p.ProductId == request.ProductId);

                if (isThereAnyProduct != null)
                {
                    return new ErrorResult(Messages.ProductAlreadyExist);
                }

                var product = new Product
                {
                    ProductId = request.ProductId,
                    ProductName = request.ProductName,
                    ProductColor = request.ProductColor,
                    ProductSize = request.ProductSize,
                    CreatedConsumerId=request.CreatedConsumerId,
                    CreatedDate = request.CreatedDate,
                    isDeleted = false,

                };
                _productRepository.Add(product);
                await _productRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Added);

            }

        }
    }
}    


 
    

