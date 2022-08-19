using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace tictactoe
{
    public partial class Form1 : Form
    {
        public enum Player
        {
            X,
            O
        }

        Player currentPlayer;            // call player class
        List<Button> buttons;            // create list of buttons
        readonly Random rand = new Random();
        public int playerWins = 0;       // keep track of wins
        public int computerWins = 0;
        public string mode = "minimax";  // define ai mode
        
        string[] board = new string[] { " ", " ", " ", " ", " ", " ", " ", " ", " " };
        

        public Form1()
        {
            InitializeComponent();
            ResetGame();
        }
        
        private void RestartGame(object sender, EventArgs e)
        {
            ResetGame();
        }

        private void PlayerClick(object sender, EventArgs e)
        {   
            var button = (Button)sender;                  // locate clicked button
            currentPlayer = Player.X;                     // player symbol X
            button.Text = currentPlayer.ToString();       // place player symbol on button
            button.Enabled = false;                       // diable button
            
            int index = buttons.IndexOf(button);
            board[index] = "X";

            // check for win states and allow Ai to move
            CheckWinner();
            LockButtons();
            Computer_timer.Start();
        }

        private void LoadButtons()
        {
            buttons = new List<Button> { button1, button2, button3, button4, button5, button6, button7, button8, button9 };
            board = new string[] { " ", " ", " ", " ", " ", " ", " ", " ", " " };
        }

        private void ResetGame()
        {
            foreach (Control X in this.Controls)
            {
                if (X is Button button && X.Tag == "play")
                {
                    button.Text = "?";
                    button.Enabled = true;
                }
            }
            LoadButtons(); // add all buttons to list
        }

        private void ComputerMove(object sender, EventArgs e)
        {
            if (buttons.Count > 0)
            {
                if (mode == "naive")
                {
                    // Naive strategy, random move
                    int index = rand.Next(buttons.Count);            // get random button number
                    buttons[index].Enabled = false;                  // select button at index and disable
                    currentPlayer = Player.O;                        // player symbol O
                    buttons[index].Text = currentPlayer.ToString();  // place player symbol on button
                    board[index] = "O";
                }
                if (mode == "minimax")
                { 
                    int bestMove = 0;
                    if (BoardIsEmpty())
                    {
                        bestMove = rand.Next(buttons.Count);
                    }                
                    else
                    {
                        float zero = 0;
                        float bestScore = -1 / zero;
                        for (int i = 0; i < board.Length; i++)
                        {
                            if (IsFree(i))
                            {
                                board[i] = "O";
                                float score = MiniMax(board, 0, false);
                                board[i] = " ";
                                if (score > bestScore)
                                {
                                    bestScore = score;
                                    bestMove = i;
                                }
                            }
                        }
                    }

                    buttons[bestMove].Enabled = false;                  // select button at index and disable
                    currentPlayer = Player.O;                           // player symbol O
                    buttons[bestMove].Text = currentPlayer.ToString();  // place player symbol on button
                    board[bestMove] = "O";

                }

                // check for win states and allow player to move
                CheckWinner();
                UnlockButtons();
                Computer_timer.Stop();
            }
        }

        public bool CheckBoard(string letter)
        {
            // check if player has won
            return (board[0] == letter && board[1] == letter && board[2] == letter) ||
                   (board[3] == letter && board[4] == letter && board[5] == letter) ||
                   (board[6] == letter && board[7] == letter && board[8] == letter) ||
                   (board[0] == letter && board[3] == letter && board[6] == letter) ||
                   (board[1] == letter && board[4] == letter && board[7] == letter) ||
                   (board[2] == letter && board[5] == letter && board[8] == letter) ||
                   (board[0] == letter && board[4] == letter && board[8] == letter) ||
                   (board[2] == letter && board[4] == letter && board[6] == letter);
        }

        public void CheckWinner()
        {
            if (CheckBoard("X"))
            {
                // finishing procedure
                Computer_timer.Stop();
                MessageBox.Show("Player Wins");
                playerWins++;
                label1.Text = "Player Wins- " + playerWins;
                ResetGame();
            }

            else if (CheckBoard("O"))
            {
                // finishing procedure
                Computer_timer.Stop();
                MessageBox.Show("Computer Wins");
                computerWins++;
                label2.Text = "Computer Wins- " + computerWins;
                ResetGame();
            }
            
            else if (BoardIsFull())
            {
                Computer_timer.Stop();
                MessageBox.Show("Draw");
                ResetGame();
            }
                
        }

        // Lock buttons to prevent player from clicking after making a move
        public void LockButtons()
        {
            foreach (Control X in this.Controls)
            {
                if (X is Button button && X.Tag == "play")
                {
                    button.Enabled = false;
                }
            }
        }

        // Unlock buttons to allow player to click after computer makes a move
        public void UnlockButtons()
        {
            foreach (Control X in this.Controls)
            {
                if (X is Button button && X.Tag == "play" && X.Text == "?")
                {
                    button.Enabled = true;
                }
            }
        }
        
        private float MiniMax(string[] board, int depth, bool maximize)
        {
            if (BoardIsFull())
                return 0;
            if (CheckBoard("O"))
                return 1;
            if (CheckBoard("X"))
                return -1;

            if (maximize)
            {
                float zero = 0;
                float bestScore = -1 / zero;
                
                for (int i = 0; i < board.Length; i++)
                {
                    if (IsFree(i))
                    {
                        board[i] = "O";
                        float score = MiniMax(board, depth + 1, false);
                        board[i] = " ";
                        bestScore = Math.Max(score, bestScore);
                    }
                }
                return bestScore;
            }
            else
            {
                float zero = 0;
                float bestScore = 1 / zero;

                for (int i = 0; i < board.Length; i++)
                {
                    if (IsFree(i))
                    {
                        board[i] = "X";
                        float score = MiniMax(board, depth + 1, true);
                        board[i] = " ";
                        bestScore = Math.Min(score, bestScore);
                    }
                }
                return bestScore;
            }

        }
        
        bool IsFree(int pos)
        {
            return board[pos] == " ";
        }

        bool BoardIsFull()
        {
            foreach (string space in board)
            {
                if (space == " ")
                {
                    return false;
                }
            }
            return true;
        }

        bool BoardIsEmpty()
        {
            foreach (string space in board)
            {
                if (space == "X" || space == "O")
                {
                    return false;
                }
            }
            return true;
        }

        private void ChangeMode(object sender, EventArgs e)
        {
            if (mode == "naive")                 // if mode is naive, change to minimax
            {
                mode = "minimax";
                label3.Text = "Current mode: Minimax";
            }
            else                                  // if mode is minimax, change to naive
            {
                mode = "naive";
                label3.Text = "Current mode: Naive";
            }
        }
    }
}