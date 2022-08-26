namespace RestaurantManager.UserInterface.PointofSale
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class MpesaPayment : Form
    {
        public decimal Amount = 0M;
        public string Refference = "";
        private IContainer components = null;
        private Button Btn_Close;
        private TextBox textBox1;
        private Label label1;
        private Button Btn_Ok;

        public MpesaPayment()
        {
            this.InitializeComponent();
            this.Amount = 0M;
            this.Refference = "";
        }

        private void Btn_Close_Click(object sender, EventArgs e)
        {
            this.Amount = 0M;
            this.Refference = "";
            base.Close();
        }

        private void Btn_Ok_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.textBox1.Text == "")
                {
                    MessageBox.Show("Incomplete Details", "MessageBox", MessageBoxButtons.OK);
                }
                else
                {
                    this.Amount = Convert.ToDecimal(this.textBox1.Text);
                    this.Refference = "#null";
                    base.Close();
                }
            }
            catch
            {
                this.Amount = 0M;
                base.Close();
            }
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
            this.Btn_Close = new Button();
            this.textBox1 = new TextBox();
            this.label1 = new Label();
            this.Btn_Ok = new Button();
            base.SuspendLayout();
            this.Btn_Close.DialogResult = DialogResult.Cancel;
            this.Btn_Close.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.Btn_Close.Location = new Point(0x1c, 0x5d);
            this.Btn_Close.Name = "Btn_Close";
            this.Btn_Close.Size = new Size(0x4b, 0x1d);
            this.Btn_Close.TabIndex = 3;
            this.Btn_Close.Text = "Close";
            this.Btn_Close.UseVisualStyleBackColor = true;
            this.Btn_Close.Click += new EventHandler(this.Btn_Close_Click);
            this.textBox1.Font = new Font("Microsoft Sans Serif", 16f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.textBox1.Location = new Point(0x10, 0x2d);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new Size(0xc4, 0x20);
            this.textBox1.TabIndex = 0;
            this.textBox1.KeyDown += new KeyEventHandler(this.TextBox1_KeyDown);
            this.textBox1.KeyPress += new KeyPressEventHandler(this.InputAmountPaid);
            this.label1.AutoSize = true;
            this.label1.Font = new Font("Microsoft Sans Serif", 14f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.label1.Location = new Point(0x2e, 0x12);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x8a, 0x18);
            this.label1.TabIndex = 5;
            this.label1.Text = "Mpesa Amount";
            this.Btn_Ok.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.Btn_Ok.Location = new Point(0x80, 0x5d);
            this.Btn_Ok.Name = "Btn_Ok";
            this.Btn_Ok.Size = new Size(0x54, 0x1d);
            this.Btn_Ok.TabIndex = 2;
            this.Btn_Ok.Text = "Accept";
            this.Btn_Ok.UseVisualStyleBackColor = true;
            this.Btn_Ok.Click += new EventHandler(this.Btn_Ok_Click);
            base.AcceptButton = this.Btn_Ok;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.CancelButton = this.Btn_Close;
            base.ClientSize = new Size(0xe7, 0x86);
            base.Controls.Add(this.Btn_Close);
            base.Controls.Add(this.textBox1);
            base.Controls.Add(this.label1);
            base.Controls.Add(this.Btn_Ok);
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "MpesaPayment";
            base.ShowIcon = false;
            base.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Mpesa Payment";
            base.TopMost = true;
            base.Load += new EventHandler(this.MpesaPayment_Load);
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void InputAmountPaid(object sender, KeyPressEventArgs e)
        {
            e.Handled = !(((e.KeyChar.ToString() == ".") || char.IsControl(e.KeyChar)) || char.IsNumber(e.KeyChar));
        }

        private void MpesaPayment_Load(object sender, EventArgs e)
        {
            base.ActiveControl = this.textBox1;
            this.textBox1.Focus();
        }

        private void TextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode == Keys.Up) || (e.KeyCode == Keys.Down))
            {
            }
        }

        private void TextBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode == Keys.Up) || (e.KeyCode == Keys.Down))
            {
                this.textBox1.Focus();
            }
        }
    }
}

