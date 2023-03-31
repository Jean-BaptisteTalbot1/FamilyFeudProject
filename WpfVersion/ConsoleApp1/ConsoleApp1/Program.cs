using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

class Block
{
    public int Index { get; set; }
    public DateTime Timestamp { get; set; }
    public string Data { get; set; }
    public string PreviousHash { get; set; }
    public string Hash { get; set; }

    public Block(DateTime timestamp, string data, string previousHash)
    {
        Index = 0;
        Timestamp = timestamp;
        Data = data;
        PreviousHash = previousHash;
        Hash = CalculateHash();
    }

    public string CalculateHash()
    {
        SHA256 sha256 = SHA256.Create();

        string rawData = $"{PreviousHash}|{Timestamp.ToString("yyyy-MM-dd HH:mm:ss.fff")}|{Data}";
        byte[] bytes = Encoding.UTF8.GetBytes(rawData);
        byte[] hash = sha256.ComputeHash(bytes);

        return Convert.ToBase64String(hash);
    }
}

class Blockchain
{
    public List<Block> Chain { get; set; }

    public Blockchain()
    {
        Chain = new List<Block>();

        DateTime timestamp = DateTime.Now;
        string data = "Genesis block";
        string previousHash = string.Empty;
        Block genesisBlock = new Block(timestamp, data, previousHash);

        Chain.Add(genesisBlock);
    }

    public void AddBlock(string data)
    {
        Block previousBlock = Chain[Chain.Count - 1];
        int index = previousBlock.Index + 1;
        DateTime timestamp = DateTime.Now;
        string previousHash = previousBlock.Hash;

        Block block = new Block(timestamp, data, previousHash);
        block.Index = index;

        Chain.Add(block);
    }

    public bool IsChainValid()
    {
        for (int i = 1; i < Chain.Count; i++)
        {
            Block currentBlock = Chain[i];
            Block previousBlock = Chain[i - 1];

            if (currentBlock.Hash != currentBlock.CalculateHash())
            {
                return false;
            }

            if (currentBlock.PreviousHash != previousBlock.Hash)
            {
                return false;
            }
        }

        return true;
    }
}

class Program
{
    static void Main(string[] args)
    {
        Blockchain blockchain = new Blockchain();

        blockchain.AddBlock("Transaction 1");
        blockchain.AddBlock("Transaction 2");

        Console.WriteLine($"Is blockchain valid? {blockchain.IsChainValid()}");
    }
}
