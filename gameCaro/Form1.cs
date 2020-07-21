using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace gameCaro
{
    public partial class Form1 : Form
    { 
        #region Properties   
            ChessBoardManager ChessBoard;
        #endregion
        public Form1()
        {
            InitializeComponent();

            ChessBoard = new ChessBoardManager(pnlChessBoard,txbPlayerName,ptcbMark,prcbCountDown);

            prcbCountDown.Step = Cons.Cool_Down_Step;
            prcbCountDown.Maximum = Cons.Cool_Down_Time;
            prcbCountDown.Value = 0;

            tmCountDown.Interval = Cons.Cool_Down_Interval;
            ChessBoard.DrawChessBoard();
            
        }
        private void tmCountDown_Tick(object sender, EventArgs e)
        {

            prcbCountDown.PerformStep();

            if (prcbCountDown.Value == prcbCountDown.Maximum)
            {
                tmCountDown.Stop();
                MessageBox.Show("End Game");
            }
        }


        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click_1(object sender, EventArgs e)
        {

        }

        private void pnlChessBoard_Paint(object sender, PaintEventArgs e)
        {

        }

        private void ptcbMark_Click(object sender, EventArgs e)
        {
            
        }

        private void prcbCountDown_Click(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            tmCountDown.Start();
        }
        private void NewGame()
        {
            tmCountDown.Stop();
            prcbCountDown.Value = 0;
            ChessBoardManager a = new ChessBoardManager(pnlChessBoard, txbPlayerName, ptcbMark, prcbCountDown);
            a.DrawChessBoard();
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewGame();
        }

        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Quit();
        }

        private void Quit()
        {
            Application.Exit();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("Do you wanna exit ? ", "Notify", MessageBoxButtons.OKCancel) != System.Windows.Forms.DialogResult.OK)
            e.Cancel = true;
        }

        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChessBoard.Undo();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            tmCountDown.Start();
        }
    }
}
