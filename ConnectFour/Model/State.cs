namespace ConnectFour.Model
{
    /// <summary>
    /// ゲーム状態
    /// </summary>
    public class State
    {
        Player[] players;
        Piece[] pieces;
        int gameRoundsPlayed;
        bool gameOver;

        public State(int pieceCount)
        {
            players =
            [
                new Player() { Name = "Player", Points = 0 },
                new Player() { Name = "Opponent", Points=0 },
            ];
            pieces = new Piece[ pieceCount ];
            gameRoundsPlayed = 0;
            gameOver = false;
        }

        public void ResetGame()
        {
            gameOver = false;
            players[0].Points = 0;
            players[1].Points = 0;
        }

        public void EndGame()
        {
            gameOver = true;
            gameRoundsPlayed++;
        }

        public void PlayPiece(int position)
        {
            pieces[position].Occupied = true;
        }
    }
}
