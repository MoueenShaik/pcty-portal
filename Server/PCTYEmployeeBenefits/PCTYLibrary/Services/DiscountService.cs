using PCTYLibrary.Models;

namespace PCTYLibrary.Services
{
    public interface IDiscountService
    {
        public int GetEligibleDependendsForDiscount(Discount discount, IList<Dependent> dependents);
        public bool GetEligibleEmployeeForDiscount(Discount discount, Employee currentEmployee);

        public Discount GetCurrentDiscount(EmployerConfigurationOptions employerConfigurationOptions);
    }
        
    public class DiscountService : IDiscountService
    {
        public int GetEligibleDependendsForDiscount(Discount discount, IList<Dependent> dependents)
        {
            if (discount.Type == "Name")
            {
                return dependents.Where(a => a.Name.StartsWith(discount.Condition)).Count();
            }

            return 0;
        }

        public bool GetEligibleEmployeeForDiscount(Discount discount, Employee currentEmployee)
        {
            if (discount.Type == "Name")
            {
                return currentEmployee.Name.StartsWith(discount.Condition);
            }

            return false;
        }

        public Discount GetCurrentDiscount(EmployerConfigurationOptions employerConfigurationOptions)
        {
            Discount? discount = null;

            if (!employerConfigurationOptions.IsDiscountsApplicable)
            {
                return discount;
            }

            discount = employerConfigurationOptions?.Discount?.FirstOrDefault(a => a.Type == employerConfigurationOptions?.DiscountType);
            return discount;
        }
    }
}
