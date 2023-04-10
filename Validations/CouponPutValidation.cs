using FluentValidation;
using MinimalAPI.DTO;

namespace MinimalAPI.Validations
{
    public class CouponPutValidation : AbstractValidator<CouponPutDTO>
    {
        public CouponPutValidation() 
        {
            RuleFor(model => model.Name).NotEmpty();
            RuleFor(model => model.Name).NotEmpty();
            RuleFor(model => model.Percent).InclusiveBetween(1, 100);
        }
    }
}
