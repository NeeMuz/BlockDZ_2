using System.Security.Cryptography;
using System.Text;
using BlockChain_P.Models;

namespace BlockChain_P.Services
{
    public class HashingService
    {
        public string ComputeHash(Block block)
        {
            string raw = block.Index.ToString()
                + block.TimeStamp.ToString("o")
                + block.Data
                + block.Author
                + block.PrevHash
                + block.Nonce.ToString();

            return ComputeHash(raw);
        }

        public string ComputeHash(string input)
        {
            using (SHA256 sha = SHA256.Create())
            {
                byte[] bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(input));
                string hash = BitConverter.ToString(bytes);
                hash = hash.Replace("-", "");
                hash = hash.ToLower();
                return hash;
            }
        }
    }
}
