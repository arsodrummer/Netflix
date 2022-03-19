using Microsoft.Data.SqlClient;

public static class SqlHelper
{
    public static void ExecuteSql(string connectionString, string sql)
    {
        EnsureDatabaseExists(connectionString);

        using (var connection = new SqlConnection(connectionString))
        {
            connection.Open();

            using (var command = connection.CreateCommand())
            {
                command.CommandText = sql;
                command.ExecuteNonQuery();
            }
        }
    }

    public static void CreateSchema(string connectionString, string schema)
    {
        var sql = $@" IF NOT EXISTS
                (SELECT *
                    FROM sys.schemas
                    WHERE name = N'{schema}')
                    EXEC('CREATE SCHEMA {schema}'
                );";
        ExecuteSql(connectionString, sql);
    }

    public static void EnsureDatabaseExists(string connectionString)
    {
        var builder = new SqlConnectionStringBuilder(connectionString);
        var database = builder.InitialCatalog;

        var masterConnection = connectionString.Replace(builder.InitialCatalog, "master");

        using (var connection = new SqlConnection(masterConnection))
        {
            connection.Open();

            using (var command = connection.CreateCommand())
            {
                command.CommandText = $@"IF(db_id('{database}') IS NULL) CREATE DATABASE [{database}]";
                command.ExecuteNonQuery();
            }
        }
    }
}