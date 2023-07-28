using Infrastructure.Data;
using Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public interface IMessageReactDetailRepository : IRepository<MessageReactDetail>
    {

    }
    public interface IMessageAttachmentRepository : IRepository<MessageAttachment>
    {

    }
    public interface IMessageContactRepository : IRepository<MessageContact>
    {

    }
    public interface IMessageToDoListRepository : IRepository<MessageToDoList>
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

    public class MessageAttachmentRepository : Repository<MessageAttachment>, IMessageAttachmentRepository
    {
        public MessageAttachmentRepository(ZaloDbContext context) : base(context)

        {
        }
    }
    public class MessageContactRepository : Repository<MessageContact>, IMessageContactRepository
    {
        public MessageContactRepository(ZaloDbContext context) : base(context)

        {
        }
    }
    public class MessageToDoListRepository : Repository<MessageToDoList>, IMessageToDoListRepository
    {
        public MessageToDoListRepository(ZaloDbContext context) : base(context)

        {
        }
    }
    public class MessageReactDetailRepository : Repository<MessageReactDetail>, IMessageReactDetailRepository
    {
        public MessageReactDetailRepository(ZaloDbContext context) : base(context)

        {
        }
    }
}
