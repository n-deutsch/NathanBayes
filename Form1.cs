using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NathanBayes
{
    public partial class Form1 : Form
    {
        private NaiveBayes NB;

        public Form1()
        {
            InitializeComponent();
            NB = new NaiveBayes();
            OutputBox.AppendText("Hello!\r\n");
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void TrainButton_Click(object sender, EventArgs e)
        {
            //build frequency table
            String output = String.Empty;

            OutputBox.AppendText("Processing training data...\r\n");
            NB.buildFrequencyTable();
            OutputBox.AppendText("Done!\r\n");

            return;
        }

        private void TestButton_Click(object sender, EventArgs e)
        {
            //test 15 TRUSTED articles and 15 UNTRUSTED articles
            String output = String.Empty;

            OutputBox.AppendText("Testing...\r\n");
            output = output + NB.test();
            OutputBox.AppendText(output);
            OutputBox.AppendText("\r\n");
            OutputBox.AppendText("Done!\r\n");

            return;
        }

        private void ClassifyButton_Click(object sender, EventArgs e)
        {
            //classify some new URL

            //grab input from UI
            String query = InputBox.Text;

            OutputBox.AppendText("classifying " + query + "...\r\n");
            //classify call
            int result = NB.classify(query);

            //append output to UI
            if (result == 1)
            {
                OutputBox.AppendText("[TRUSTED]");
            }
            else if (result == 0)
            {
                OutputBox.AppendText("[UNTRUSTED]");
            }
            else
            {
                OutputBox.AppendText("[ERROR]");
            }

            OutputBox.AppendText("\r\n"); //newline character

            return;
        }
    }
}
