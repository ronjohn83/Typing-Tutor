using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _14_TypingTutor
{
    public partial class Form1 : Form
    {
        private string filePath = "panagrams.txt";
        string[] pangrams;
        int[] accuracy;
        private int pangramCounter = 0;
        private int accuracyCounter = 0;
        private bool startTyping = false;

        public Form1()
        {
            InitializeComponent();
            pangrams = new string[3];
            this.Focus();
           
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            LoadPhases();
            startTyping = true;
            txtWrite.Focus();
            txtWrite.ReadOnly = false;
        }

        private void btnQuit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (startTyping)
            {
                lblInvalidKey.BackColor = Color.Empty;
                if (e.KeyCode == Keys.Delete || e.KeyCode == Keys.Back || e.KeyCode == Keys.Left || e.KeyCode == Keys.Right)
                {
                    e.SuppressKeyPress = true;
                    lblInvalidKey.BackColor = Color.Crimson;
                }
                //load new pangram.
                if (e.KeyCode == Keys.Enter)
                {
                    //Rate accuracy of pangram before load new pangram.
                    GetAccuracy(pangramCounter);
                    if (pangramCounter >= 3)
                    {
                        MessageBox.Show("No more pangrams, you've completed the excersise. View your results.");
                        DisplayStatistics();
                    }
                    else
                    {
                        txtPhrase.Text = pangrams[pangramCounter];
                        txtWrite.Clear();
                        pangramCounter++;
                    }
                }
                else
                {
                    HightlightKeyPres(e.KeyCode);
                }
            }
        }

        private void LoadPhases()
        {
            using (StreamReader sr = new StreamReader(filePath))
            {

                int counter = 0;
                while (!sr.EndOfStream)
                {
                    pangrams[counter] = sr.ReadLine();
                    counter++;
                }
            }

            txtPhrase.Text = pangrams[0];
            pangramCounter++;
        }

        private void HightlightKeyPres(Keys key)
        {
            foreach (Control c in Controls)
            {
                if (c is Label)
                {
                    Label label = (Label)c;
                    if (label.Tag != null  && label.Tag.ToString().ToUpper() == key.ToString().ToUpper() || Control.ModifierKeys == Keys.ShiftKey) 
                    {
                        label.BackColor = Color.Crimson;
                    }
                    else
                    {
                        label.BackColor = Color.Empty;
                    }
                }
            }
        }

        private void GetAccuracy(int pangramCounter)
        {
            //store accuracy.
            accuracy = new int[3];

            //Accuracy results based on length.
            var phase = txtPhrase.Text.Length;
            var write = txtWrite.Text.Length;
            var result = Math.Abs(phase - write);
            if (result != 0)
                accuracy[accuracyCounter] = 1;

            //Accuracy results character for character.
            var writeText = txtWrite.Text;
            char[] pangram = pangrams[pangramCounter-1].ToCharArray(); //pangram counter is minus 1 because because pangram counter was incremented earlier.
            char[] studentPangram = writeText.ToCharArray();
            int loopCounter = phase > write ? write : phase;
            for (int i = 0; i < loopCounter; i++)
            {
                if (pangram[i] != studentPangram[i])
                {
                    accuracy[accuracyCounter]++;
                }
            }
            //int accuracyResult = accuracy[accuracyCounter];
            //Console.WriteLine(accuracy[accuracyCounter]);
            
            accuracyCounter++;
        }

        private void DisplayStatistics()
        {
            lstStatistics.Items.Add("Your results:");
            for (int i = 0; i < accuracy.Length; i++)
            {
                lstStatistics.Items.Add("In the pangram # " + (i + 1) + " you made " + accuracy[i] + " mistakes");
            }
        }

        private void txtWrite_Click(object sender, EventArgs e)
        {
            txtWrite.SelectionStart = txtWrite.Text.Length;
        }
    }
}
