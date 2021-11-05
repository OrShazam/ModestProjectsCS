using System;
using System.Collections.Generic;

namespace ChessBoard
{
    class Engine
    {
        
    }
    enum PieceColor : ushort
    {
        None,White = 1, Black,
    }
    enum PieceType : ushort
    {
        None = 0,Pawn,Bishop = 3,Knight = 4,Rook = 5,Queen = 9,King = 700,
    }
    sealed class Piece
    {
        public Piece() { color = PieceColor.None; type = PieceType.None; }
        public PieceColor color;
        public PieceType type;
    }
    sealed class Board
    {
        Dictionary<char, PieceType> typeMapping = new Dictionary<char, PieceType>();
        private Piece[,] pieces;
        PieceColor currPlayingColor = PieceColor.White;
        bool WLRookMoved, WRRookMoved, BLRookMoved, BRRookMoved,WKingMoved, BKingMoved = false;
        public PieceColor GetCurrPlayer()
        {
            return currPlayingColor;
        }
        public void printBoard()
        {
            for (int i = 7; i >= 0; i--)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (pieces[i,j].type == PieceType.None)
                    {
                        Console.Write("@ ");
                        continue;
                    }
                    Console.ForegroundColor = (pieces[i, j].color == PieceColor.White) ? ConsoleColor.Red
                        : ConsoleColor.Cyan;
                    if (pieces[i,j].type == PieceType.Knight)
                    {
                        Console.Write("N "); Console.ResetColor();continue;
                    }
                    Console.Write((pieces[i, j].type).ToString()[0] + " ");
                    Console.ResetColor();
                }
                Console.WriteLine();
            }
        }
        public Board()
        {
            SetBoard();
            typeMapping.Add('P', PieceType.Pawn); typeMapping.Add('N', PieceType.Knight);
            typeMapping.Add('B', PieceType.Bishop); typeMapping.Add('R', PieceType.Rook);
            typeMapping.Add('Q', PieceType.Queen); typeMapping.Add('K', PieceType.King);
        }
        private int[] getPositionsByType(PieceType type)
        {
            List<int> xyPos = new List<int>();
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (pieces[i,j].type == type)
                    {
                        xyPos.Add(i);xyPos.Add(j);
                    }
                }
            }
            return xyPos.ToArray();

        }
        private void SetBoard()
        {
            pieces = new Piece[8, 8];
            for (int i = 0; i < 8; i++) { for (int j = 0; j < 8; j++) { pieces[i, j] = new Piece(); } }
            for (int i = 0; i < 8; i++)
            {
                pieces[0, i].color = PieceColor.White; pieces[1, i].color = PieceColor.White;
                pieces[1, i].type = PieceType.Pawn;
                pieces[6, i].color = PieceColor.Black; pieces[7, i].color = PieceColor.Black;
                pieces[6, i].type = PieceType.Pawn;
            }
            for (int j = 0; j <= 7; j += 7)
            {
                pieces[j, 0].type = PieceType.Rook; pieces[j, 7].type = PieceType.Rook;
                pieces[j, 1].type = PieceType.Knight; pieces[j, 6].type = PieceType.Knight;
                pieces[j, 2].type = PieceType.Bishop; pieces[j, 5].type = PieceType.Bishop;
                pieces[j, 3].type = PieceType.Queen; pieces[j, 4].type = PieceType.King;
            }
        }
        private int[] PosToInt(string pos)
        {
            if (pos.Length != 2) return new int[] { -1, -1 };
            return new int[] { pos[1] - '1', pos[0] - 'a' };
        }
        public bool Move(string move)
        {
            // move funciton also checks if move is valid
            // will not check if a piece is pinned or if king takes a protected piece
            // simply because the engine won't make such a terrible move
            if (move.Length < 2) return false;
            if (move.Contains('x'))
            {
                if (move[0] <= 'h' && move[0] >= 'a')
                    move = 'P' + move.Substring(1, move.Length - 1);
                move = move[0] + move.Substring(2, move.Length - 2);
            }
            if (move.Length == 2)
            {
                move = 'P' + move;
            }
            int[] xy = PosToInt(move.Substring(1, 2));
            if (xy[0] == -1) return false;
            if (xy[0] > 7 || xy[1] > 7) return false;
            if (!typeMapping.ContainsKey(move[0])) return false;
            return MovePiece(typeMapping[move[0]], xy[0], xy[1]);

        }
        private bool MovePiece(PieceType type, int y, int x)
        {
            int[] xypos = getPositionsByType(type);
            for (int i = 0; i < xypos.Length; i += 2)
            {
                if (CanMove(xypos[i], xypos[i+1], type, y, x))
                {
                    if (currPlayingColor == PieceColor.White) currPlayingColor++;
                    else currPlayingColor--;
                    if (y == 7 && type == PieceType.Pawn)
                    {
                        type = askPromotion();
                    }
                    PieceColor tempColor = pieces[xypos[i], xypos[i+1]].color;
                    pieces[xypos[i], xypos[i+1]] = new Piece();
                    pieces[y,x].type = type;
                    pieces[y,x].color = tempColor;
                    return true;
                }
            }
            return false;
        }
        bool CanMove(int startY, int startX, PieceType type, int targetY, int targetX)
        {
            if (startX == targetX && startY == targetY) return false;
            if (pieces[startY, startX].color == pieces[targetY, targetX].color)
                return false;
            if (pieces[startY, startX].color != currPlayingColor)
                return false;
            if (type == PieceType.Pawn)
                return canMovePawn(startY, startX, targetY, targetX);

            if (type == PieceType.Bishop)
                return canMoveBishop(startY, startX, targetY, targetX);

            if (type == PieceType.Knight)
                return canMoveKnight(startY, startX, targetY, targetX);

            if (type == PieceType.Rook)
                return canMoveRook(startY, startX, targetY, targetX);

            if (type == PieceType.Queen)
                return canMoveQueen(startY, startX, targetY, targetX);

            if (type == PieceType.King)
                return canMoveKing(startY, startX, targetY, targetX);
            return false;
        }
        private bool canMovePawn(int startY, int startX, int targetY, int targetX)
        {
            if (Math.Abs(targetY - startY) == 1)
            {
                if (startX == targetX && pieces[targetY,targetX].type == PieceType.None) return true;
                if (Math.Abs(targetX - startX) == 1)
                {
                    if (pieces[targetY, targetX].type != PieceType.None)
                        return true;
                    if (pieces[targetY, startX].type == PieceType.Pawn)
                        //enpassant 
                        return true;
                }
            }
            else if (Math.Abs(targetY - startY) == 2  && targetX == startX && 
                (startY == 1 || startY == 6))
                return true;
            return false;
        }
        private bool canMoveBishop(int startY, int startX, int targetY, int targetX)
        {
            if (!(startX + startY == targetX + targetY) && !(startX - startY == targetX - targetY))
                // first check if in the same diagonal 
                return false;
            int startI, endI, startJ, endJ;
            if (startX < targetX) { startI = startX; endI = targetX; }
            else { startI = targetX;  endI = startX; }
            if (startY < targetY) { startJ = startY; endJ = targetY; }
            else { startJ = targetY; endJ = startY; }
            startI++;startJ++;
            for (int i = startI; i < endI; i++)
            {
                for (int j = startJ; j < endJ; j++)
                {
                    if (pieces[i, j].type != PieceType.None) // blocked
                        return false;
                }
            }
            return true;
        }
        private bool canMoveKnight(int startY, int startX, int targetY, int targetX)
        {
            int deltaX = Math.Abs(startX - targetX); int deltaY = Math.Abs(startY - targetY);
            return (deltaX == 1 && deltaY == 2) || (deltaX == 2 && deltaY == 1);
        }
        private bool canMoveRook(int startY, int startX, int targetY, int targetX)
        {
            if (startX != targetX && startY != targetY)
                // first check if on the same row or column
                return false;
            bool columnLoop = false;
            int startI = 0, endI = 0;
            if (startX != targetX)
            {
                columnLoop = false;
                if (startX < targetX) { startI = startX; endI = targetX; }
                else { startI = targetX; endI = startX; }
            }
            else if (startY != targetY)
            {
                columnLoop = true;
                if (startY < targetY) { startI = startY; endI = targetY; }
                else { startI = targetY; endI = startY; }
            }
            for (int i = startI + 1; i < endI; i++)
            {
                if (columnLoop)
                {
                    if (pieces[startX, i].type != PieceType.None) // blocked
                        return false;
                }
                else
                {
                    if (pieces[i, startY].type != PieceType.None) // blocked
                        return false;
                }
            }
            return true;
        }
        private bool canMoveQueen(int startY, int startX, int targetY, int targetX)
        {
            return canMoveRook(startX,startY,targetX,targetY) 
                || canMoveBishop(startX,startY,targetX,targetY);
        }
        private bool canMoveKing(int startY, int startX, int targetY, int targetX)
        {
            // CHECK IF KING OR ROOK IS TARGETED AFTER CASTLE IF TRYING TO CASTLE 
            int deltaX = Math.Abs(startX - targetX); int deltaY = Math.Abs(startY - targetY);
            if (deltaX <= 1 && deltaY <= 1) return true;
            if (deltaX == 2 && startY % 7 == 0 && deltaY == 0)
            {
                return (startY == 0) ? WKingMoved : BKingMoved &&
                    pieces[startY, startX + 1].type == PieceType.None &&
                    pieces[startY, startX + 2].type == PieceType.None &&
                    pieces[startY, startX + 3].type == PieceType.Rook &&
                    (startY == 0) ? WRRookMoved : BRRookMoved;
            }
            if (deltaX == 3 && startY % 7 == 0 && deltaY == 0)
            {
                return (startY == 0) ? WKingMoved : BKingMoved &&
                    pieces[startY, startX - 1].type == PieceType.None &&
                    pieces[startY, startX - 2].type == PieceType.None &&
                    pieces[startY, startX - 3].type == PieceType.None &&
                    pieces[startY, startX - 4].type == PieceType.None &&
                    (startY == 0) ? WLRookMoved : BLRookMoved;
            }
            return false;
        }
        private PieceType askPromotion()
        {
            ask:
            Console.Write("What Would you like to promote to? (B,N,R,Q) ");
            string target = Console.ReadLine();
            if (!typeMapping.ContainsKey(target[0]) || target[0] == 'P' || target[0] == 'K')
                goto ask;
            return typeMapping[target[0]];
        }
        public int evaluation() {
            int eval = 0;
            for (int i =0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (pieces[i, j].color == PieceColor.White)
                        eval += (ushort)pieces[i, j].type;
                    else if (pieces[i, j].color == PieceColor.Black)
                        eval -= (ushort)pieces[i, j].type;
                }
            }
            return eval;
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            // just interpreting moves for now
            Board board = new Board();
            Console.WriteLine("Type q to exit...");
            Console.Write("[WHITE]> ");
            string line;
            while ((line = Console.ReadLine()) != "q")
            {
                if (line == "p")
                {
                    board.printBoard();
                    goto printPrompt;
                } 
                if (line == "eval")
                {
                    Console.WriteLine(board.evaluation());
                    goto printPrompt;
                }
                if (!board.Move(line))
                {
                    Console.WriteLine("[ERROR]: Invalid move");
                }
                printPrompt:
                if ((ushort)board.GetCurrPlayer() == 1)
                    Console.Write("[WHITE]> ");
                else
                    Console.Write("[BLACK]> ");
            }
        }
    }
}