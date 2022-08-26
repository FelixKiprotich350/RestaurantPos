namespace RestaurantManager.UserInterface.PointofSale
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class CardPayment : Form
    {
        public decimal Amount = 0M;
        public string Refference = "";
        private IContainer components = null;
        private Label label2;
        private Button Btn_Close;
        private TextBox textBox1;
        private Label label1;
        private Button Btn_Ok;
        private ComboBox Combo_BankName;

        public CardPayment()
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
                if ((this.textBox1.Text == "") || (this.Combo_BankName.Text == ""))
                {
                    MessageBox.Show("Incomplete Details", "MessageBox", MessageBoxButtons.OK);
                }
                else
                {
                    this.Amount = Convert.ToDecimal(this.textBox1.Text);
                    this.Refference = this.Combo_BankName.Text;
                    base.Close();
                }
            }
            catch
            {
                this.Amount = 0M;
                base.Close();
            }
        }

        private void CardPayment_Load(object sender, EventArgs e)
        {
            this.Combo_BankName.SelectedItem = null;
            base.ActiveControl = this.textBox1;
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
            this.label2 = new Label();
            this.Btn_Close = new Button();
            this.textBox1 = new TextBox();
            this.label1 = new Label();
            this.Btn_Ok = new Button();
            this.Combo_BankName = new ComboBox();
            base.SuspendLayout();
            this.label2.AutoSize = true;
            this.label2.Font = new Font("Microsoft Sans Serif", 14f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.label2.Location = new Point(0x47, 0x56);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x61, 0x18);
            this.label2.TabIndex = 15;
            this.label2.Text = "Card Bank";
            this.Btn_Close.DialogResult = DialogResult.Cancel;
            this.Btn_Close.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.Btn_Close.Location = new Point(0x25, 160);
            this.Btn_Close.Name = "Btn_Close";
            this.Btn_Close.Size = new Size(0x4b, 0x1d);
            this.Btn_Close.TabIndex = 3;
            this.Btn_Close.Text = "Close";
            this.Btn_Close.UseVisualStyleBackColor = true;
            this.Btn_Close.Click += new EventHandler(this.Btn_Close_Click);
            this.textBox1.Font = new Font("Microsoft Sans Serif", 16f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.textBox1.Location = new Point(30, 0x2a);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new Size(0xc4, 0x20);
            this.textBox1.TabIndex = 0;
            this.label1.AutoSize = true;
            this.label1.Font = new Font("Microsoft Sans Serif", 14f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.label1.Location = new Point(60, 15);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x79, 0x18);
            this.label1.TabIndex = 14;
            this.label1.Text = "Card Amount";
            this.Btn_Ok.DialogResult = DialogResult.Cancel;
            this.Btn_Ok.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.Btn_Ok.Location = new Point(0x89, 160);
            this.Btn_Ok.Name = "Btn_Ok";
            this.Btn_Ok.Size = new Size(0x4b, 0x1d);
            this.Btn_Ok.TabIndex = 2;
            this.Btn_Ok.Text = "Ok";
            this.Btn_Ok.UseVisualStyleBackColor = true;
            this.Btn_Ok.Click += new EventHandler(this.Btn_Ok_Click);
            this.Combo_BankName.DropDownStyle = ComboBoxStyle.DropDownList;
            this.Combo_BankName.Font = new Font("Microsoft Sans Serif", 16f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.Combo_BankName.FormattingEnabled = true;
            object[] items = new object[9];
            items[0] = "KCB";
            items[1] = "EQUITY";
            items[2] = "ABSA";
            items[3] = "COOPERATIVE";
            items[4] = "TRANSNATIONAL";
            items[5] = "DTB";
            items[6] = "NATIONAL";
            items[7] = "VISA";
            items[8] = "MASTERCARD";
            this.Combo_BankName.Items.AddRange(items);
            this.Combo_BankName.Location = new Point(30, 0x71);
            this.Combo_BankName.Name = "Combo_BankName";
            this.Combo_BankName.Size = new Size(0xc4, 0x21);
            this.Combo_BankName.TabIndex = 1;
            base.AcceptButton = this.Btn_Ok;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.CancelButton = this.Btn_Close;
            base.ClientSize = new Size(0x101, 0xcd);
            base.Controls.Add(this.Combo_BankName);
            base.Controls.Add(this.label2);
            base.Controls.Add(this.Btn_Close);
            base.Controls.Add(this.textBox1);
            base.Controls.Add(this.label1);
            base.Controls.Add(this.Btn_Ok);
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "CardPayment";
            base.ShowIcon = false;
            base.StartPosition = FormStartPosition.CenterParent;
            this.Text = "CardPayment";
            base.TopMost = true;
            base.Load += new EventHandler(this.CardPayment_Load);
            base.ResumeLayout(false);
            base.PerformLayout();
        }
    }
}

