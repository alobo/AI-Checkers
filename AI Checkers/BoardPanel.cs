using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using AICheckers.Properties;

namespace AICheckers
{
    public class BoardPanel : Panel
    {
        IAI AI = null;

        //Assets
        Image checkerRed = Resources.checkerred;
        Image checkerRedKing = Resources.checkerredking;
        Image checkerBlack = Resources.checkerblack;
        Image checkerBlackKing = Resources.checkerblackking;

        Color darkSquare = Color.DarkGray;
        Color lightSquare = Color.Gainsboro;

        bool animating = false;
        const int animDuration = 1000;
        Square animPiece;
        Point oldPoint = new Point(-1, -1);
        Point currentPoint = new Point(-1, -1);
        Point newPoint = new Point(-1, -1);
        int delta = 10;

        int squareWidth = 0;

        Point selectedChecker = new Point(-1, -1);
        List<Move> possibleMoves = new List<Move>();
        //List<Point> highlightedSquares = new List<Point>();

        CheckerColour currentTurn = CheckerColour.Black;
        private System.ComponentModel.IContainer components;

        Square[,] Board = new Square[8,8];
        
        public BoardPanel()
            : base()
        {
            //this.DoubleBuffered = true;
            this.ResizeRedraw = true;
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint | ControlStyles.Opaque | ControlStyles.AllPaintingInWmPaint, true);

            //Initialize board
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    Board[i, j] = new Square();
                    Board[i, j].Colour = CheckerColour.Empty;
                }
            }

            //Setup Pieces
            for (int i = 0; i < 8; i += 1)
            {
                int offset = 0;
                if (i % 2 != 0)
                {
                    offset++;
                }
                for (int j = offset; j < 8; j += 2)
                {
                    if (i < 3) Board[i, j].Colour = CheckerColour.Red;
                    if (i > 4) Board[i, j].Colour = CheckerColour.Black;
                }
            }

            AI = new AI_Tree();
            AI.Colour = CheckerColour.Black;

            AdvanceTurn();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            //Logic

            //Draw
            base.OnPaint(e);                        
            e.Graphics.Clear(lightSquare);

            //Draw the board
            squareWidth = (Width) / 8;
            for (int c = 0; c < Width; c += squareWidth)
            {
                int offset = 0;
                if ((c / squareWidth) % 2 != 0)
                {
                    offset += squareWidth;
                }
                for (int i = offset; i < Width; i += (squareWidth * 2))
                {
                    e.Graphics.FillRectangle(Brushes.DarkGray, c, i, squareWidth, squareWidth);
                }
            }

            //Draw possible moves
            foreach (Move move in possibleMoves)
            {
                e.Graphics.FillRectangle(Brushes.PaleTurquoise, move.Destination.X * squareWidth, move.Destination.Y * squareWidth, squareWidth, squareWidth);
            }

            //Draw selected checker
            if (selectedChecker.X >= 0 && selectedChecker.Y >= 0)
            {
                e.Graphics.FillRectangle(Brushes.PeachPuff, selectedChecker.X * squareWidth, selectedChecker.Y * squareWidth, squareWidth, squareWidth);
            }

            //Draw Border
            e.Graphics.DrawRectangle(Pens.DarkGray,
            e.ClipRectangle.Left,
            e.ClipRectangle.Top,
            e.ClipRectangle.Width - 1,
            e.ClipRectangle.Height - 1);

            //Set for higher quality resizing of images
            //e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            e.Graphics.InterpolationMode = InterpolationMode.NearestNeighbor;

            //Draw Checker Images
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (Board[i,j].Colour == CheckerColour.Red)
                    {
                        if (Board[i, j].King)
                        {
                            e.Graphics.DrawImage(checkerRedKing, new Rectangle(j * squareWidth, i * squareWidth, squareWidth, squareWidth));
                        }
                        else
                        {
                            e.Graphics.DrawImage(checkerRed, new Rectangle(j * squareWidth, i * squareWidth, squareWidth, squareWidth));
                        }
                    }
                    else if (Board[i, j].Colour == CheckerColour.Black)
                    {
                        if (Board[i, j].King)
                        {
                            e.Graphics.DrawImage(checkerBlackKing, new Rectangle(j * squareWidth, i * squareWidth, squareWidth, squareWidth));
                        }
                        else
                        {
                            e.Graphics.DrawImage(checkerBlack, new Rectangle(j * squareWidth, i * squareWidth, squareWidth, squareWidth));
                        }
                    }
                }
            }

            //if (animating)
            //{
            //    Console.WriteLine("Animating" + currentPoint.ToString());
            //    //currentPoint.X += -1 * (newPoint.X - oldPoint.X) / 10;
            //    //currentPoint.Y += -1 * (newPoint.Y - oldPoint.Y) / 10;

            //    e.Graphics.DrawImage(checkerBlack, new Rectangle(currentPoint.Y, currentPoint.Y, squareWidth, squareWidth));

            //    if (currentPoint.X == newPoint.X && currentPoint.Y == newPoint.Y)
            //        animating = false;
            //}

        }
        
        protected override void OnMouseClick(MouseEventArgs e)
        {
            int clickedX = (int)(((double)e.X / (double)Width) * 8.0d);
            int clickedY = (int)(((double)e.Y / (double)Height) * 8.0d);

            Point clickedPoint = new Point(clickedX, clickedY);

            //Determine if this is the correct player
            if (Board[clickedY, clickedX].Colour != CheckerColour.Empty
                && Board[clickedY, clickedX].Colour != currentTurn)
                return;
          
            //Determine if this is a move or checker selection
            List<Move> matches = possibleMoves.Where(m => m.Destination == clickedPoint).ToList<Move>();
            if (matches.Count > 0)
            {
                //Move the checker to the clicked square
                MoveChecker(matches[0]);
            }
            else if (Board[clickedY, clickedX].Colour != CheckerColour.Empty)
            {
                //Select the clicked checker
                selectedChecker.X = clickedX;
                selectedChecker.Y = clickedY;
                possibleMoves.Clear();

                Console.WriteLine("Selected Checker: {0}",selectedChecker.ToString());
                
                Move[] OpenSquares = Utils.GetOpenSquares(Board, selectedChecker);
                possibleMoves.AddRange(OpenSquares);

                this.Invalidate();
            }            
        }

        private void MoveChecker(Move move)
        {
            Console.WriteLine(move.ToString());

            Board[move.Destination.Y, move.Destination.X].Colour = Board[move.Source.Y, move.Source.X].Colour;
            Board[move.Destination.Y, move.Destination.X].King = Board[move.Source.Y, move.Source.X].King;
            ResetSquare(move.Source);

            foreach (Point point in move.Captures)
                ResetSquare(point);

            selectedChecker.X = -1;
            selectedChecker.Y = -1;

            //Kinging
            if ((move.Destination.Y == 7 && Board[move.Destination.Y, move.Destination.X].Colour == CheckerColour.Red)
                || (move.Destination.Y == 0 && Board[move.Destination.Y, move.Destination.X].Colour == CheckerColour.Black))
            {
                Board[move.Destination.Y, move.Destination.X].King = true;
            }


            possibleMoves.Clear();

            oldPoint.X = move.Source.Y * squareWidth;
            oldPoint.Y = move.Source.X * squareWidth;
            newPoint.X = move.Destination.Y * squareWidth;
            newPoint.Y = move.Destination.X * squareWidth;
            currentPoint = oldPoint;
            animating = true;

            this.Invalidate();

            AdvanceTurn();
        }

        private void ResetSquare(Point square)
        {
            //Reset the square and the selected checker
            Board[square.Y, square.X].Colour = CheckerColour.Empty;
            Board[square.Y, square.X].King = false;
        }

        private void AdvanceTurn()
        {
            if (currentTurn == CheckerColour.Red)
            {
                currentTurn = CheckerColour.Black;
            }
            else
            {
                currentTurn = CheckerColour.Red;
            }

            if (AI != null && AI.Colour == currentTurn)
            {
                Move aiMove = AI.Process(Board);
                MoveChecker(aiMove);
            }
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.ResumeLayout(false);
        }
    }
}
