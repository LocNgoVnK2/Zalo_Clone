using Infrastructure.Data;
using Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public interface ISignUpUserRepository : IRepository<SignUpUser>
    {

    }
    public class SignUpUserRepository : Repository<SignUpUser>, ISignUpUserRepository
    {
        public SignUpUserRepository(ZaloDbContext context) : base(context)

        {
        }
    }
}
