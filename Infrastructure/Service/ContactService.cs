using Infrastructure.Entities;
using Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace Infrastructure.Service
{
    public interface IContactService
    {
        Task<Contact> GetContactData(string id);
        Task<bool> AddContact(Contact data);
        Task<bool> UpdateContact(Contact data);
    }
    public class ContactService : IContactService
    {
        private IContactRepository _repo;
        public ContactService(IContactRepository repo)
        {
            this._repo = repo;
        }

        public async Task<bool> AddContact(Contact data)
        {
            return await _repo.Add(data);
        }

        public async Task<Contact> GetContactData(string id)
        {
            return await _repo.GetById(id);
        }

        public async Task<bool> UpdateContact(Contact data)
        {
            return await _repo.Update(data);
        }
    }
}
