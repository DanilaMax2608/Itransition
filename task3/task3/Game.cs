using System;
using System.Collections.Generic;

class Game
{
    private List<string> moves;
    private Rules rules;
    private KeyGenerator keyGenerator;
    private HmacCalculator hmacCalculator;

    public Game(string[] args)
    {
        if (!IsValidArgs(args))
        {
            ShowHelp();
            Environment.Exit(1);
        }

        moves = new List<string>(args);
        rules = new Rules(moves);
        keyGenerator = new KeyGenerator();
        hmacCalculator = new HmacCalculator();
    }

    public void Start()
    {
        byte[] key = keyGenerator.GenerateKey();
        string computerMove = GetComputerMove();
        string hmac = hmacCalculator.CalculateHmac(computerMove, key);

        Console.WriteLine($"HMAC: {hmac}");

        ShowMenu();
        int userChoice = GetUserChoice();
        if (userChoice == 0) return;

        string userMove = moves[userChoice - 1];
        Console.WriteLine($"Your move: {userMove}");
        Console.WriteLine($"Computer move: {computerMove}");

        string result = rules.DetermineResult(userMove, computerMove);
        Console.WriteLine(result);

        Console.WriteLine($"HMAC key: {BitConverter.ToString(key).Replace("-", "")}");
    }

    private bool IsValidArgs(string[] args)
    {
        if (args.Length < 3 || args.Length % 2 == 0)
        {
            return false;
        }

        HashSet<string> uniqueMoves = new HashSet<string>(args);
        return uniqueMoves.Count == args.Length;
    }

    private void ShowHelp()
    {
        Console.WriteLine("Usage: game.exe move1 move2 move3 ... moveN");
        Console.WriteLine("Move count must be an odd number and greater than or equal to 3.");
    }

    private void ShowMenu()
    {
        Console.WriteLine("Available moves:");
        for (int i = 0; i < moves.Count; i++)
        {
            Console.WriteLine($"{i + 1} - {moves[i]}");
        }
        Console.WriteLine("0 - exit");
        Console.WriteLine("? - help");
    }

    private int GetUserChoice()
    {
        while (true)
        {
            string input = Console.ReadLine();
            if (input == "?")
            {
                ShowHelpTable(); 
                continue; 
            }
            if (int.TryParse(input, out int choice) && choice >= 0 && choice <= moves.Count)
            {
                return choice;
            }
            Console.WriteLine("Invalid choice. Please enter a number between 0 and " + moves.Count);
        }
    }

    private string GetComputerMove()
    {
        Random random = new Random();
        int index = random.Next(moves.Count);
        return moves[index];
    }

    private void ShowHelpTable()
    {
        int n = moves.Count;

        Console.Write("+-------------+");
        foreach (var move in moves)
        {
            Console.Write(new string('-', move.Length).PadRight(8, '-') + "+");
        }
        Console.WriteLine();

        Console.Write("| v PC/User > | ");
        foreach (var move in moves)
        {
            Console.Write($"{move.PadRight(8)}| ");
        }
        Console.WriteLine();

        Console.Write("+-------------+");
        foreach (var move in moves)
        {
            Console.Write(new string('-', 8) + "+");
        }
        Console.WriteLine();

        for (int i = 0; i < n; i++)
        {
            Console.Write($"| {moves[i].PadRight(11)}| ");
            for (int j = 0; j < n; j++)
            {
                if (i == j)
                {
                    Console.Write("Draw".PadRight(8) + "| ");
                }
                else if ((j - i + n) % n <= n / 2)
                {
                    Console.Write("Win".PadRight(8) + "| ");
                }
                else
                {
                    Console.Write("Lose".PadRight(8) + "| ");
                }
            }
            Console.WriteLine();

            Console.Write("+-------------+");
            foreach (var move in moves)
            {
                Console.Write(new string('-', 8) + "+");
            }
            Console.WriteLine();
        }
    }

}
