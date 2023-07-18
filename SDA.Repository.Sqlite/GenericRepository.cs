using SQLite;

namespace SDA.Repository.Sqlite
{
    public class GenericRepository
    {
        string _dbPath;

        public string StatusMessage { get; set; } = string.Empty;

        private SQLiteAsyncConnection _connection;

        public GenericRepository(string dbPath)
        {
            _dbPath = dbPath;
        }

        private async Task Init<T>() where T : new()
        {
            if (_connection != null)
                return;

            _connection = new SQLiteAsyncConnection(_dbPath);
            await _connection.CreateTableAsync<T>();
        }

        public async Task<List<T>> GetAllItems<T>() where T : new()
        {
            try
            {
                await Init<T>();
                return await _connection.Table<T>().ToListAsync();
            }
            catch (Exception ex)
            {
                StatusMessage = string.Format("Failed to retrieve data. {0}", ex.Message);
                return new List<T>();
            }
        }

        public async Task<int> AddItem<T>(T entry) where T : new()
        {
            if (entry == null)
            {
                StatusMessage = string.Format("Entry is not valid");
                return -1;
            }

            try
            {
                await Init<T>();
                int result = await _connection.InsertAsync(entry);

                StatusMessage = string.Format("Entry was successfull added");
                return result;
            }
            catch (Exception ex)
            {
                StatusMessage = string.Format("Failed to add entry. Error: {0}", ex.Message);
                return -1;
            }
        }

        public async Task<int> UpdateItem<T>(T entry) where T : new()
        {
            if (entry == null)
            {
                StatusMessage = string.Format("Entry is not valid");
                return -1;
            }

            try
            {
                await Init<T>();
                int result = await _connection.UpdateAsync(entry);

                StatusMessage = string.Format("Entry was successfull updated");
                return result;
            }
            catch (Exception ex)
            {
                StatusMessage = string.Format("Failed to update entry. Error: {0}", ex.Message);
                return -1;
            }
        }

        public async Task<int> DeleteItem<T>(T entry) where T : new()
        {
            if (entry == null)
            {
                StatusMessage = string.Format("Entry is not valid");
                return -1;
            }

            try
            {
                await Init<T>();
                int result = await _connection.DeleteAsync(entry);

                StatusMessage = string.Format("Entry was successfull deleted");
                return result;
            }
            catch (Exception ex)
            {
                StatusMessage = string.Format("Failed to deleted entry. Error: {0}", ex.Message);
                return -1;
            }
        }

        public async Task<int> DropItemTable<T>() where T : new()
        {            
            try
            {
                await Init<T>();
                return await _connection.DropTableAsync<T>();
            }
            catch (Exception ex)
            {
                StatusMessage = string.Format("Failed to drop table related to {0}. {1}", typeof(T).ToString(), ex.Message);
                return -1;
            }
        }
    }
}