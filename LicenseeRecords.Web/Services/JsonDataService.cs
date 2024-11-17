using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace LicenseeRecords.Web.Services
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
            if (!File.Exists(_filePath))
            {
                return new List<T>();
            }

            var json = await File.ReadAllTextAsync(_filePath);
            return JsonSerializer.Deserialize<List<T>>(json) ?? new List<T>();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            var entities = await GetAllAsync();
            var idProperty = typeof(T).GetProperties()
                .FirstOrDefault(p => p.Name.EndsWith("Id", System.StringComparison.OrdinalIgnoreCase));

            if (idProperty == null)
                throw new InvalidOperationException($"Entity type {typeof(T).Name} does not have an 'Id' property.");

            return entities.FirstOrDefault(e =>
            {
                var value = idProperty.GetValue(e);
                return value != null && (int)value == id;
            });
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
            var idProperty = typeof(T).GetProperties()
                .FirstOrDefault(p => p.Name.EndsWith("Id", System.StringComparison.OrdinalIgnoreCase));

            if (idProperty == null)
                throw new InvalidOperationException($"Entity type {typeof(T).Name} does not have an 'Id' property.");

            var id = (int)idProperty.GetValue(entity);
            var existingEntity = entities.FirstOrDefault(e =>
            {
                var value = idProperty.GetValue(e);
                return value != null && (int)value == id;
            });

            if (existingEntity != null)
            {
                entities.Remove(existingEntity);
                entities.Add(entity);
            }
            await SaveAsync(entities);
        }

        public async Task DeleteAsync(int id)
        {
            var entities = await GetAllAsync();
            var idProperty = typeof(T).GetProperties()
                .FirstOrDefault(p => p.Name.EndsWith("Id", System.StringComparison.OrdinalIgnoreCase));

            if (idProperty == null)
                throw new InvalidOperationException($"Entity type {typeof(T).Name} does not have an 'Id' property.");

            var entityToRemove = entities.FirstOrDefault(e =>
            {
                var value = idProperty.GetValue(e);
                return value != null && (int)value == id;
            });

            if (entityToRemove != null)
            {
                entities.Remove(entityToRemove);
                await SaveAsync(entities);
            }
        }

        private async Task SaveAsync(List<T> entities)
        {
            var json = JsonSerializer.Serialize(entities, new JsonSerializerOptions { WriteIndented = true });
            await File.WriteAllTextAsync(_filePath, json);
        }
    }
}