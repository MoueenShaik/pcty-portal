namespace PCTYLibrary
{
    public class Discount
    {
        public string Type { get; set; }
        public string Condition { get; set; }
        public int DiscountPercentage { get; set; }
    }

    public class EmployerConfigurationOptions
    {
        public int EmployeeCostOfBenefits { get; set; }
        public int DependentCostOfBenefits { get; set; }
        public bool IsDiscountsApplicable { get; set; }
        public string DiscountType { get; set; }
        public int EmployeeSalaryPerPayCheck { get; set; }
        public int TotalPayChecksPerYear { get; set; }
        public List<Discount> Discount { get; set; }
    }

    public class EmployeeCosts
    {
        public int TotalCostOfBenefitsPerYear { get; set; }
        public int SalaryPerPayCheck { get; set; }
        public int SalaryAfterBenefitsDeductionPerPayCheck { get; set; }
    }
}