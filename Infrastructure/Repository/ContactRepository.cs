using Infrastructure.Data;
using Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public interface IContactRepository : IRepository<Contact>
    {

    }
    public class ContactRepository : Repository<Contact>, IContactRepository
    {
        public ContactRepository(ZaloDbContext context) : base(context)

        {
        }
    }
}
