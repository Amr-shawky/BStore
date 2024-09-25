using BStore.Models.Context;
using BStore.Repository;
using System.ComponentModel.DataAnnotations;

namespace BStore.Models
{
    public class UniqueAttribute:ValidationAttribute
    {

        ISearchUserName Search;
        public UniqueAttribute() {
            Search = new SearchUserName(new BStore_Context());
        }
        
        
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null)
                return null;
            string name = value.ToString()??"";
            if (Search.IsUserNameUnique(name)) { 
            
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult(name);
            }
        }
    }
}
