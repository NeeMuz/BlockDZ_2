using System.Diagnostics;
using BlockChain_P.Models;

namespace BlockChain_P.Services
{
    public class BlockChainService
    {
        public List<Block> Chain { get; set; }
        public int Difficulty { get; set; }

        private HashingService hashing;
        private MiningService mining;
        private ChainStorage storage;

        public BlockChainService()
        {
            Difficulty = 2;
            hashing = new HashingService();
            mining = new MiningService(hashing);
            storage = new ChainStorage();
            Chain = new List<Block>();

            SaveData saved = storage.Load();
            if (saved != null && saved.Chain != null && saved.Chain.Count > 0)
            {
                Chain = saved.Chain;
                Difficulty = saved.Difficulty;
            }
            else
            {
                CreateGenesis();
                Save();
            }
        }

        public string GetPrefix()
        {
            return new string('0', Difficulty);
        }

        private void CreateGenesis()
        {
            Block g = new Block(0, DateTime.Now, "Genesis Block", "Genesis Author", "0");
            mining.MineBlock(g, GetPrefix());
            Chain.Add(g);
        }

        private void Save()
        {
            storage.Save(Chain, Difficulty);
        }

        public void AddBlock(string data, string author)
        {
            Block prev = Chain[Chain.Count - 1];
            Block b = new Block(prev.Index + 1, DateTime.Now, data, author, prev.Hash);
            mining.MineBlock(b, GetPrefix());
            Chain.Add(b);
            Save();
        }

        public void AddBlockWithPrefix(string data, string author, string prefix)
        {
            Block prev = Chain[Chain.Count - 1];
            Block b = new Block(prev.Index + 1, DateTime.Now, data, author, prev.Hash);
            mining.MineBlock(b, prefix);
            Chain.Add(b);
            Save();
        }

        public bool HasVanityBlock(string data, string prefix)
        {
            for (int i = 0; i < Chain.Count; i++)
            {
                if (Chain[i].Data == data && Chain[i].Hash.StartsWith(prefix))
                    return true;
            }
            return false;
        }

        public void IncreaseDifficulty()
        {
            Difficulty = Difficulty + 1;
            Console.WriteLine("Difficulty: " + Difficulty);
            Console.WriteLine("Target prefix: " + GetPrefix());
            Save();
        }

        public void DecreaseDifficulty()
        {
            if (Difficulty > 1)
                Difficulty = Difficulty - 1;

            Console.WriteLine("Difficulty: " + Difficulty);
            Console.WriteLine("Target prefix: " + GetPrefix());
            Save();
        }

        public void MineThreeBlocks()
        {
            Difficulty = 3;
            Console.WriteLine("Difficulty: " + Difficulty);
            Console.WriteLine("Target prefix: " + GetPrefix());
            Console.WriteLine();
            Console.WriteLine("Mining 3 blocks with Difficulty = 3...");

            for (int i = 1; i <= 3; i++)
            {
                Console.WriteLine("--- Block " + i + "/3 ---");
                AddBlock("PoW test block " + i, "student");
            }

            Console.WriteLine();
            Console.WriteLine("Done. Use menu 2 to show chain, menu 3 to validate.");
        }

        public void PerformanceTest()
        {
            Console.WriteLine("Performance analysis (Difficulty 1..5)...");
            Console.WriteLine();
            Console.WriteLine("Складність   | Кількість спроб (Nonce)  | Витрачений час");
            Console.WriteLine("----------------------------------------------------------------");

            string prevHash = "0";
            if (Chain.Count > 0)
                prevHash = Chain[Chain.Count - 1].Hash;

            for (int d = 1; d <= 5; d++)
            {
                string prefix = new string('0', d);
                Block b = new Block(0, DateTime.Now, "test " + d, "test", prevHash);

                Stopwatch sw = new Stopwatch();
                sw.Start();

                b.Nonce = 0;
                b.Hash = hashing.ComputeHash(b);
                while (b.Hash.StartsWith(prefix) == false)
                {
                    b.Nonce = b.Nonce + 1;
                    b.Hash = hashing.ComputeHash(b);
                }

                sw.Stop();

                Console.WriteLine(d + "            | " + b.Nonce + "                       | " + sw.Elapsed.TotalSeconds.ToString("0.0000") + " сек");
                prevHash = b.Hash;
            }

            Console.WriteLine();
        }

        public void HackChain(int index, string fakeData)
        {
            if (index < 0 || index >= Chain.Count)
            {
                Console.WriteLine("Wrong index!");
                return;
            }

            Console.WriteLine("Hacking block #" + index + " with data: \"" + fakeData + "\"...");

            Stopwatch sw = new Stopwatch();
            sw.Start();

            Chain[index].Data = fakeData;
            mining.MineBlock(Chain[index], GetPrefix());

            for (int i = index + 1; i < Chain.Count; i++)
            {
                Chain[i].PrevHash = Chain[i - 1].Hash;
                Console.WriteLine("Re-mining block #" + i + "...");
                mining.MineBlock(Chain[i], GetPrefix());
            }

            sw.Stop();
            Save();

            Console.WriteLine();
            Console.WriteLine("Hack finished in: " + sw.Elapsed);
            Console.WriteLine("Total hack time: " + sw.Elapsed);
        }

        public void CreateChainForHack()
        {
            Difficulty = 3;
            Chain = new List<Block>();
            CreateGenesis();

            Console.WriteLine("Building chain of 5 blocks at Difficulty=3...");
            for (int i = 1; i <= 5; i++)
            {
                Console.WriteLine("--- Block " + i + "/5 ---");
                Block prev = Chain[Chain.Count - 1];
                Block b = new Block(prev.Index + 1, DateTime.Now, "Block " + i, "demo", prev.Hash);
                mining.MineBlock(b, GetPrefix());
                Chain.Add(b);
            }

            Save();
            Console.WriteLine("Chain ready for hack simulation.");
        }

        public bool isValid()
        {
            for (int i = 1; i < Chain.Count; i++)
            {
                Block cur = Chain[i];
                Block prev = Chain[i - 1];

                if (cur.Hash != hashing.ComputeHash(cur))
                    return false;

                if (cur.PrevHash != prev.Hash)
                    return false;
            }

            return true;
        }
    }
}
