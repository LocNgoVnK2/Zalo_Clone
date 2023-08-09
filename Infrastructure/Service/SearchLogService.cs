using Infrastructure.Entities;
using Infrastructure.Repository;
using Infrastructure.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Service
{

    public interface ISearchLogService
    {
        Task<List<SearchLog>> GetSearchResent(string userID);
        Task<bool> AddSearchLog(string userSrcId, string userDesId);
        Task<bool> DeleteSearchLog(string userSrcId, string userDesId);


    }
}
public class SearchLogService : ISearchLogService
{
    private ISearchLogRepository _repo;
    public SearchLogService(ISearchLogRepository repo)
    {
        this._repo = repo;
    }

    public async Task<bool> AddSearchLog(string userSrcId, string userDesId)
    {
        List<SearchLog> sameTimesSearch = _repo.GetAll()
        .Where(user => user.UserSrcId == userSrcId && user.UserDesId == userDesId)
        .ToList();
        foreach (SearchLog element in sameTimesSearch)
        {
            await _repo.Delete(element);
        }
        SearchLog searchLog = new SearchLog()
        {
            UserDesId = userDesId,
            UserSrcId = userSrcId,
            AtTime = DateTime.Now
        };
        bool result = await _repo.Add(searchLog);
        int timesSearch = _repo.GetAll()
         .Where(user => user.UserSrcId == userSrcId)
         .Count();
        if (timesSearch > 5)
        {
            SearchLog oldestSearch = _repo.GetAll()
                .Where(user => user.UserSrcId == userSrcId)
                .OrderBy(x => x.AtTime).FirstOrDefault();

            if (oldestSearch != null)
            {
                result = await _repo.Delete(oldestSearch);
            }
        }
        return result;
    }

    public async Task<bool> DeleteSearchLog(string userSrcId, string userDesId)
    {
        SearchLog searchLog = _repo.GetAll()
        .Where(user => user.UserSrcId == userSrcId && user.UserDesId == userDesId).FirstOrDefault();
        return await _repo.Delete(searchLog);
    }

    public async Task<List<SearchLog>> GetSearchResent(string userID)
    {

        List<SearchLog> searchLogs = _repo.GetAll()
       .Where(user => user.UserSrcId == userID)
       .OrderByDescending(x => x.AtTime)
       .Take(5)
       .ToList();

        return searchLogs;
    }


}
