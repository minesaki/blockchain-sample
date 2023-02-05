public record Block
{
    public string? prevHash { get; private set; }
    public string hash { get; private set; }
    public string? data { get; private set; }
    public string nonce { get; private set; }
    public int difficulty { get; private set; }

    public Block(string? prevHash, string? data, int difficulty)
    {
        this.prevHash = prevHash;
        this.data = data;
        this.difficulty = difficulty;

        // This hash is calculated with the previous block's hash.
        // So, if one block is modified, all subsequent blocks need to be recalculated.
        // This works as an overwrite protection.
        // But this safety measure is not enough because the recalculation would be done quickly.
        hash = BlockUtil.CalcHash(prevHash, data);

        // Block creation (and recalculation) should be somewhat high cost.
        // To slow down block creation, enforce users to find a value that meets criteria. (= Mining)
        // The value is usually called "nonce (number used once)".
        // The first X digits of the hash of "the concatenation of this block's hash and the nonce" must be 0.
        // Required time depends on X, so X is called "difficulty".
        // On my MacBook Pro, X=3: <1sec, X=4: 2-3sec, X=5: 30-40sec, X=6: 3-4min
        // this.nonce = FindNonce();
        nonce = BlockUtil.FindNonce(difficulty, hash);
    }

    // This method is to forcibly modify the block's data and to make this block invalid for test.
    public void ForceOverwriteData(string? data) => this.data = data;
}