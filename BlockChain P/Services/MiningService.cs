using BlockChain_P.Models;

namespace BlockChain_P.Services
{
    public class MiningService
    {
        private HashingService hashing;

        public MiningService(HashingService hashingService)
        {
            hashing = hashingService;
        }

        public void MineBlock(Block block, string prefix)
        {
            block.Nonce = 0;
            block.Hash = hashing.ComputeHash(block);

            while (block.Hash.StartsWith(prefix) == false)
            {
                block.Nonce = block.Nonce + 1;
                block.Hash = hashing.ComputeHash(block);
            }

            Console.WriteLine("Block mined successfully! Nonce: " + block.Nonce);
        }
    }
}
