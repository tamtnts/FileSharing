namespace DataAccess.Repositories.Text
{
    public interface ITextRepository 
    {
        Task<Models.Text> GetTextByIdAsync(Guid id);
        Task AddTextAsync(Models.Text textRecord);
        Task RemoveTextAsync(Models.Text textRecord);
    }
}
