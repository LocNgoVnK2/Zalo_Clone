using System.Text.Json.Serialization;
using Infrastructure.Service;

namespace Zalo_Clone.Models
{
    public class ValidationByEmailModel
    {
        public string Email { get; set; }
        public string ValidationCode {get; set; }
    
        public ValidationType ValidationType { get; set; }
    }
}
