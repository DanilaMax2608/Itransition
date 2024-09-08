using System.Collections.Generic;

class Rules
{
    private List<string> moves;
    private Dictionary<string, HashSet<string>> winConditions;

    public Rules(List<string> moves)
    {
        this.moves = moves;
        winConditions = new Dictionary<string, HashSet<string>>();

        int numMoves = moves.Count;
        for (int i = 0; i < numMoves; i++)
        {
            string move = moves[i];
            HashSet<string> winSet = new HashSet<string>();
            for (int j = 1; j <= numMoves / 2; j++)
            {
                int winIndex = (i + j) % numMoves;
                winSet.Add(moves[winIndex]);
            }
            winConditions[move] = winSet;
        }
    }

    public string DetermineResult(string userMove, string computerMove)
    {
        if (userMove == computerMove)
        {
            return "Draw!";
        }
        if (winConditions[userMove].Contains(computerMove))
        {
            return "You win!";
        }
        return "You lose!";
    }
}
