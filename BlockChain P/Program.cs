using System.Text;
using BlockChain_P.Services;

Console.OutputEncoding = Encoding.UTF8;

BlockChainService chain = new BlockChainService();
BlockChainDisplayService display = new BlockChainDisplayService();

if (chain.HasVanityBlock("German", "cafe") == false)
{
    Console.WriteLine("Vanity mining (cafe)...");
    chain.AddBlockWithPrefix("German", "student", "cafe");
    Console.WriteLine();
}

while (true)
{
    display.ShowMenu();
    Console.Write("Enter your choice: ");
    string choice = Console.ReadLine();

    if (choice == "1")
    {
        Console.Write("Enter block data: ");
        string data = Console.ReadLine();
        if (data == null)
            data = "";

        Console.Write("Enter block author: ");
        string author = Console.ReadLine();
        if (author == null)
            author = "";

        chain.AddBlock(data, author);
    }
    else if (choice == "2")
    {
        display.ShowBlockChain(chain.Chain);
    }
    else if (choice == "3")
    {
        display.ShowValidationResult(chain.isValid());
    }
    else if (choice == "4")
    {
        chain.IncreaseDifficulty();
    }
    else if (choice == "5")
    {
        chain.DecreaseDifficulty();
    }
    else if (choice == "6")
    {
        chain.MineThreeBlocks();
        Console.WriteLine();
        display.ShowBlockChain(chain.Chain);
        display.ShowValidationResult(chain.isValid());
    }
    else if (choice == "7")
    {
        chain.PerformanceTest();
    }
    else if (choice == "8")
    {
        Console.WriteLine("Rebuild a fresh chain of 5 blocks (Difficulty 3) for hack demo? (y/n)");
        string rebuild = Console.ReadLine();

        if (rebuild == "y" || rebuild == "Y")
        {
            chain.CreateChainForHack();
            Console.WriteLine();
            display.ShowBlockChain(chain.Chain);
        }

        Console.Write("Enter block index to hack: ");
        string indexText = Console.ReadLine();
        int hackIndex = 0;
        bool ok = int.TryParse(indexText, out hackIndex);

        if (ok == false)
        {
            Console.WriteLine("Invalid index.");
        }
        else
        {
            Console.Write("Enter fake data: ");
            string fakeData = Console.ReadLine();
            if (fakeData == null)
                fakeData = "HACKED";

            chain.HackChain(hackIndex, fakeData);
            Console.WriteLine();
            display.ShowBlockChain(chain.Chain);
            display.ShowValidationResult(chain.isValid());
        }
    }
    else if (choice == "9")
    {
        break;
    }
    else
    {
        Console.WriteLine("Invalid choice.");
    }

    Console.WriteLine("Press any key to continue...");
    Console.ReadKey(true);
    Console.WriteLine();
}
