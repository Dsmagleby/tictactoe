using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
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

        Player currentPlayer;          // call player class
        List<Button> buttons;          // create list of buttons
        readonly Random rand = new Random();
        public int playerWins = 0;     // keep track of wins
        public int computerWins = 0;

        
        public Form1()
        {
            InitializeComponent();
            ResetGame();
        }

        private void DebugAI(object sender, EventArgs e)
        {

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
            buttons.Remove(button);                       // remove button from list of buttons

            // check for win states and allow Ai to move
            Check();
            LockButtons();
            Computer_timer.Start();
        }

        private void LoadButtons()
        {
            buttons = new List<Button> { button1, button2, button3, button4, button5, button6, button7, button8, button9 };
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

                // Native strategy, random move
                int index = rand.Next(buttons.Count);            // get random button number
                buttons[index].Enabled = false;                  // select button at index and disable
                currentPlayer = Player.O;                        // player symbol O
                buttons[index].Text = currentPlayer.ToString();  // place player symbol on button
                buttons.RemoveAt(index);                         // remove button from list

                // check for win states and allow player to move
                Check();
                UnlockButtons();
                Computer_timer.Stop();
            }
        }

        private void Check()
        {
            // check if player has won
            if (button1.Text == "X" && button2.Text == "X" && button3.Text == "X"
               || button4.Text == "X" && button5.Text == "X" && button6.Text == "X"
               || button7.Text == "X" && button9.Text == "X" && button8.Text == "X"
               || button1.Text == "X" && button4.Text == "X" && button7.Text == "X"
               || button2.Text == "X" && button5.Text == "X" && button8.Text == "X"
               || button3.Text == "X" && button6.Text == "X" && button9.Text == "X"
               || button1.Text == "X" && button5.Text == "X" && button9.Text == "X"
               || button3.Text == "X" && button5.Text == "X" && button7.Text == "X")
            {
                // finishing procedure
                Computer_timer.Stop();
                MessageBox.Show("Player Wins");
                playerWins++;
                label1.Text = "Player Wins- " + playerWins;
                ResetGame();
            }
            // check if the computer has won
            else if (button1.Text == "O" && button2.Text == "O" && button3.Text == "O"
            || button4.Text == "O" && button5.Text == "O" && button6.Text == "O"
            || button7.Text == "O" && button9.Text == "O" && button8.Text == "O"
            || button1.Text == "O" && button4.Text == "O" && button7.Text == "O"
            || button2.Text == "O" && button5.Text == "O" && button8.Text == "O"
            || button3.Text == "O" && button6.Text == "O" && button9.Text == "O"
            || button1.Text == "O" && button5.Text == "O" && button9.Text == "O"
            || button3.Text == "O" && button5.Text == "O" && button7.Text == "O")
            {

                // finishing procedure
                Computer_timer.Stop();
                MessageBox.Show("Computer Wins");
                computerWins++;
                label2.Text = "Computer Wins- " + computerWins;
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
                if (X is Button button && X.Tag == "play")
                {
                    button.Enabled = true;
                }
            }
        }

    }
}
