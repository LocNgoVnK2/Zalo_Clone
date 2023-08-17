using Infrastructure.Data;
using Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public interface ISearchLogRepository : IRepository<SearchLog>
    {

    }
    public class SearchLogRepository : Repository<SearchLog>, ISearchLogRepository
    {
        public SearchLogRepository(ZaloDbContext context) : base(context)

        {
        }
    }

}
