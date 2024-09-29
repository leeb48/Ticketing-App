namespace TicketingApp.Services.TicketLockService;

public interface ILockService<Entity>
{
    public void CreateLock(List<Entity> entities, int TTL);
    public Task<List<Entity>> GetLockedEntities(List<Entity> entities);
}