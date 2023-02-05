class BlockchainService
{
    public int difficulty { get; init; }

    private List<Block> _blockchain = new List<Block>();

    public BlockchainService(int difficulty)
    {
        this.difficulty = difficulty;
    }

    public void AddBlock(string? data)
    {
        string? prevHash = _blockchain.LastOrDefault()?.hash;
        Block newBlock = new(prevHash, data, difficulty);
        _blockchain.Add(newBlock);
    }

    public void List()
    {
        System.Console.WriteLine("====== List blocks START ======");
        _blockchain.ForEach(b =>
        {
            Console.WriteLine($"{(VerifyBlock(b) ? "[OK]" : "[INVALID!]")} {b}");
        });
        System.Console.WriteLine("====== List blocks END ======");
    }

    public bool VerifyBlock(Block block)
    {
        // check hash
        string hash = BlockUtil.CalcHash(block.prevHash, block.data);
        if (hash != block.hash) return false;

        // check nonce
        return BlockUtil.CheckNonce(difficulty, block.hash, block.nonce, out _);
    }

    public int GetBlockchainLength() => _blockchain.Count();

    public string? GetBlockDataAt(int index) => _blockchain.ElementAt(index).data;

    public void ForceOverwriteDataAt(int index, string? newData)
        => _blockchain.ElementAt(index).ForceOverwriteData(newData);
}