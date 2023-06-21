using Infrastructure.Data;
using Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public interface IMessageReceipentRepository : IRepository<MessageReceipent>
    {

    }
    public interface IMessageRepository : IRepository<Message>
    {

    }
    public class MessageRepository : Repository<Message>, IMessageRepository
    {
        public MessageRepository(ZaloDbContext context) : base(context)

        {
        }
    }
    public class MessageReceipentRepository : Repository<MessageReceipent>, IMessageReceipentRepository
    {
        public MessageReceipentRepository(ZaloDbContext context) : base(context)

        {
        }
    }
}
