using System;
using System.Collections.Generic;

//TODO - CASTLE move
//TODO - Check moves
//TODO - pawn fly move
//TODO - pawn replace move
//TODO - DO NOT KILL KING

namespace ChessMove
{   
    //0 - empty
    //1 - pawn
    //2 - rook (tower)
    //3 - knight (horse)
    //4 - bishop
    //5 - queen
    //6 - king
    //<0 - enemy figures
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            List<List<List<int>>> boardStory = new List<List<List<int>>>();
            List<List<int>> board = new List<List<int>>();
            board.Add(new List<int> { 2, 3, 4, 5, 6, 4, 3, 2 });
            board.Add(new List<int> { 1, 1, 1, 1, 1, 1, 1, 1 });
            board.Add(new List<int> { 0, 0, 0, 0, 0, 0, 0, 0 });//2,3
            board.Add(new List<int> { 0, 0, 0, 0, 0, 0, 0, 0 });
            board.Add(new List<int> { 0, 0, 0, 0, 0, 0, 0, 0 });
            board.Add(new List<int> { 0, 0, 0, 0, 0, 0, 0, 0 });
            board.Add(new List<int> { -1, -1, -1, -1, -1, -1, -1, -1 });
            board.Add(new List<int> { -2, -3, -4, -5, -6, -4, -3, -2 });
            showBoard(board);
            boardStory.Add(board);
            GetAllMoves(boardStory, new List<int>{2,3});
            
            
        }
        static void MakeMove(List<List<List<int>>> boardHistory, List<int> pieceStartPosition, List<int> pieceEndPosition) {
            if (IsMoveAllowed(boardHistory, pieceStartPosition, pieceEndPosition)) {
                List<List<int>> newBoard = boardHistory[boardHistory.Count - 1];
                newBoard[pieceEndPosition[0]][pieceEndPosition[1]] = newBoard[pieceStartPosition[0]][pieceStartPosition[1]];
                newBoard[pieceStartPosition[0]][pieceStartPosition[1]] = 0;
                boardHistory.Add(newBoard);
            }
        }
        static bool IsMoveAllowed(List<List<List<int>>> boardHistory, List<int> pieceStartPosition, List<int> pieceEndPosition) {
            List<List<bool>> allowedMoves = GetAllMoves(boardHistory, pieceStartPosition);
            if (allowedMoves[pieceEndPosition[0]][pieceEndPosition[1]]) return true;
            else return false;
        }
        static List<List<bool>> GetAllMoves(List<List<List<int>>> boardHistory, List<int> piecePosition) {
            List<List<bool>> result = new List<List<bool>>();
            result.Add(new List<bool> { false, false, false, false, false, false, false, false });
            result.Add(new List<bool> { false, false, false, false, false, false, false, false });
            result.Add(new List<bool> { false, false, false, false, false, false, false, false });
            result.Add(new List<bool> { false, false, false, false, false, false, false, false });
            result.Add(new List<bool> { false, false, false, false, false, false, false, false });
            result.Add(new List<bool> { false, false, false, false, false, false, false, false });
            result.Add(new List<bool> { false, false, false, false, false, false, false, false });
            result.Add(new List<bool> { false, false, false, false, false, false, false, false });

            List<List<int>> board = boardHistory[boardHistory.Count - 1];
            int piece = board[piecePosition[0]][piecePosition[1]];
            int coordY = piecePosition[0];
            int coordX = piecePosition[1];
            if (piece == 0)
            {

            }
            else if (piece == 1)
            { //PAWN
                //TODO flyHIT
                //Hit move
                if (coordX - 1 >= 0 && coordY + 1 < 8)
                {
                    if (board[coordY + 1][coordX - 1] < 0)
                    {
                        result[coordY + 1][coordX - 1] = true;
                    }
                }
                if (coordX + 1 < 8 && coordY + 1 < 8)
                {
                    if (board[coordY + 1][coordX + 1] < 0)
                    {
                        result[coordY + 1][coordX + 1] = true;
                    }
                }
                //Normal Move
                if (coordY + 1 < 8 && board[coordY + 1][coordX] == 0)
                {
                    result[coordY + 1][coordX] = true;
                }
                if (coordY == 1 && board[coordY + 2][coordX] == 0)
                {
                    result[coordY + 2][coordX] = true;
                }
            }
            else if (piece == 2) //ROOK
            {
                if (coordY + 1 < 8)//UP
                {
                    for (int i = coordY + 1; i < 8; i++)
                    {
                        if (board[i][coordX] > 0)
                        {
                            break;
                        }
                        else if (board[i][coordX] < 0)
                        {
                            result[i][coordX] = true;
                            break;
                        }
                        else
                        {
                            result[i][coordX] = true;
                        }

                    }

                }
                if (coordY - 1 >= 0)//DOWN
                {
                    for (int i = coordY - 1; i >= 0; i--)
                    {
                        if (board[i][coordX] > 0)
                        {
                            break;
                        }
                        else if (board[i][coordX] < 0)
                        {
                            result[i][coordX] = true;
                            break;
                        }
                        else
                        {
                            result[i][coordX] = true;
                        }

                    }

                }
                if (coordX + 1 < 8)
                {
                    for (int i = coordX + 1; i < 8; i++)
                    {
                        if (board[coordY][i] > 0)
                        {
                            break;
                        }
                        else if (board[coordY][i] < 0)
                        {
                            result[coordY][i] = true;
                            break;
                        }
                        else
                        {
                            result[coordY][i] = true;
                        }

                    }

                }
                if (coordX - 1 >= 0)
                {
                    for (int i = coordX - 1; i >= 0; i--)
                    {
                        if (board[coordY][i] > 0)
                        {
                            break;
                        }
                        else if (board[coordY][i] < 0)
                        {
                            result[coordY][i] = true;
                            break;
                        }
                        else
                        {
                            result[coordY][i] = true;
                        }

                    }

                }
            }
            else if (piece == 3) //knight
            {
                if (coordY + 2 < 8)
                {
                    if (coordX - 1 >= 0)
                    {
                        if (board[coordY + 2][coordX - 1] <= 0)
                        {
                            result[coordY + 2][coordX - 1] = true;
                        }
                    }
                    if (coordX + 1 < 8)
                    {
                        if (board[coordY + 2][coordX + 1] <= 0)
                        {
                            result[coordY + 2][coordX + 1] = true;
                        }
                    }
                }
                if (coordY - 2 >= 0)
                {
                    if (coordX - 1 >= 0)
                    {
                        if (board[coordY - 2][coordX - 1] <= 0)
                        {
                            result[coordY - 2][coordX - 1] = true;
                        }
                    }
                    if (coordX + 1 < 8)
                    {
                        if (board[coordY - 2][coordX + 1] <= 0)
                        {
                            result[coordY - 2][coordX + 1] = true;
                        }
                    }
                }
                if (coordX + 2 < 8)
                {
                    if (coordY - 1 >= 0)
                    {
                        if (board[coordY - 1][coordX + 2] <= 0)
                        {
                            result[coordY - 1][coordX + 2] = true;
                        }
                    }
                    if (coordX + 1 < 8)
                    {
                        if (board[coordY + 1][coordX + 2] <= 0)
                        {
                            result[coordY + 1][coordX + 2] = true;
                        }
                    }
                }
                if (coordX - 2 >= 0)
                {
                    if (coordY - 1 >= 0)
                    {
                        if (board[coordY - 1][coordX - 2] <= 0)
                        {
                            result[coordY - 1][coordX - 2] = true;
                        }
                    }
                    if (coordY + 1 < 8)
                    {
                        if (board[coordY + 1][coordX - 2] <= 0)
                        {
                            result[coordY + 1][coordX - 2] = true;
                        }
                    }
                }
            }
            else if (piece == 4) //bishof
            {
                int i = 1;
                while (coordX + i < 8 && coordY + i < 8)
                {
                    if (board[coordY + i][coordX + i] > 0) break;
                    result[coordY + i][coordX + i] = true;
                    if (board[coordY + i][coordX + i] < 0) break;
                    i++;
                }
                i = 1;
                while (coordX + i < 8 && coordY - i >= 0)
                {
                    if (board[coordY - i][coordX + i] > 0) break;
                    result[coordY - i][coordX + i] = true;
                    if (board[coordY - i][coordX + i] < 0) break;
                    i++;
                }
                i = 1;
                while (coordX - i >= 0 && coordY + i < 8)
                {
                    if (board[coordY + i][coordX - i] > 0) break;
                    result[coordY + i][coordX - i] = true;
                    if (board[coordY + i][coordX - i] < 0) break;
                    i++;
                }
                i = 1;
                while (coordX - i >= 0 && coordY - i >= 0)
                {
                    if (board[coordY - i][coordX - i] > 0) break;
                    result[coordY - i][coordX - i] = true;
                    if (board[coordY - i][coordX - i] < 0) break;
                    i++;
                }
            }
            else if (piece == 5) { //queen
                int i = 1;
                while (coordX + i < 8 && coordY + i < 8)
                {
                    if (board[coordY + i][coordX + i] > 0) break;
                    result[coordY + i][coordX + i] = true;
                    if (board[coordY + i][coordX + i] < 0) break;
                    i++;
                }
                i = 1;
                while (coordX + i < 8 && coordY - i >= 0)
                {
                    if (board[coordY - i][coordX + i] > 0) break;
                    result[coordY - i][coordX + i] = true;
                    if (board[coordY - i][coordX + i] < 0) break;
                    i++;
                }
                i = 1;
                while (coordX - i >= 0 && coordY + i < 8)
                {
                    if (board[coordY + i][coordX - i] > 0) break;
                    result[coordY + i][coordX - i] = true;
                    if (board[coordY + i][coordX - i] < 0) break;
                    i++;
                }
                i = 1;
                while (coordX - i >= 0 && coordY - i >= 0)
                {
                    if (board[coordY - i][coordX - i] > 0) break;
                    result[coordY - i][coordX - i] = true;
                    if (board[coordY - i][coordX - i] < 0) break;
                    i++;
                }
                if (coordY + 1 < 8)//UP
                {
                    for ( i = coordY + 1; i < 8; i++)
                    {
                        if (board[i][coordX] > 0)
                        {
                            break;
                        }
                        else if (board[i][coordX] < 0)
                        {
                            result[i][coordX] = true;
                            break;
                        }
                        else
                        {
                            result[i][coordX] = true;
                        }

                    }

                }
                if (coordY - 1 >= 0)//DOWN
                {
                    for ( i = coordY - 1; i >= 0; i--)
                    {
                        if (board[i][coordX] > 0)
                        {
                            break;
                        }
                        else if (board[i][coordX] < 0)
                        {
                            result[i][coordX] = true;
                            break;
                        }
                        else
                        {
                            result[i][coordX] = true;
                        }

                    }

                }
                if (coordX + 1 < 8)
                {
                    for ( i = coordX + 1; i < 8; i++)
                    {
                        if (board[coordY][i] > 0)
                        {
                            break;
                        }
                        else if (board[coordY][i] < 0)
                        {
                            result[coordY][i] = true;
                            break;
                        }
                        else
                        {
                            result[coordY][i] = true;
                        }

                    }

                }
                if (coordX - 1 >= 0)
                {
                    for ( i = coordX - 1; i >= 0; i--)
                    {
                        if (board[coordY][i] > 0)
                        {
                            break;
                        }
                        else if (board[coordY][i] < 0)
                        {
                            result[coordY][i] = true;
                            break;
                        }
                        else
                        {
                            result[coordY][i] = true;
                        }

                    }

                }
            }
            if (piece == 6) {
                if (coordY + 1 < 8 && board[coordY + 1][coordX] <= 0) result[coordY + 1][coordX] = true;
                if (coordY + 1 < 8 && coordX + 1 < 8 && board[coordY + 1][coordX + 1] <= 0) result[coordY + 1][coordX + 1] = true;
                if (coordX + 1 < 8 && board[coordY][coordX+1] <= 0) result[coordY][coordX+1] = true;
                if (coordY - 1 >= 0 && coordX + 1 < 8 && board[coordY - 1][coordX + 1] <= 0) result[coordY - 1][coordX + 1] = true;
                if (coordY - 1 >= 0 && board[coordY - 1][coordX] <= 0) result[coordY-1][coordX] = true;
                if (coordY - 1 >= 0 && coordX - 1 >= 0 && board[coordY - 1][coordX - 1] <= 0) result[coordY - 1][coordX - 1] = true;
                if (coordX - 1 >=0 && board[coordY][coordX - 1] <= 0) result[coordY][coordX - 1] = true;
                if (coordY + 1 < 8 && coordX - 1 >= 0 && board[coordY + 1][coordX - 1] <= 0) result[coordY + 1][coordX - 1] = true;
            }
            Console.Write("    | A| B| C| D| E| F| G| H|\n\n");
            for (int i = 7; i >= 0; i--)
            {
                int fi = i + 1;
                Console.Write("[" + fi.ToString() + "] |");
                List<bool> row = result[i];
                for (int j = 0; j < 8; j++) {
                    if (i == piecePosition[0] && j == piecePosition[1])
                    {
                        Console.Write(" X|");
                    }
                    else if (result[i][j])
                    {
                        Console.Write(" O|");
                    }
                    else {
                        Console.Write("  |");
                    }
                }
                Console.Write(" [" + fi.ToString() + "]\n");
            }
            Console.Write("\n    | A| B| C| D| E| F| G| H|");

            return result;
        }
        static void showBoard(List<List<int>> board) {
            Console.Write("    | A| B| C| D| E| F| G| H|\n\n");
            for (int i = 7; i >= 0; i--)
            {
                int fi = i + 1;
                Console.Write("[" + fi.ToString() + "] |");
                List<int> row = board[i];
                foreach (int pawn in row)
                {
                    String strpawn;
                    if (pawn >= 0)
                        strpawn = " " + pawn.ToString();
                    else
                        strpawn = pawn.ToString();
                    Console.Write(strpawn + "|");
                }
                Console.Write(" [" + fi.ToString() + "]\n");
            }
            Console.Write("\n    | A| B| C| D| E| F| G| H|\n\n\n");
        }
    }
}
