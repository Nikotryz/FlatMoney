using SQLite;
using FlatMoney.Models;
using System.Diagnostics;
namespace FlatMoney.LocalDataBase
{
    public class LocalDBService
    {
        private const string DB_NAME = "FlatMoneyDB.db";
        private readonly static string _dbPath = Path.Combine(FileSystem.AppDataDirectory, DB_NAME);
        private SQLiteAsyncConnection _connection = new SQLiteAsyncConnection(_dbPath);
        private bool _isInitialized = false;

        private async Task Init()
        {
            if (_isInitialized) return;

            try
            {
                _connection = new SQLiteAsyncConnection(_dbPath);

                _connection.Tracer = new Action<string>(q => Debug.WriteLine(q));
                _connection.Trace = true;

                await CreateTables();
                _isInitialized = true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Initialization error: {ex}");
                throw;
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
            await Init();

            return await _connection.Table<T>().ToListAsync();
        }

        public async Task<T> GetItem<T>(int? id) where T : BaseTable, new()
        {
            await Init();

            return await _connection.GetAsync<T>(id);
        }

        public async Task<int> GetCountOfItems<T>() where T : BaseTable, new()
        {
            await Init();

            return await _connection.Table<T>().CountAsync();
        }

        public async Task InsertItem<T>(T item)
        {
            await Init();

            await _connection.InsertAsync(item);
        }

        public async Task UpdateItem<T>(T item)
        {
            await Init();

            await _connection.UpdateAsync(item);
        }

        public async Task DeleteItem<T>(T item)
        {
            await Init();

            await _connection.DeleteAsync(item);
        }

        public async Task DeleteAllItems<T>()
        {
            await Init();

            await _connection.DeleteAllAsync<T>();
        }
    }
}
