using Moq;
using SQLite;
using FlatMoney.LocalDataBase;
using FlatMoney.Models;

namespace FlatMoneyUnitTests
{
    public class LocalDBServiceTests
    {
        private const string TEST_DB_NAME = "TestDatabase.db";
        private readonly string _testDbPath = Path.Combine(Path.GetTempPath(), TEST_DB_NAME);

        [Fact]
        public async Task Initialize_ShouldCreateDatabaseAndTables()
        {
            // Arrange
            File.Delete(_testDbPath);
            var service = new LocalDBService(dbName: TEST_DB_NAME, dbPath: _testDbPath, isTest: true);

            // Act
            await service.Initialization();

            // Assert
            using (var connection = new SQLiteConnection($"Data Source={_testDbPath};"))
            {
                string commandText = "SELECT name FROM sqlite_master WHERE type='table' AND name NOT LIKE 'sqlite_%'";
                var command = connection.CreateCommand(commandText);
                var reader = command.ExecuteQuery<string>();

                var tables = new List<string>();
                foreach (var table in reader)
                {
                    tables.Add(table);
                }

                Assert.Equal
                (
                    new[] 
                    { 
                        Tables.FLATS_TABLE_NAME, 
                        Tables.CLIENTS_TABLE_NAME, 
                        Tables.RESERVATIONS_TABLE_NAME, 
                        Tables.SERVICES_TABLE_NAME, 
                        Tables.RESERVATION_SERVICES_TABLE_NAME, 
                        Tables.RESERVATION_CLIENTS_TABLE_NAME, 
                        Tables.PAYMENTS_TABLE_NAME, 
                        Tables.EXPENSE_TYPES_TABLE_NAME, 
                        Tables.EXPENSES_TABLE_NAME 
                    }, 
                    tables
                );
            }
        }

        [Theory]
        [InlineData($"INSERT INTO {Tables.FLATS_TABLE_NAME} (flat_name, flat_type) VALUES ('TestFlat', '{Tables.FlatType1}')")]
        [InlineData($"UPDATE {Tables.FLATS_TABLE_NAME} SET flat_name = 'UpdatedName' WHERE Id = 1")]
        [InlineData($"DELETE FROM {Tables.FLATS_TABLE_NAME} WHERE Id = 1")]
        public async Task ExecuteQuery_ShouldExecuteQuerySuccessfully(string query)
        {
            // Arrange
            File.Delete(_testDbPath);
            var service = new LocalDBService(dbName: TEST_DB_NAME, dbPath: _testDbPath, isTest: true);
            await service.Initialization();

            // Act
            var result = await service.ExecuteQuery(query);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task GetItems_ShouldReturnAllItemsFromTable()
        {
            // Arrange
            File.Delete(_testDbPath);
            var service = new LocalDBService(dbName: TEST_DB_NAME, dbPath: _testDbPath, isTest: true);
            await service.Initialization();

            var flat1 = new FlatModel { Name = "Flat1", Type = Tables.FlatType1 };
            var flat2 = new FlatModel { Name = "Flat2", Type = Tables.FlatType1 };
            await service.InsertItem(flat1);
            await service.InsertItem(flat2);

            // Act
            var items = await service.GetItems<FlatModel>();

            // Assert
            Assert.Equal(2, items.Count);
            Assert.Contains(items, x => x.Id == 1 && x.Name == flat1.Name);
            Assert.Contains(items, x => x.Id == 2 && x.Name == flat2.Name);
        }

        [Fact]
        public async Task GetItem_ShouldReturnItemById()
        {
            // Arrange
            File.Delete(_testDbPath);
            var service = new LocalDBService(dbName: TEST_DB_NAME, dbPath: _testDbPath, isTest: true);
            await service.Initialization();

            var flat1 = new FlatModel { Name = "Flat1", Type = Tables.FlatType1 };
            await service.InsertItem(flat1);

            // Act
            var item = await service.GetItem<FlatModel>(1);

            // Assert
            Assert.NotNull(item);
            Assert.Equal(1, item.Id);
            Assert.Equal(flat1.Name, item.Name);
        }

        [Fact]
        public async Task GetCountOfItems_ShouldReturnCorrectCount()
        {
            // Arrange
            File.Delete(_testDbPath);
            var service = new LocalDBService(dbName: TEST_DB_NAME, dbPath: _testDbPath, isTest: true);
            await service.Initialization();

            var flat1 = new FlatModel { Name = "Flat1", Type = Tables.FlatType1 };
            var flat2 = new FlatModel { Name = "Flat2", Type = Tables.FlatType1 };
            await service.InsertItem(flat1);
            await service.InsertItem(flat2);

            // Act
            var count = await service.GetCountOfItems<FlatModel>();

            // Assert
            Assert.Equal(2, count);
        }

        [Fact]
        public async Task InsertItem_ShouldInsertNewItemIntoTable()
        {
            // Arrange
            File.Delete(_testDbPath); // ”дал€ем базу данных перед каждым запуском теста
            var service = new LocalDBService(dbName: TEST_DB_NAME, dbPath: _testDbPath, isTest: true);
            await service.Initialization();

            var flat = new FlatModel { Name = "Flat1", Type = Tables.FlatType1 };

            // Act
            await service.InsertItem(flat);

            // Assert
            var insertedItem = await service.GetItem<FlatModel>(1);
            Assert.NotNull(insertedItem);
            Assert.Equal(1, insertedItem.Id);
            Assert.Equal(flat.Name, insertedItem.Name);
        }

        [Fact]
        public async Task UpdateItem_ShouldUpdateExistingItemInTable()
        {
            // Arrange
            File.Delete(_testDbPath);
            var service = new LocalDBService(dbName: TEST_DB_NAME, dbPath: _testDbPath, isTest: true);
            await service.Initialization();

            var flat = new FlatModel { Name = "Flat1", Type = Tables.FlatType1 };
            await service.InsertItem(flat);

            flat.Name = "UpdatedName";

            // Act
            await service.UpdateItem(flat);

            // Assert
            var updatedItem = await service.GetItem<FlatModel>(1);
            Assert.NotNull(updatedItem);
            Assert.Equal(1, updatedItem.Id);
            Assert.Equal(flat.Name, updatedItem.Name);
        }

        [Fact]
        public async Task DeleteItem_ShouldDeleteItemFromTable()
        {
            // Arrange
            File.Delete(_testDbPath);
            var service = new LocalDBService(dbName: TEST_DB_NAME, dbPath: _testDbPath, isTest: true);
            await service.Initialization();

            var flat = new FlatModel { Name = "Flat1", Type = Tables.FlatType1 };
            await service.InsertItem(flat);

            // Act
            await service.DeleteItem(flat);

            // Assert
            var deletedItem = await service.GetItem<FlatModel>(1);
            Assert.Null(deletedItem);
        }

        [Fact]
        public async Task DeleteAllItems_ShouldDeleteAllItemsFromTable()
        {
            // Arrange
            File.Delete(_testDbPath);
            var service = new LocalDBService(dbName: TEST_DB_NAME, dbPath: _testDbPath, isTest: true);
            await service.Initialization();

            var flat1 = new FlatModel { Name = "Flat1", Type = Tables.FlatType1 };
            var flat2 = new FlatModel { Name = "Flat2", Type = Tables.FlatType1 };
            await service.InsertItem(flat1);
            await service.InsertItem(flat2);

            // Act
            await service.DeleteAllItems<FlatModel>();

            // Assert
            var items = await service.GetItems<FlatModel>();
            Assert.Empty(items);
        }
    }
}