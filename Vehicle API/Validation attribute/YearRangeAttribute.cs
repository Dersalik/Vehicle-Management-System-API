using System.ComponentModel.DataAnnotations;

namespace Vehicle_API.Validation_attribute
{
    public class YearRangeAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            int year;
            if (int.TryParse(value.ToString(), out year))
            {
                int currentYear = DateTime.Now.Year;
                if (year >= 1900 && year <= currentYear)
                {
                    return true;
                }
            }
            return false;
        }

        public override string FormatErrorMessage(string name)
        {
            return $"{name} must be between 1900 and the current year.";
        }
    }

}
