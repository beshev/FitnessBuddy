namespace FitnessBuddy.Web.Infrastructure.Attributes
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.IO;
    using System.Linq;

    using Microsoft.AspNetCore.Http;

    public class AllowedExtensionsAttribute : ValidationAttribute
    {
        private readonly string[] extensions;

        public AllowedExtensionsAttribute(params string[] extensions)
        {
            this.extensions = extensions;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var file = value as IFormFile;

            if (file != null)
            {
                var extension = Path.GetExtension(file.FileName);
                if (this.extensions.Contains(extension) == false)
                {
                    this.ErrorMessage = $"Allowed extensions: {string.Join(' ', this.extensions)}";

                    return new ValidationResult(this.ErrorMessage);
                }
            }

            return ValidationResult.Success;
        }
    }
}
