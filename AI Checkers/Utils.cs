using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace AICheckers
{
    static class Utils
    {
        //Convenience method
        public static Move[] GetOpenSquares(Square[,] Board, Point checker)
        {
            return GetOpenSquares(Board, checker, new Move(-1, -1, -1, -1), null);
        }

        private static Move[] GetOpenSquares(Square[,] Board, Point checker, Move lastMove, List<Point> priorPositions)
        {
            if (priorPositions == null)
            {
                priorPositions = new List<Point>();
                priorPositions.Add(checker);
            }

            List<Move> OpenSquares = new List<Move>();

            //Top Left
            if (Board[priorPositions[0].Y, priorPositions[0].X].Colour != CheckerColour.Red || Board[priorPositions[0].Y, priorPositions[0].X].King)        //Stop regular red pieces from moving backwards
            {
                if (IsValidPoint(checker.X - 1, checker.Y - 1))
                {
                    if (Board[checker.Y - 1, checker.X - 1].Colour == CheckerColour.Empty && lastMove.Destination.X == -1)  //Allow immediate empty spaces if it's the first jump
                    {
                        OpenSquares.Add(new Move(priorPositions[0], checker.X - 1, checker.Y - 1));
                    }
                    //Check for a capturable piece
                    else if (IsValidPoint(checker.X - 2, checker.Y - 2)
                        && ((checker.X - 2) != lastMove.Destination.X || (checker.Y - 2) != lastMove.Destination.Y)
                        && ((checker.X - 2) != priorPositions[0].X || (checker.Y - 2) != priorPositions[0].Y)
                        && Board[checker.Y - 1, checker.X - 1].Colour != Board[checker.Y, checker.X].Colour
                        && Board[checker.Y - 2, checker.X - 2].Colour == CheckerColour.Empty)
                    {
                        Point newDest = new Point(checker.X - 2, checker.Y - 2);
                        if (!priorPositions.Contains(newDest))
                        {
                            Move move = new Move(priorPositions[0], newDest);
                            move.Captures.Add(new Point(checker.X - 1, checker.Y - 1));
                            move.Captures.AddRange(lastMove.Captures);
                            OpenSquares.Add(move);

                            priorPositions.Add(newDest);

                            //Use recursion to find multiple captures                        
                            OpenSquares.AddRange(GetOpenSquares(Board, new Point(checker.X - 2, checker.Y - 2), move, priorPositions));
                        }
                    }
                }
            }

            //Top Right
            if (Board[priorPositions[0].Y, priorPositions[0].X].Colour != CheckerColour.Red || Board[priorPositions[0].Y, priorPositions[0].X].King)
            {
                if (IsValidPoint(checker.X + 1, checker.Y - 1))
                {
                    if (Board[checker.Y - 1, checker.X + 1].Colour == CheckerColour.Empty && lastMove.Destination.X == -1)
                    {
                        OpenSquares.Add(new Move(priorPositions[0], checker.X + 1, checker.Y - 1));
                    }
                    //Check for a capturable piece
                    else if (IsValidPoint(checker.X + 2, checker.Y - 2)
                        && ((checker.X + 2) != lastMove.Destination.X || (checker.Y - 2) != lastMove.Destination.Y)
                        && ((checker.X + 2) != priorPositions[0].X || (checker.Y - 2) != priorPositions[0].Y)
                        && Board[checker.Y - 1, checker.X + 1].Colour != Board[checker.Y, checker.X].Colour
                        && Board[checker.Y - 2, checker.X + 2].Colour == CheckerColour.Empty)
                    {
                        Point newDest = new Point(checker.X + 2, checker.Y - 2);
                        if (!priorPositions.Contains(new Point(checker.X + 2, checker.Y - 2)))
                        {
                            Move move = new Move(priorPositions[0], newDest);
                            move.Captures.Add(new Point(checker.X + 1, checker.Y - 1));
                            move.Captures.AddRange(lastMove.Captures);
                            OpenSquares.Add(move);

                            priorPositions.Add(newDest);

                            //Use recursion to find multiple captures
                            OpenSquares.AddRange(GetOpenSquares(Board, new Point(checker.X + 2, checker.Y - 2), move, priorPositions));
                        }
                    }
                }
            }

            //Bottom Left
            if (Board[priorPositions[0].Y, priorPositions[0].X].Colour != CheckerColour.Black || Board[priorPositions[0].Y, priorPositions[0].X].King)
            {
                if (IsValidPoint(checker.X - 1, checker.Y + 1))
                {
                    if (Board[checker.Y + 1, checker.X - 1].Colour == CheckerColour.Empty && lastMove.Destination.X == -1)
                    {
                        OpenSquares.Add(new Move(priorPositions[0], checker.X - 1, checker.Y + 1));
                    }
                    //Check for a capturable piece
                    else if (IsValidPoint(checker.X - 2, checker.Y + 2)
                        && ((checker.X - 2) != lastMove.Destination.X || (checker.Y + 2) != lastMove.Destination.Y)
                        && ((checker.X - 2) != priorPositions[0].X || (checker.Y + 2) != priorPositions[0].Y)
                        && Board[checker.Y + 1, checker.X - 1].Colour != Board[checker.Y, checker.X].Colour
                        && Board[checker.Y + 2, checker.X - 2].Colour == CheckerColour.Empty)
                    {
                        Point newDest = new Point(checker.X - 2, checker.Y + 2);
                        if (!priorPositions.Contains(newDest))
                        {
                            Move move = new Move(priorPositions[0], newDest);
                            move.Captures.Add(new Point(checker.X - 1, checker.Y + 1));
                            move.Captures.AddRange(lastMove.Captures);
                            OpenSquares.Add(move);

                            priorPositions.Add(newDest);

                            //Use recursion to find multiple captures
                            OpenSquares.AddRange(GetOpenSquares(Board, new Point(checker.X - 2, checker.Y + 2), move, priorPositions));
                        }
                    }
                }
            }

            //Bottom Right
            if (Board[priorPositions[0].Y, priorPositions[0].X].Colour != CheckerColour.Black || Board[priorPositions[0].Y, priorPositions[0].X].King)
            {
                if (IsValidPoint(checker.X + 1, checker.Y + 1))
                {
                    if (Board[checker.Y + 1, checker.X + 1].Colour == CheckerColour.Empty && lastMove.Destination.X == -1)
                    {
                        OpenSquares.Add(new Move(priorPositions[0], checker.X + 1, checker.Y + 1));
                    }
                    //Check for a capturable piece
                    else if (IsValidPoint(checker.X + 2, checker.Y + 2)
                        && ((checker.X + 2) != lastMove.Destination.X || (checker.Y + 2) != lastMove.Destination.Y)
                        && ((checker.X + 2) != priorPositions[0].X || (checker.Y + 2) != priorPositions[0].Y)
                        && Board[checker.Y + 1, checker.X + 1].Colour != Board[checker.Y, checker.X].Colour
                        && Board[checker.Y + 2, checker.X + 2].Colour == CheckerColour.Empty)
                    {
                        Point newDest = new Point(checker.X + 2, checker.Y + 2);
                        if (!priorPositions.Contains(newDest))
                        {
                            Move move = new Move(priorPositions[0], newDest);
                            move.Captures.Add(new Point(checker.X + 1, checker.Y + 1));
                            move.Captures.AddRange(lastMove.Captures);
                            OpenSquares.Add(move);

                            priorPositions.Add(newDest);

                            //Use recursion to find multiple captures
                            OpenSquares.AddRange(GetOpenSquares(Board, new Point(checker.X + 2, checker.Y + 2), move, priorPositions));
                        }
                    }
                }
            }

            return OpenSquares.ToArray();
        }

        private static bool IsValidPoint(int x, int y)
        {
            if (0 <= x && x < 8 && 0 <= y && y < 8) return true;
            return false;
        }

        private static bool IsValidPoint(Point point)
        {
            return (IsValidPoint(point.X, point.Y));
        }
    }
}
