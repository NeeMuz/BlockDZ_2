using BlockChain_P.Models;

namespace BlockChain_P.Services
{
    public class BlockChainDisplayService
    {
        public void ShowMenu()
        {
            Console.WriteLine("Block Management Menu:");
            Console.WriteLine("1. Add a new block");
            Console.WriteLine("2. Display the blockchain");
            Console.WriteLine("3. Validate the blockchain");
            Console.WriteLine("4. Change difficulty ++");
            Console.WriteLine("5. Change difficulty --");
            Console.WriteLine("6. Level 1 demo (3 blocks, Difficulty 3)");
            Console.WriteLine("7. Level 2: performance analysis (Diff 1..5)");
            Console.WriteLine("8. Level 3: hack chain simulation");
            Console.WriteLine("9. Exit");
        }

        public void ShowBlockChain(List<Block> chain)
        {
            for (int i = 0; i < chain.Count; i++)
            {
                Block block = chain[i];
                Console.WriteLine("Index: " + block.Index);
                Console.WriteLine("Timestamp: " + block.TimeStamp);
                Console.WriteLine("Data: " + block.Data);
                Console.WriteLine("Author: " + block.Author);
                Console.WriteLine("Previous Hash: " + block.PrevHash);
                Console.WriteLine("Nonce: " + block.Nonce);
                Console.WriteLine("Hash: " + block.Hash);
                Console.WriteLine("--------------------------------------------------");
            }
        }

        public void ShowValidationResult(bool ok)
        {
            if (ok == true)
                Console.WriteLine("The blockchain is valid.");
            else
                Console.WriteLine("The blockchain is NOT valid.");
        }
    }
}
