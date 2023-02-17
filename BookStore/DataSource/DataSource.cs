using Npgsql;
namespace BookStore.DataSource
{
    public class DataSource
    {
        public NpgsqlDataSource source;
        public DataSource()
        {
            var connectionString = "Server=localhost;Port=5432;Database=book;User Id=postgres;Password=12345678;";
            source = NpgsqlDataSource.Create(connectionString);
        }
    }
}
