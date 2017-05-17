namespace SMAS.Data
{
    public interface IDataContextFactory
    {
        IDataContext GetContext();
        //string GetConnectionString();
    }
}
