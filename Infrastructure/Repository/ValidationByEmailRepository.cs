using Infrastructure.Data;
using Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public interface IValidationByEmailRepository : IRepository<ValidationByEmail>
    {

    }
    public class ValidationByEmailRepository : Repository<ValidationByEmail>, IValidationByEmailRepository
    {
        public ValidationByEmailRepository(ZaloDbContext context) : base(context)

        {
        }
    }
}
