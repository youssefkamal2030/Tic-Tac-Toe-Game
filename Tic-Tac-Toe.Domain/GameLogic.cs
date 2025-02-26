using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tic_Tac_Toe.Domain.Entities;
using Tic_Tac_Toe.Domain.Inerfaces;

namespace Tic_Tac_Toe.Domain
{
    public class GameLogic : IGameLogic
    {
        private readonly Random _random = new Random();
        public bool MakeMove(Game game, int row, int col, out string message)
        {
            if (game.Status != "InProgress")
            {
                message = "Game is already over.";
                return false;
            }

            char[,] board = game.GetBoard();
            if (row < 0 || row >= 3 || col < 0 || col >= 3 || board[row, col] != ' ')
            {
                message = "Invalid move.";
                return false;
            }

            board[row, col] = game.CurrentPlayer;
            game.SetBoard(board);

            if (CheckWinner(game, game.CurrentPlayer))
            {
                game.Status = $"{game.CurrentPlayer}Won";
                message = $"Player {game.CurrentPlayer} Won";
                return true;
            }
            if (IsFull(game))
            {
                game.Status = "Draw";
                message = "It's a draw";
                return true;
            }

            game.CurrentPlayer = (game.CurrentPlayer == 'X') ? 'O' : 'X';
            message = $"Player {game.CurrentPlayer}'s turn";
            return true;
        }
        public void MakeAIMove(Game game)
        {
            if (game.Status != "InProgress") return;

            char[,] board = game.GetBoard();
            int blockRow = -1, blockCol = -1;
            for (int i = 0; i < 3; i++)
            {
                if (board[i, 0] == 'X' && board[i, 1] == 'X' && board[i, 2] == ' ')
                {
                    blockRow = i;
                    blockCol = 2;
                    break;
                }
                else if (board[i, 0] == 'X' && board[i, 2] == 'X' && board[i, 1] == ' ')
                {
                    blockRow = i;
                    blockCol = 1;
                    break;
                }
                else if (board[i, 1] == 'X' && board[i, 2] == 'X' && board[i, 0] == ' ')
                {
                    blockRow = i;
                    blockCol = 0;
                    break;
                }
            }
            if (blockRow == -1)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (board[0, j] == 'X' && board[1, j] == 'X' && board[2, j] == ' ')
                    {
                        blockRow = 2;
                        blockCol = j;
                        break;
                    }
                    else if (board[0, j] == 'X' && board[2, j] == 'X' && board[1, j] == ' ')
                    {
                        blockRow = 1;
                        blockCol = j;
                        break;
                    }
                    else if (board[1, j] == 'X' && board[2, j] == 'X' && board[0, j] == ' ')
                    {
                        blockRow = 0;
                        blockCol = j;
                        break;
                    }
                }
            }
            if (blockRow == -1)
            {
                if (board[0, 0] == 'X' && board[1, 1] == 'X' && board[2, 2] == ' ')
                {
                    blockRow = 2;
                    blockCol = 2;
                }
                else if (board[0, 0] == 'X' && board[2, 2] == 'X' && board[1, 1] == ' ')
                {
                    blockRow = 1;
                    blockCol = 1;
                }
                else if (board[1, 1] == 'X' && board[2, 2] == 'X' && board[0, 0] == ' ')
                {
                    blockRow = 0;
                    blockCol = 0;
                }
            }
            if (blockRow == -1)
            {
                if (board[0, 2] == 'X' && board[1, 1] == 'X' && board[2, 0] == ' ')
                {
                    blockRow = 2;
                    blockCol = 0;
                }
                else if (board[0, 2] == 'X' && board[2, 0] == 'X' && board[1, 1] == ' ')
                {
                    blockRow = 1;
                    blockCol = 1;
                }
                else if (board[1, 1] == 'X' && board[2, 0] == 'X' && board[0, 2] == ' ')
                {
                    blockRow = 0;
                    blockCol = 2;
                }
            }
            if (blockRow != -1 && blockCol != -1)
            {
                board[blockRow, blockCol] = 'O';
                game.SetBoard(board);
                game.CurrentPlayer = 'X';
                if (CheckWinner(game, 'O')) game.Status = "OWon";
                else if (IsFull(game)) game.Status = "Draw";
                return;
            }
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                    if (board[i, j] == ' ')
                    {
                        board[i, j] = 'O';
                        if (CheckWinner(game, 'O'))
                        {
                            game.SetBoard(board);
                            game.Status = "OWon";
                            return;
                        }
                        board[i, j] = ' ';
                    }
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                    if (board[i, j] == ' ')
                    {
                        board[i, j] = 'X';
                        if (CheckWinner(game, 'X'))
                        {
                            board[i, j] = 'O';
                            game.SetBoard(board);
                            game.CurrentPlayer = 'X';
                            return;
                        }
                        board[i, j] = ' ';
                    }
            if (board[1, 1] == ' ')
            {
                board[1, 1] = 'O';
                game.SetBoard(board);
                game.CurrentPlayer = 'X';
                return;
            }
            int row, col;
            do
            {
                row = _random.Next(0, 3);
                col = _random.Next(0, 3);
            } while (board[row, col] != ' ');
            board[row, col] = 'O';
            game.SetBoard(board);
            game.CurrentPlayer = 'X';

            if (CheckWinner(game, 'O')) game.Status = "OWon";
            else if (IsFull(game)) game.Status = "Draw";
        }

        public bool CheckWinner(Game game, char player)
        {
            char[,] board = game.GetBoard();
            for (int i = 0; i < 3; i++)
            {
                if (board[i, 0] == player && board[i, 1] == player && board[i, 2] == player) return true;
                if (board[0, i] == player && board[1, i] == player && board[2, i] == player) return true;
            }
            if (board[0, 0] == player && board[1, 1] == player && board[2, 2] == player) return true;
            if (board[0, 2] == player && board[1, 1] == player && board[2, 0] == player) return true;
            return false;
        }

        public bool IsFull(Game game)
        {
            char[,] board = game.GetBoard();
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                    if (board[i, j] == ' ') return false;
            return true;
        }


    }
}
