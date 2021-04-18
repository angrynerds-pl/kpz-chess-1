using System;
using UnityEngine;
using System.Collections.Generic;
using Zenject;

public class ChessMoveLogic : IChessMoveLogic, IInitializable
{
    public List<List<List<int>>> boardStory { get; private set; } = new List<List<List<int>>>();
    public List<List<int>> board { get; private set; } = new List<List<int>>();

    public void Initialize()
    {
        board.Add(new List<int> { 2, 3, 4, 6, 5, 4, 3, 2 });
        board.Add(new List<int> { 1, 1, 1, 1, 1, 1, 1, 1 });
        board.Add(new List<int> { 0, 0, 0, 0, 0, 0, 0, 0 });//2,3
        board.Add(new List<int> { 0, 0, 0, 0, 0, 0, 0, 0 });
        board.Add(new List<int> { 0, 0, 0, 0, 0, 0, 0, 0 });
        board.Add(new List<int> { 0, 0, 0, 0, 0, 0, 0, 0 });
        board.Add(new List<int> { -1, -1, -1, -1, -1, -1, -1, -1 });
        board.Add(new List<int> { -2, -3, -4, -6, -5, -4, -3, -2 });
        //showBoard(board);
        boardStory.Add(board);
    }


    public void MakeEnemyMove(List<int> pieceStartPosition, List<int> pieceEndPosition)
    {
        List<List<int>> newBoard = boardStory[boardStory.Count - 1];
        newBoard[pieceEndPosition[0]][pieceEndPosition[1]] = newBoard[pieceStartPosition[0]][pieceStartPosition[1]];
        newBoard[pieceStartPosition[0]][pieceStartPosition[1]] = 0;
        boardStory.Add(newBoard);
    }


    public void MakeMove(List<int> pieceStartPosition, List<int> pieceEndPosition)
    {
        if (isCastle(pieceStartPosition, pieceEndPosition))
            {
                if (isCastleAllowed(boardHistory, pieceStartPosition, pieceEndPosition))
                {
                    List<List<int>> newBoard = boardHistory[boardHistory.Count - 1];
                    if (pieceEndPosition[1] == 0)//big castle
                    {
                        newBoard[pieceStartPosition[0]][2] = 6;
                        newBoard[pieceEndPosition[0]][3] = 2;
                        newBoard[pieceStartPosition[0]][pieceStartPosition[1]] = 0;
                        newBoard[pieceEndPosition[0]][pieceEndPosition[1]] = 0;
                        boardHistory.Add(newBoard);
                    }
                    else
                    {//small castle
                        newBoard[pieceStartPosition[0]][6] = 6;
                        newBoard[pieceEndPosition[0]][5] = 2;
                        newBoard[pieceStartPosition[0]][pieceStartPosition[1]] = 0;
                        newBoard[pieceEndPosition[0]][pieceEndPosition[1]] = 0;
                        boardHistory.Add(newBoard);
                    }
                    return;
                }
                
            }
            if (boardHistory[boardHistory.Count - 1][pieceStartPosition[0]][pieceStartPosition[1]] == 1 || (isBlackGlobal && boardHistory[boardHistory.Count - 1][pieceStartPosition[0]][pieceStartPosition[1]] == -1)) {
                if (isEnPassant(boardHistory, pieceStartPosition, new List<int> {pieceEndPosition[0]+1,pieceEndPosition[1]})|| isEnPassant(boardHistory, pieceStartPosition, new List<int> { pieceEndPosition[0] - 1, pieceEndPosition[1] })) {
                    List <List<int>> newBoard = boardHistory[boardHistory.Count - 1];
                    newBoard[pieceEndPosition[0]][pieceEndPosition[1]] = newBoard[pieceStartPosition[0]][pieceStartPosition[1]];
                    if (isBlackGlobal)
                    {
                        newBoard[pieceEndPosition[0] + 1][pieceEndPosition[1]] = 0;
                    }
                    else {
                        newBoard[pieceEndPosition[0] - 1][pieceEndPosition[1]] = 0;
                        Console.Write(pieceEndPosition[0] - 1 + " " + pieceEndPosition[1]);
                    }
                    newBoard[pieceStartPosition[0]][pieceStartPosition[1]] = 0;
                    boardHistory.Add(newBoard);
                    return;
                }
                
            }

            if (IsMoveAllowed(boardHistory, pieceStartPosition, pieceEndPosition))
            {
                List<List<int>> newBoard = boardHistory[boardHistory.Count - 1];
                newBoard[pieceEndPosition[0]][pieceEndPosition[1]] = newBoard[pieceStartPosition[0]][pieceStartPosition[1]];
                newBoard[pieceStartPosition[0]][pieceStartPosition[1]] = 0;
                boardHistory.Add(newBoard);
            }
        else
        {
            Debug.Log("Move is not allowed: " + pieceStartPosition[0] + " " + 
                pieceStartPosition[1] + " -> " + pieceEndPosition[0] + " " +
                pieceEndPosition[1]);
        }
    }


    public bool IsMoveAllowed(List<int> pieceStartPosition, List<int> pieceEndPosition)
    {
        List<List<bool>> allowedMoves = GetAllMoves(pieceStartPosition);
        if (allowedMoves[pieceEndPosition[0]][pieceEndPosition[1]]) return true;
        else return false;
    }

	static bool isCastle(List<int> pieceStartPosition, List<int> pieceEndPosition) {
        int kx = pieceStartPosition[0];
        int ky = pieceStartPosition[1];
        int rx = pieceEndPosition[0];
        int ry = pieceEndPosition[1];
        if ((ky == 4 && ((isBlackGlobal && kx == 7) || kx == 0)) && ((ry == 0 && ((isBlackGlobal && rx == 7) || rx == 0)) || (ry == 7 && ((isBlackGlobal && rx == 7) || rx == 0)))) return true;
        else return false;
    }
    static bool isCastleAllowed(List<List<List<int>>> boardHistory, List<int> pieceStartPosition, List<int> pieceEndPosition) {
        List<List<int>> boardNow = boardHistory[boardHistory.Count - 1];
        int ky = pieceStartPosition[1];//4
        int ry = pieceEndPosition[1];//0 OR 7
        if (isCheck(boardHistory, boardNow)) return false;
        if (ky > ry) //0
        {
            for (int i = ky - 1; i > ry; i--)
            {
                if (boardNow[pieceStartPosition[0]][i] != 0) return false;
            }
        }
        else {
            for (int i = ky + 1; i < ry; i++) {
                if (boardNow[pieceStartPosition[0]][i] != 0) return false;
            }
        }
        for (int i = 0; i < boardHistory.Count; i++) {
            if (boardHistory[i][pieceStartPosition[0]][pieceStartPosition[1]] != 6 || boardHistory[i][pieceEndPosition[0]][pieceEndPosition[1]] != 2) {
                return false;
            }
        }
        return true;
    }
    static bool isEnPassant(List<List<List<int>>> boardHistory, List<int> pieceStartPosition, List<int> pieceEndPosition) {
        List<List<int>> boardNow = boardHistory[boardHistory.Count - 1];
        List<List<int>> boardBefore = boardHistory[boardHistory.Count - 2];
        if ((pieceStartPosition[1] - 1 >= 0 || pieceEndPosition[1] + 1 < 8) && (pieceStartPosition[0] == 4 || (isBlackGlobal && pieceStartPosition[0] == 3))) {
            if (pieceEndPosition[0] == pieceStartPosition[0] && (pieceEndPosition[1] == pieceStartPosition[1] - 1 || pieceEndPosition[1] == pieceStartPosition[1] + 1))
            {
                if ((boardNow[pieceStartPosition[0]][pieceStartPosition[1]] == 1 && boardNow[pieceEndPosition[0]][pieceEndPosition[1]] == -1) || (boardNow[pieceStartPosition[0]][pieceStartPosition[1]] == -1 || boardNow[pieceEndPosition[0]][pieceEndPosition[1]] == 1))
                {
                    if (boardNow[pieceEndPosition[0]][pieceEndPosition[1]] == -1)
                    {
                        if (boardBefore[pieceEndPosition[0] + 2][pieceEndPosition[1]] == boardNow[pieceEndPosition[0]][pieceEndPosition[1]])
                        {
                            return true;
                        }
                        else return false;
                    }
                    else
                    {
                        if (boardBefore[pieceEndPosition[0] - 2][pieceEndPosition[1]] == boardNow[pieceEndPosition[0]][pieceEndPosition[1]])
                        {
                            return true;
                        }
                        else return false;
                    }
                }
                else return false;
            }
            else return false;
        }
        else return false;
    }
    static bool isMate(List<List<List<int>>> boardHistory)
    {
        List<List<int>> board = new List<List<int>>(boardHistory[boardHistory.Count - 1]);
        for (int i = 0; i < board.Count; i++)
        {
            List<int> list = board[i];
            for (int j = 0; j < list.Count; j++)
            {
                int cell = list[j];
              
                    bool tempIsBlack = false;
                    
                    if (isBlackGlobal) tempIsBlack = true;
                    if (board[i][j] > 0 || (board[i][j] < 0 && tempIsBlack)) {
                        bool isMove = false;
                        List<List<bool>> allowedMoves = GetAllMoves(boardHistory, new List<int> { i, j }, tempIsBlack, false);
                        for (int k = 0; k < allowedMoves.Count; k++)
                        {
                            List<bool> listtwo = allowedMoves[k];
                            for (int l = 0; l < listtwo.Count; l++)
                            {
                                bool celltwo = listtwo[l];
                                if (celltwo)
                                {
                                    return false;
                                }
                            }
                        }
                    }
                
            }
        }
        return true;
    }
    static bool isCheck(List<List<List<int>>> boardHistory, List<List<int>> board)
    {
        for (int i = 0; i < board.Count; i++)
        {
            List<int> list = board[i];
            for (int j = 0; j < list.Count; j++)
            {
                int cell = list[j];
                if (cell < 0)
                {
                    bool tempIsBlack = true;
                    if (isBlackGlobal) tempIsBlack = false;
                  
                    List<List<bool>> allowedMoves = GetAllMoves(boardHistory, new List<int> { i, j }, tempIsBlack, true);
                    for (int k = 0; k < allowedMoves.Count; k++) {
                        List<bool> listtwo = allowedMoves[k];
                        for (int l = 0; l < listtwo.Count; l++) {
                            bool celltwo = listtwo[l];
                            if (celltwo) {
                                if (board[k][l] == 6) return true;
                            }
                        }
                    }
                }
            }
        }

        return false;
    }
		
    static List<List<bool>> GetAllMoves(List<List<List<int>>> boardHistory, List<int> piecePosition, bool isBlack, bool isAlt = false) {
            List<List<bool>> result = new List<List<bool>>();
            result.Add(new List<bool> { false, false, false, false, false, false, false, false });
            result.Add(new List<bool> { false, false, false, false, false, false, false, false });
            result.Add(new List<bool> { false, false, false, false, false, false, false, false });
            result.Add(new List<bool> { false, false, false, false, false, false, false, false });
            result.Add(new List<bool> { false, false, false, false, false, false, false, false });
            result.Add(new List<bool> { false, false, false, false, false, false, false, false });
            result.Add(new List<bool> { false, false, false, false, false, false, false, false });
            result.Add(new List<bool> { false, false, false, false, false, false, false, false });

            

            List<List<int>> tempBoard = new List<List<int>>(boardHistory[boardHistory.Count - 1]);
            List<List<int>> board; 
            int piece = tempBoard[piecePosition[0]][piecePosition[1]];
            int coordY;
            int coordX; 
            if (isBlack)
            {
                piece *= -1;
                board = new List<List<int>>();
                for (int i = tempBoard.Count - 1; i > -1; i--) {
                    List<int> templist = tempBoard[i];
                    List<int> list = new List<int>();
                    for (int j = templist.Count - 1; j > -1; j--) {
                        list.Add(templist[j] * -1);
                    }
                    board.Add(list);
                }
                coordY = 7-piecePosition[0];
                coordX = 7-piecePosition[1];
            }
            else {
                board = new List<List<int>>(tempBoard);
                coordY = piecePosition[0];
                coordX = piecePosition[1];
            }

            //showBoard(tempBoard);

            if (piece == 0)
            {

            }
            else if (piece == 1)
            { //PAWN

                //Hit move
                if (coordX - 1 >= 0 || coordX + 1 < 8 && coordY == 4) {
                    if (isEnPassant(boardHistory, piecePosition, new List<int> { piecePosition[0], piecePosition[1] - 1 })) {
                        result[coordY + 1][coordX - 1] = true;
                    }
                    else if(isEnPassant(boardHistory, piecePosition, new List<int> { piecePosition[0], piecePosition[1] + 1 }))
                    {
                        result[coordY + 1][coordX + 1] = true;
                    }
                }

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
                if (coordY == 1 && board[coordY + 2][coordX] == 0 && board[coordY + 1][coordX] == 0)
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
                if (!isAlt)
                {
                    if (isCastleAllowed(boardHistory, piecePosition, new List<int> { piecePosition[0], 0 })) { result[piecePosition[0]][0] = true; }
                    if (isCastleAllowed(boardHistory, piecePosition, new List<int> { piecePosition[0], 7 })) { result[piecePosition[0]][7] = true; }
                }            
            }

            if (isBlack) {
                List<List<bool>> tempResult = new List<List<bool>>();
                for (int i = result.Count - 1; i > -1; i--)
                {
                    List<bool> templist = result[i];
                    List<bool> list = new List<bool>();
                    for (int j = templist.Count - 1; j > -1; j--)
                    {
                        list.Add(templist[j]);
                        
                    }
                    tempResult.Add(list);
                }
                result = tempResult;
            }
            if (!isAlt)
            {
                for (int i = result.Count - 1; i > -1; i--)
                {
                    List<bool> list = result[i];
                    for (int j = list.Count - 1; j > -1; j--)
                    {
                        if (list[j])
                        {
                            
                            List<List<int>> altBoard = new List<List<int>>(tempBoard);
                            int old = altBoard[i][j];
                            altBoard[i][j] = altBoard[piecePosition[0]][piecePosition[1]];
                            
                            altBoard[piecePosition[0]][piecePosition[1]] = 0;
                            List<List<List<int>>> altBoardHistory = new List<List<List<int>>>(boardHistory);
                            altBoardHistory.Add(altBoard);
                            
                            if (isCheck(altBoardHistory, altBoard))
                            {
                                result[i][j] = false;
                            }
                            altBoard[piecePosition[0]][piecePosition[1]] = altBoard[i][j];
                            altBoard[i][j] = old;
                        }
                    }
                }
            }
            



            return result;
        }
    }
   /* static void showBoard(List<List<int>> board)
    {
        //Console.Write("    | A| B| C| D| E| F| G| H|\n\n");
        for (int i = 7; i >= 0; i--)
        {
            int fi = i + 1;
            //Console.Write("[" + fi.ToString() + "] |");
            List<int> row = board[i];
            foreach (int pawn in row)
            {
                String strpawn;
                if (pawn >= 0)
                    strpawn = " " + pawn.ToString();
                else
                    strpawn = pawn.ToString();
                //Console.Write(strpawn + "|");
            }
            //Console.Write(" [" + fi.ToString() + "]\n");
        }
        //Console.Write("\n    | A| B| C| D| E| F| G| H|\n\n\n");
    }*/

    
}