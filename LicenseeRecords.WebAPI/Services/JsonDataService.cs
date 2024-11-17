using Newtonsoft.Json;
using System.IO;
using System.Threading.Tasks;

namespace LicenseeRecords.WebAPI.Services
{
    public class JsonDataService<T> : IJsonDataService<T>
    {
        private readonly string _filePath;

        public JsonDataService(string filePath)
        {
            _filePath = filePath;
        }

        public async Task<List<T>> GetAllAsync()
        {
            if (!File.Exists(_filePath)) return new List<T>();
            var json = await File.ReadAllTextAsync(_filePath);
            return JsonConvert.DeserializeObject<List<T>>(json) ?? new List<T>();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            var entities = await GetAllAsync();
            return entities.FirstOrDefault(e => (int)e.GetType().GetProperty("Id").GetValue(e) == id);
        }

        public async Task AddAsync(T entity)
        {
            var entities = await GetAllAsync();
            entities.Add(entity);
            await SaveAsync(entities);
        }

        public async Task UpdateAsync(T entity)
        {
            var entities = await GetAllAsync();
            var index = entities.FindIndex(e => (int)e.GetType().GetProperty("Id").GetValue(e) == (int)entity.GetType().GetProperty("Id").GetValue(entity));
            if (index >= 0)
            {
                entities[index] = entity;
                await SaveAsync(entities);
            }
        }

        public async Task DeleteAsync(int id)
        {
            var entities = await GetAllAsync();
            entities.RemoveAll(e => (int)e.GetType().GetProperty("Id").GetValue(e) == id);
            await SaveAsync(entities);
        }

        private async Task SaveAsync(List<T> entities)
        {
            var json = JsonConvert.SerializeObject(entities, Formatting.Indented);
            await File.WriteAllTextAsync(_filePath, json);
        }
    }
}