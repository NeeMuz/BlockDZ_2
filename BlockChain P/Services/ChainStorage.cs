using System.Text.Json;
using BlockChain_P.Models;

namespace BlockChain_P.Services
{
    public class SaveData
    {
        public int Difficulty { get; set; }
        public List<Block> Chain { get; set; }

        public SaveData()
        {
            Difficulty = 2;
            Chain = new List<Block>();
        }
    }

    public class ChainStorage
    {
        private string path = Path.Combine(AppContext.BaseDirectory, "blockchain.json");

        public void Save(List<Block> chain, int difficulty)
        {
            SaveData data = new SaveData();
            data.Difficulty = difficulty;
            data.Chain = chain;

            string json = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(path, json);
        }

        public SaveData Load()
        {
            if (File.Exists(path) == false)
                return null;

            string json = File.ReadAllText(path);
            return JsonSerializer.Deserialize<SaveData>(json);
        }
    }
}
