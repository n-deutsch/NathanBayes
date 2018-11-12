namespace NathanBayes
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.TrainButton = new System.Windows.Forms.Button();
            this.TestButton = new System.Windows.Forms.Button();
            this.ClassifyButton = new System.Windows.Forms.Button();
            this.InputBox = new System.Windows.Forms.TextBox();
            this.OutputLabel = new System.Windows.Forms.Label();
            this.OutputBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // TrainButton
            // 
            this.TrainButton.Location = new System.Drawing.Point(12, 12);
            this.TrainButton.Name = "TrainButton";
            this.TrainButton.Size = new System.Drawing.Size(75, 23);
            this.TrainButton.TabIndex = 0;
            this.TrainButton.Text = "Train";
            this.TrainButton.UseVisualStyleBackColor = true;
            this.TrainButton.Click += new System.EventHandler(this.TrainButton_Click);
            // 
            // TestButton
            // 
            this.TestButton.Location = new System.Drawing.Point(12, 41);
            this.TestButton.Name = "TestButton";
            this.TestButton.Size = new System.Drawing.Size(75, 23);
            this.TestButton.TabIndex = 1;
            this.TestButton.Text = "Test";
            this.TestButton.UseVisualStyleBackColor = true;
            this.TestButton.Click += new System.EventHandler(this.TestButton_Click);
            // 
            // ClassifyButton
            // 
            this.ClassifyButton.Location = new System.Drawing.Point(12, 70);
            this.ClassifyButton.Name = "ClassifyButton";
            this.ClassifyButton.Size = new System.Drawing.Size(75, 23);
            this.ClassifyButton.TabIndex = 2;
            this.ClassifyButton.Text = "Classify";
            this.ClassifyButton.UseVisualStyleBackColor = true;
            this.ClassifyButton.Click += new System.EventHandler(this.ClassifyButton_Click);
            // 
            // InputBox
            // 
            this.InputBox.Location = new System.Drawing.Point(93, 72);
            this.InputBox.Name = "InputBox";
            this.InputBox.Size = new System.Drawing.Size(379, 20);
            this.InputBox.TabIndex = 3;
            // 
            // OutputLabel
            // 
            this.OutputLabel.AutoSize = true;
            this.OutputLabel.Location = new System.Drawing.Point(12, 96);
            this.OutputLabel.Name = "OutputLabel";
            this.OutputLabel.Size = new System.Drawing.Size(39, 13);
            this.OutputLabel.TabIndex = 4;
            this.OutputLabel.Text = "Ouput:";
            // 
            // OutputBox
            // 
            this.OutputBox.Location = new System.Drawing.Point(15, 112);
            this.OutputBox.Multiline = true;
            this.OutputBox.Name = "OutputBox";
            this.OutputBox.ReadOnly = true;
            this.OutputBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.OutputBox.Size = new System.Drawing.Size(457, 137);
            this.OutputBox.TabIndex = 5;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(484, 261);
            this.Controls.Add(this.OutputBox);
            this.Controls.Add(this.OutputLabel);
            this.Controls.Add(this.InputBox);
            this.Controls.Add(this.ClassifyButton);
            this.Controls.Add(this.TestButton);
            this.Controls.Add(this.TrainButton);
            this.Name = "Form1";
            this.Text = "Super Sexy Naive Bayes";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button TrainButton;
        private System.Windows.Forms.Button TestButton;
        private System.Windows.Forms.Button ClassifyButton;
        private System.Windows.Forms.TextBox InputBox;
        private System.Windows.Forms.Label OutputLabel;
        private System.Windows.Forms.TextBox OutputBox;
    }
}

