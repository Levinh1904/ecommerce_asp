using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.ViewModels.Catalog.Producers
{
    public class ProducerCreateRequestValidation: AbstractValidator<ProducerCreateRequest>
    {
        public ProducerCreateRequestValidation()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Tên danh mục không được để trống")
                .MaximumLength(200).WithMessage("Tên danh mục không được vượt quá 200 kí tự");
        }
    }
}
