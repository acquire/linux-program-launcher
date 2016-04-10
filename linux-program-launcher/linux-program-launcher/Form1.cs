using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

namespace linux_program_launcher
{
    public partial class Form1 : Form
    {

        List<string> items = new List<string>();

        public Form1()
        {
            InitializeComponent();
            //start form in middle of screen
            StartPosition = FormStartPosition.CenterScreen;

            var files = Directory.GetFiles(@"/usr/bin");
            foreach (var f in files)
            {
                items.Add(Path.GetFileName(f));
            }
            listBox1.Items.Clear();
            listBox1.Items.AddRange(items.ToArray<string>());

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (String.IsNullOrEmpty(textBox1.Text))
                {
                    listBox1.Items.Clear();
                    listBox1.Items.AddRange(items.ToArray<string>());
                }
                else
                {
                    listBox1.Items.Clear();
                    listBox1.Items.AddRange(items.Where(i => i.Contains(textBox1.Text)).ToArray<string>());
                }
                if(listBox1.Items.Count > 0)
                {
                    listBox1.SelectedIndex = 0;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                //launch the program
                if (e.KeyCode == Keys.Enter)
                {
                    LaunchProgram();
                    e.SuppressKeyPress = true;
                    this.Close();
                }

                //move the selection up
                if (e.KeyCode == Keys.Down)
                {
                    if (listBox1.SelectedIndex != listBox1.Items.Count - 1)
                    {
                        listBox1.SelectedIndex += 1;
                    }
                }

                //move the selection down
                if (e.KeyCode == Keys.Up)
                {
                    if (listBox1.SelectedIndex != 0)
                    {
                        listBox1.SelectedIndex -= 1;
                    }
                }

                //press esc to exit
                if (e.KeyCode == Keys.Escape)
                {
                    this.Close();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void LaunchProgram()
        {
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            startInfo.FileName = "setsid";
            startInfo.Arguments = listBox1.SelectedItem.ToString();
            process.StartInfo = startInfo;
            process.Start();
        }

        private void listBox1_KeyDown(object sender, KeyEventArgs e)
        {
            //launch the program
            if (e.KeyCode == Keys.Enter)
            {
                LaunchProgram();
                e.SuppressKeyPress = true;
                this.Close();
            }

            //press esc to exit
            else if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }

            else
            {
                e.SuppressKeyPress = true;
                textBox1.Focus();
                textBox1.SelectionStart = textBox1.Text.Length;

                //handle transition when focusing from listbox to textbox
                if (e.KeyCode == Keys.Back)
                {
                    //when you press the backspace key on listbox
                    textBox1.Text = textBox1.Text.Remove(textBox1.Text.Length - 1);
                }
                else
                {
                    //when you try to type something on the listbox
                    textBox1.Text += e.KeyData.ToString().ToLower();
                }
                
                textBox1.SelectionStart = textBox1.Text.Length;


            }
        }
    }
}
