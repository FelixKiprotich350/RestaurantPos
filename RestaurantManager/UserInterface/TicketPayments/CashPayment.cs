using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace RestaurantManager.UserInterface.TicketPayments
{ 
    public class CashPayment : Form
    {
        public decimal Amount = 0M;
        private readonly IContainer components = null;
        private Button Btn_Ok;
        private Label label1;
        private TextBox textBox1;
        private Button Btn_Close;

        public CashPayment()
        {
            this.InitializeComponent();
            this.Amount = 0M;
        }

        private void Btn_Close_Click(object sender, EventArgs e)
        {
            this.Amount = 0M;
            base.Close();
        }

        private void Btn_Ok_Click(object sender, EventArgs e)
        {
            try
            {
                this.Amount = Convert.ToDecimal(this.textBox1.Text);
            }
            catch
            {
                this.Amount = 0M;
            }
            base.Close();
        }

        private void CashPayment_Load(object sender, EventArgs e)
        {
            this.textBox1.Focus();
        }

        private void CashPayment_Shown(object sender, EventArgs e)
        {
            this.textBox1.Focus();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.Btn_Ok = new Button();
            this.label1 = new Label();
            this.textBox1 = new TextBox();
            this.Btn_Close = new Button();
            base.SuspendLayout();
            this.Btn_Ok.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.Btn_Ok.Location = new Point(120, 0x65);
            this.Btn_Ok.Name = "Btn_Ok";
            this.Btn_Ok.Size = new Size(0x58, 0x1d);
            this.Btn_Ok.TabIndex = 1;
            this.Btn_Ok.Text = "Accept";
            this.Btn_Ok.UseVisualStyleBackColor = true;
            this.Btn_Ok.Click += new EventHandler(this.Btn_Ok_Click);
            this.label1.AutoSize = true;
            this.label1.Font = new Font("Microsoft Sans Serif", 14f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.label1.Location = new Point(0x27, 9);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x7c, 0x18);
            this.label1.TabIndex = 1;
            this.label1.Text = "Cash Amount";
            this.textBox1.Font = new Font("Microsoft Sans Serif", 16f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.textBox1.Location = new Point(12, 0x2e);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new Size(0xc4, 0x20);
            this.textBox1.TabIndex = 0;
            this.textBox1.KeyPress += new KeyPressEventHandler(this.InputAmountPaid);
            this.Btn_Close.DialogResult = DialogResult.Cancel;
            this.Btn_Close.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.Btn_Close.Location = new Point(20, 0x65);
            this.Btn_Close.Name = "Btn_Close";
            this.Btn_Close.Size = new Size(0x4b, 0x1d);
            this.Btn_Close.TabIndex = 2;
            this.Btn_Close.Text = "Close";
            this.Btn_Close.UseVisualStyleBackColor = true;
            this.Btn_Close.Click += new EventHandler(this.Btn_Close_Click);
            base.AcceptButton = this.Btn_Ok;
            base.AutoScaleDimensions = new SizeF(8f, 16f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.CancelButton = this.Btn_Close;
            base.ClientSize = new Size(220, 0x90);
            base.Controls.Add(this.Btn_Close);
            base.Controls.Add(this.textBox1);
            base.Controls.Add(this.label1);
            base.Controls.Add(this.Btn_Ok);
            this.Font = new Font("Microsoft Sans Serif", 10f, FontStyle.Regular, GraphicsUnit.Point, 0);
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.Margin = new Padding(4);
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "CashPayment";
            base.ShowIcon = false;
            base.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Cash Payment";
            base.TopMost = true;
            base.Load += new EventHandler(this.CashPayment_Load);
            base.Shown += new EventHandler(this.CashPayment_Shown);
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void InputAmountPaid(object sender, KeyPressEventArgs e)
        {
            e.Handled = !(((e.KeyChar.ToString() == ".") || char.IsControl(e.KeyChar)) || char.IsNumber(e.KeyChar));
        }
    }
}

