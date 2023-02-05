
// Decide "difficulty" (See Block.cs for explanation)
Console.Write("Input a positive number for difficulty (Default=4): ");
if (!int.TryParse(Console.ReadLine(), out int difficulty))
{
    Console.WriteLine("No input or invalid number. Setting to the default: 4");
    difficulty = 4;
}

// Initialize
BlockchainService blockchain = new(difficulty);

// Read user's input and execute commands interactively
while (true)
{
    Console.Write("Type command [a(Add)/l(List)/m(Modify)/q(Quit)]: ");

    switch (Console.ReadLine()?.ToLower())
    {
        case "a":
            // Add data to blockchain
            Console.Write("Type text to store: ");
            string? data = Console.ReadLine();
            blockchain.AddBlock(data);
            break;

        case "l":
            // List data in blockchain (and check data validity)
            blockchain.List();
            break;

        case "m":
            // Forcibly overwrite data in blockchain
            Console.WriteLine("For test of verification function. Overwritten data will be invalid.");
            Console.Write("Type index of data to modify [0-{0}] :", blockchain.GetBlockchainLength() - 1);
            if (!int.TryParse(Console.ReadLine(), out int index)) goto default;

            Console.Write($"old data: {blockchain.GetBlockDataAt(index)} -> new data: ");
            string? newData = Console.ReadLine();
            blockchain.ForceOverwriteDataAt(index, newData);
            break;

        case "q":
            // Exit app
            return;

        default:
            // Error
            System.Console.WriteLine("Unknown command or invalid input.");
            break;
    }
}