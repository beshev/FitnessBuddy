namespace FitnessBuddy.Web.Infrastructure.Attributes
{
    using System.ComponentModel.DataAnnotations;

    public class NonNegativeNumberAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if ((double)value < 0)
            {
                this.ErrorMessage = $"{validationContext.MemberName} must be a non-negative number!";
                return new ValidationResult(this.ErrorMessage);
            }

            return ValidationResult.Success;
        }
    }
}
