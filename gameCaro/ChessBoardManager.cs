using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace gameCaro
{
    class ChessBoardManager
    {
        #region Properties
        private ProgressBar prcbCountDown;
       private Panel chessBoard;
        public Panel ChessBoard { get => chessBoard; set => chessBoard = value; }
        public List<Player> Player1 { get => Player; set => Player = value; }
        public int CurrentPlayer { get; private set; }
        public TextBox PlayerName { get => playerName; set => playerName = value; }
        public PictureBox PlayerMark { get => playerMark; set => playerMark = value; }
        public List<List<Button>> Matrix { get => matrix; set => matrix = value; }
        public ProgressBar PrcbCountDown { get => prcbCountDown; set => prcbCountDown = value; }
        public Stack<Point> PlayTimeLine { get => playTimeLine; set => playTimeLine = value; }

        private List<Player> Player;
        private TextBox playerName;
        private PictureBox playerMark;
        private List<List<Button>> matrix;
        private Stack<Point> playTimeLine;
        #endregion
        #region Initialize
        public ChessBoardManager(Panel chessBoard, TextBox playerName, PictureBox playerMark,ProgressBar prcbCountDown)
        {
            this.prcbCountDown = prcbCountDown;
            this.ChessBoard = chessBoard;
            this.PlayerName = playerName;
            this.PlayerMark = playerMark;
            this.Player = new List<Player>()
            {
                new Player("SomeOne", Image.FromFile(Application.StartupPath + "\\Resources\\con1.png")),
                new Player("SomeThing", Image.FromFile(Application.StartupPath + "\\Resources\\icon2.png"))
            };
            CurrentPlayer = 0;
            ChangePlayer();
            PlayTimeLine = new Stack<Point>();
        }

        public ChessBoardManager()
        {
        }


        #endregion
        #region Methods
        public void DrawChessBoard()
        {
            ChessBoard.Controls.Clear();
            Matrix = new List<List<Button>>(); 
            Button oldButton = new Button()
            {
                Width = 0,
                Height = 0,
                Location = new Point(0, 0)
            };
            for (int i = 0; i < Cons.Chess_Board_Width; i++)
            {
                Matrix.Add(new List<Button>());
                for (int j = 0; j < Cons.Chess_Board_Height; j++)
                {
                    Button btn = new Button()
                    {
                        Width = Cons.Chess_Width,
                        Height = Cons.Chess_Height,
                        Location = new Point(oldButton.Location.X + oldButton.Width, oldButton.Location.Y),
                        BackgroundImageLayout = ImageLayout.Stretch,
                        Tag = i.ToString()
                    };
                    btn.Click += btn_Click;

                    ChessBoard.Controls.Add(btn);

                    Matrix[i].Add(btn);
                    
                    oldButton = btn;
                }
                oldButton.Location = new Point(0, oldButton.Location.Y + Cons.Chess_Height);
                oldButton.Width = 0;
                oldButton.Height = 0;
            }
        }

        private void btn_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            if (btn.BackgroundImage != null)
                return;
            
            Mark(btn);

            ChangePlayer();

            playTimeLine.Push(GetChessPoint(btn));


            CountProgressBar();

            if (isEndGame(btn))
            {
                EndGame();
            }
        }

        private void CountProgressBar()
        {
            prcbCountDown.PerformStep();
            prcbCountDown.Value = 0;
            prcbCountDown.Enabled = !prcbCountDown.Enabled;
           
        }
        private void EndGame()
        {
            MessageBox.Show("END GAME");
            Application.Exit();
        } 
        private bool isEndGame(Button btn)  //Xem lại cách chiến thắng của cách chơi cờ có bug ở nửa trên đường chéo phu
        {
            return isEndGameHorizontal(btn) || isEndGameVertical(btn) || isEndGameSub(btn) || isEndGamePrimary(btn) ;
        }    

        private Point GetChessPoint(Button btn)
        {
            int vertical = Convert.ToInt32(btn.Tag);
            int horizontal = Matrix[vertical].IndexOf(btn);
            Point point = new Point(horizontal, vertical);
            return point;
        }

        public bool Undo()
        {
            Point oldButton = playTimeLine.Pop();
            Button btn = Matrix[oldButton.Y][oldButton.X];
            btn.BackgroundImage = null;

            CurrentPlayer = CurrentPlayer == 1 ? 0 : 1;

            ChangePlayer();
            return false;
        }

        private bool isEndGameHorizontal(Button btn)
        {
            Point point = GetChessPoint(btn);

            int countLeft = 0;

            for (int i=point.X; i>0 ;i--)
            {
                if (Matrix[point.Y][i].BackgroundImage == btn.BackgroundImage)
                {
                    countLeft++;
                }
                else
                    break;
            }

            int countRight = 0;

            for (int i=point.X+1;i<Cons.Chess_Board_Width;i++)
            {
                if (Matrix[point.Y][i].BackgroundImage == btn.BackgroundImage)
                {
                    countRight++;
                }
                else
                    break;
            }
            return countLeft + countRight == 5;
        }
        private bool isEndGameVertical(Button btn)
        {
            Point point = GetChessPoint(btn);
            int countTop = 0;
            for (int i = point.Y; i >= 0; i--)
            {
                if (Matrix[i][point.X].BackgroundImage == btn.BackgroundImage)
                {
                    countTop++;
                }
                else
                    break;
            }

            int countBottom = 0;

            for (int i = point.Y + 1; i < Cons.Chess_Board_Height; i++)
            {
                if (Matrix[i][point.X].BackgroundImage == btn.BackgroundImage)
                {
                    countBottom++;
                }
                else
                    break;
            }
            return countTop + countBottom == 5;
        }
        private bool isEndGamePrimary(Button btn)
        {
            Point point = GetChessPoint(btn);

            int countTop = 0;
            for (int i = 0; i <= point.X; i++)
            {
                if (point.X - i < 0 || point.Y - i < 0)
                    break;

                if (Matrix[point.Y - i][point.X - i].BackgroundImage == btn.BackgroundImage)
                {
                    countTop++;
                }
                else
                    break;
            }

            int countBottom = 0;
            for (int i = 1; i <= Cons.Chess_Board_Width - point.X; i++)
            {
                if (point.Y + i >= Cons.Chess_Board_Height || point.X + i >= Cons.Chess_Board_Width)
                    break;

                if (Matrix[point.Y + i][point.X + i].BackgroundImage == btn.BackgroundImage)
                {
                    countBottom++;
                }
                else
                    break;
                
            }
            return countTop + countBottom == 5;
        }
        private bool isEndGameSub(Button btn)
        {
            Point point = GetChessPoint(btn);

            int countTop = 0;
            for (int i = 0; i <= point.X; i++)
            {
                if (point.X + i > Cons.Chess_Board_Width || point.Y - i < 0)
                    break;

                if (Matrix[point.Y - i][point.X + i].BackgroundImage == btn.BackgroundImage)
                {
                    countTop++;
                }
                else
                    break;
            }

            int countBottom = 0;
            for (int i = 1; i <= Cons.Chess_Board_Width - point.X; i++)
            {
                if (point.Y + i >= Cons.Chess_Board_Height || point.X - i < 0)
                    break;

                if (Matrix[point.Y + i][point.X - i].BackgroundImage == btn.BackgroundImage)
                {
                    countBottom++;
                }
                else
                    break;
            }

            return countTop + countBottom == 5;
        }
        private void Mark(Button btn)
        {
            btn.BackgroundImage = Player[CurrentPlayer].Mark;
            CurrentPlayer = CurrentPlayer == 1 ? 0 : 1;
        }
        private void ChangePlayer()
        {
            playerName.Text = Player[CurrentPlayer].Name;
            playerMark.Image = Player[CurrentPlayer].Mark;
        }   
        
        #endregion
    }

}
