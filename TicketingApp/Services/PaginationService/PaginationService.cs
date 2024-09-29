using Microsoft.EntityFrameworkCore;
using TicketingApp.Data;
using TicketingApp.Models;

namespace TicketingApp.Services.PaginationService;

public class PaginationService<TEntity>(TicketingAppCtx ctx) where TEntity : FullTextSearchEntity
{
    public async Task<PaginationModel<TEntity>> Search(string searchInput, int pageSize = 5)
    {

        var searchQuery = ctx.Set<TEntity>()
            .OrderBy(a => a.Name)
            .Where(a => a.SearchVector!.Matches(EF.Functions.PlainToTsQuery($"{searchInput}:*")));

        var paginationModel = new PaginationModel<TEntity>
        {
            Entities = await searchQuery.Take(pageSize).ToListAsync(),
            PageSize = pageSize,
            PageCount = searchQuery.Count() / pageSize,
            SearchInput = searchInput,
        };

        return paginationModel;
    }

    public async Task<PaginationModel<TEntity>> ListAllPage(int pageSize)
    {

        var entities = new List<TEntity>();
        var paginationModel = new PaginationModel<TEntity> { Entities = entities, PageSize = pageSize };
        var entitiesQuery = ctx.Set<TEntity>();

        paginationModel.PageCount = (entitiesQuery.Count() + pageSize - 1) / pageSize;
        paginationModel.Entities = await entitiesQuery
            .OrderBy(a => a.Name)
            .Take(pageSize)
            .ToListAsync();


        return paginationModel;
    }

    public async Task<PaginationModel<TEntity>> Pagination(string searchInput, int offset, int pageCount, int pageSize, int currentPage)
    {
        var entities = new List<TEntity>();
        var paginationModel = new PaginationModel<TEntity> { Entities = entities, CurrentPage = currentPage };

        if (string.IsNullOrEmpty(searchInput))
        {
            var getAllArtistQuery = ctx.Set<TEntity>();

            paginationModel.PageCount = pageCount;
            paginationModel.PageSize = pageSize;
            paginationModel.Entities = await getAllArtistQuery
                .OrderBy(a => a.Name)
                .Skip(offset)
                .Take(pageSize)
                .ToListAsync();


            return paginationModel;
        }

        var searchQuery = ctx.Set<TEntity>()
            .OrderBy(a => a.Name)
            .Where(a => a.SearchVector!.Matches(EF.Functions.ToTsQuery($"{searchInput}:*")));

        paginationModel.PageCount = pageCount;
        paginationModel.PageSize = pageSize;
        paginationModel.Entities = await searchQuery.Skip(offset).Take(pageSize).ToListAsync();
        paginationModel.SearchInput = searchInput;

        return paginationModel;
    }
}