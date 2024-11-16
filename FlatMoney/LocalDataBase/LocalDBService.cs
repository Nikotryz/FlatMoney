using SQLite;
using FlatMoney.Models;
using System.Diagnostics;

namespace FlatMoney.LocalDataBase
{
    public class LocalDBService
    {
        private const string DB_NAME = "database_test.db5";
        private readonly string _dbPath = Path.Combine(FileSystem.AppDataDirectory, DB_NAME);
        private SQLiteAsyncConnection _connection;

        public LocalDBService()
        {
            Initialization();
        }

        private async Task Initialization()
        {
            if (_connection != null)
                return;

            try
            {
                _connection = new SQLiteAsyncConnection(_dbPath);

                _connection.Tracer = new Action<string>(q => Debug.WriteLine(q));
                _connection.Trace = true;

                await CreateTables();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        private async Task CreateTables()
        {
            var createTableStatements = new List<string>()
            {
                Tables.CREATE_FLATS_STATEMENT,
                Tables.CREATE_CLIENTS_STATEMENT,
                Tables.CREATE_RESERVATIONS_STATEMENT,
                Tables.CREATE_SERVICES_STATEMENT,
                Tables.CREATE_RESERVATION_SERVICES_STATEMENT,
                Tables.CREATE_RESERVATION_CLIENTS_STATEMENT,
                Tables.CREATE_PAYMENTS_STATEMENT,
                Tables.CREATE_EXPENSE_TYPES_STATEMENT,
                Tables.CREATE_EXPENSES_STATEMENT
            };

            foreach (var statement in createTableStatements)
                await ExecuteQuery(statement);
        }

        public async Task<bool> ExecuteQuery(string query)
        {
            var op = await _connection.ExecuteAsync(query);
            return op > 0;
        }

        public async Task<List<T>> GetItems<T>() where T : BaseTable, new()
        {
            return await _connection.Table<T>().ToListAsync();
        }

        public async Task<int> GetCountOfItems<T>() where T : BaseTable, new()
        {
            return await _connection.Table<T>().CountAsync();
        }

        public async Task InsertItem<T>(T item)
        {
            await _connection.InsertAsync(item);
        }

        public async Task UpdateItem<T>(T item)
        {
            await _connection.UpdateAsync(item);
        }

        public async Task DeleteItem<T>(T item)
        {
            await _connection.DeleteAsync(item);
        }

        public async Task DeleteAllItems<T>()
        {
            await _connection.DeleteAllAsync<T>();
        }
    }
}
