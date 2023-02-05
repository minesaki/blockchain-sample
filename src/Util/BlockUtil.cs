using System.Security.Cryptography;
using System.Text;

static class BlockUtil
{
    static SHA256 sha256 = SHA256.Create();

    public static string CalcHash(string? prevHash, string? data)
        => CalcSHA256Hash($$"""
        {
            "prevHash": "{{prevHash}}",
            "data": "{{data}}"
        }
        """);


    // This app is just for learning, so this mining process is not optimized. 
    // Generally, we should be careful about memory allocation and outputting messages, which may decrease performance.
    public static string FindNonce(int difficulty, string hash)
    {
        string expected = new string('0', difficulty);
        int nonceNum = 0;
        string nonce, nonceJoinedHash;

        while (true)
        {
            nonce = nonceNum.ToString();
            if (CheckNonce(difficulty, hash, nonce, out nonceJoinedHash))
            {
                break;
            }
            System.Console.WriteLine($"[mining] nonceJoinedHash: {nonceJoinedHash}, nonce: {nonce}");
            nonceNum++;
        }
        Console.WriteLine($"[mining][found] nonceJoinedHash: {nonceJoinedHash}, nonce: {nonce}");
        return nonce;
    }

    // Checking nonce is done very quickly, unlike finding nonce.
    // It's very important because many distributed nodes in the cluster need to verify/accept the block.
    public static bool CheckNonce(int difficulty, string hash, string nonce, out string nonceJoinedHash)
    {
        string expected = new string('0', difficulty);
        nonceJoinedHash = CalcSHA256Hash($"{hash}{nonce}");
        return nonceJoinedHash.StartsWith(expected);
    }

    private static string CalcSHA256Hash(string s)
        => s.GetUTF8Bytes().ToSHA256Hash().ToHexString();

    private static byte[] GetUTF8Bytes(this string s)
        => UTF8Encoding.UTF8.GetBytes(s);

    private static byte[] ToSHA256Hash(this byte[] b)
        => SHA256.HashData(b);

    private static string ToHexString(this byte[] b)
        => b.Aggregate("", (prev, b) => prev + b.ToString("X2"));

}