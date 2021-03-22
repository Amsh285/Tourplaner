using Npgsql;
using System;
using System.Collections.Generic;
using Tourplaner.Infrastructure.Configuration;

namespace Tourplaner.Infrastructure.Database
{
    public sealed class PostgreSqlDatabase
    {
        public string ConnectionString { get; }

        public DatabaseSettings Settings { get; }

        public PostgreSqlDatabase(DatabaseSettings settings)
        {
            Assert.NotNull(settings, nameof(settings));

            Settings = settings;
            ConnectionString = $"User ID={settings.UserID};Password={settings.Password};" +
                $"Host={settings.Host};Port={settings.Port};Database={settings.Database};Pooling=true;";
        }

        public NpgsqlConnection CreateAndOpenConnection()
        {
            NpgsqlConnection connection = new NpgsqlConnection(ConnectionString);
            connection.Open();

            return connection;
        }

        public IEnumerable<TResult> Execute<TResult>(string statement, NpgsqlTransaction transaction,
            Func<NpgsqlDataReader, TResult> rowSelector, params NpgsqlParameter[] parameters)
        {
            if (transaction != null)
            {
                using (NpgsqlCommand command = new NpgsqlCommand(statement, transaction.Connection, transaction))
                {
                    command.Parameters.AddRange(parameters);
                    return ExecuteReader(command, rowSelector);
                }
            }
            else
            {
                using (NpgsqlConnection connection = CreateAndOpenConnection())
                using (NpgsqlCommand command = new NpgsqlCommand(statement, connection))
                {
                    command.Parameters.AddRange(parameters);
                    return ExecuteReader(command, rowSelector);
                }
            }
        }

        public TResult ExecuteScalar<TResult>(string statement, NpgsqlTransaction transaction, params NpgsqlParameter[] parameters)
        {
            if (transaction != null)
            {
                using (NpgsqlCommand command = new NpgsqlCommand(statement, transaction.Connection, transaction))
                {
                    command.Parameters.AddRange(parameters);
                    return (TResult)command.ExecuteScalar();
                }
            }
            else
            {
                using (NpgsqlConnection connection = CreateAndOpenConnection())
                using (NpgsqlCommand command = new NpgsqlCommand(statement, connection))
                {
                    command.Parameters.AddRange(parameters);
                    return (TResult)command.ExecuteScalar();
                }
            }
        }

        public void ExecuteNonQuery(string statement, NpgsqlTransaction transaction, params NpgsqlParameter[] parameters)
        {
            if (transaction != null)
            {
                using (NpgsqlCommand command = new NpgsqlCommand(statement, transaction.Connection, transaction))
                {
                    command.Parameters.AddRange(parameters);
                    command.ExecuteNonQuery();
                }
            }
            else
            {
                using (NpgsqlConnection connection = CreateAndOpenConnection())
                using (NpgsqlCommand command = new NpgsqlCommand(statement, connection))
                {
                    command.Parameters.AddRange(parameters);
                    command.ExecuteNonQuery();
                }
            }
        }

        private static IEnumerable<TResult> ExecuteReader<TResult>(NpgsqlCommand command, Func<NpgsqlDataReader, TResult> rowSelector)
        {
            Assert.NotNull(command, nameof(command));

            using (NpgsqlDataReader dataReader = command.ExecuteReader())
            {
                List<TResult> resultSet = new List<TResult>();

                while (dataReader.Read())
                    resultSet.Add(rowSelector(dataReader));

                return resultSet;
            }
        }
    }
}
