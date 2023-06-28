using Infrastructure.Data;
using Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
        public interface IToDoListRepository : IRepository<ToDoList>
        {

        }
        public class ToDoListRepository : Repository<ToDoList>, IToDoListRepository
        {
            public ToDoListRepository(ZaloDbContext context) : base(context)

            {
            }
        }

    public interface IToDoUserRepository : IRepository<ToDoUser>
    {

    }
    public class ToDoUserRepository : Repository<ToDoUser>, IToDoUserRepository
    {
        public ToDoUserRepository(ZaloDbContext context) : base(context)

        {
        }
    }
}
