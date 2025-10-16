namespace School.Application.Interfaces
{
    public interface IDataSeed
    {
        Task IdentityDataSeedAsync();

        Task DataSeedAsync();
    }
}
