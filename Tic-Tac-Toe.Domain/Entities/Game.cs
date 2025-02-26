using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tic_Tac_Toe.Domain.Entities
{
    public class Game
    {
        public int Id { get; set; }
        public string Board { get; set; } = "         "; 
        public char CurrentPlayer { get; set; } = 'X';
        public string Status { get; set; } = "InProgress";
        public DateTime StartedAt { get; set; } = DateTime.Now;
        public bool IsSinglePlayer { get; set; } = false;

        public char[,] GetBoard()
        {
            char[,] board = new char[3, 3];
            for (int i = 0; i < 9; i++)
                board[i / 3, i % 3] = Board[i];
            return board;
        }

        public void SetBoard(char[,] board)
        {
            char[] flat = new char[9];
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                    flat[i * 3 + j] = board[i, j];
            Board = new string(flat);
        }
    }
}
