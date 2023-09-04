namespace RestaurantManager.UserInterface.TicketPayments
{
    using DatabaseModels.Vouchers;
    using RestaurantManager.ApplicationFiles; 
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Linq;
    using System.Windows.Forms;

    public class VoucherPayment : Form
    {
        public VoucherCard SelectedVoucher = null; 
        private readonly IContainer components = null;
        private Button Btn_Close;
        public TextBox textBox1;
        private Label label1;
        private Button Btn_Ok;

        public VoucherPayment()
        {
            this.InitializeComponent();
            SelectedVoucher = null;
        }

        private void Btn_Close_Click(object sender, EventArgs e)
        {
            SelectedVoucher = null;
            base.Close();
        }

        private void Btn_Ok_Click(object sender, EventArgs e)
        {
            try
            {
               
                using(var db=new PosDbContext())
                {
                    var voucher=db.VoucherCard.Where(x => x.VoucherNumber == textBox1.Text).FirstOrDefault();
                    if (voucher == null)
                    {
                        MessageBox.Show("The Voucher Number does not Exist!","Message Box",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                        return;
                    }
                    if (voucher.VoucherStatus == GlobalVariables.PosEnums.VoucherStatuses.Redeemed.ToString())
                    {
                        MessageBox.Show("The Voucher Number has been Redeemed!","Message Box",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                        return;
                    }
                    if (voucher.VoucherStatus == GlobalVariables.PosEnums.VoucherStatuses.Expired.ToString())
                    {
                        MessageBox.Show("The Voucher Number has Expired!","Message Box",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                        return;
                    }
                    if (voucher.VoucherStatus == GlobalVariables.PosEnums.VoucherStatuses.Available.ToString())
                    {
                        SelectedVoucher = voucher;
                    }
                    else
                    {
                        MessageBox.Show("The Voucher Status is Uknown!", "Message Box", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            catch
            { 
                Close();
            }
        }

        private void CardPayment_Load(object sender, EventArgs e)
        { 
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
            this.Btn_Close = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.Btn_Ok = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // Btn_Close
            // 
            this.Btn_Close.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Btn_Close.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Btn_Close.Location = new System.Drawing.Point(42, 111);
            this.Btn_Close.Name = "Btn_Close";
            this.Btn_Close.Size = new System.Drawing.Size(75, 29);
            this.Btn_Close.TabIndex = 3;
            this.Btn_Close.Text = "Close";
            this.Btn_Close.UseVisualStyleBackColor = true;
            this.Btn_Close.Click += new System.EventHandler(this.Btn_Close_Click);
            // 
            // textBox1
            // 
            this.textBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox1.Location = new System.Drawing.Point(31, 45);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(196, 32);
            this.textBox1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(48, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(157, 24);
            this.label1.TabIndex = 14;
            this.label1.Text = "Voucher Number";
            // 
            // Btn_Ok
            // 
            this.Btn_Ok.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Btn_Ok.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Btn_Ok.Location = new System.Drawing.Point(142, 111);
            this.Btn_Ok.Name = "Btn_Ok";
            this.Btn_Ok.Size = new System.Drawing.Size(75, 29);
            this.Btn_Ok.TabIndex = 2;
            this.Btn_Ok.Text = "Ok";
            this.Btn_Ok.UseVisualStyleBackColor = true;
            this.Btn_Ok.Click += new System.EventHandler(this.Btn_Ok_Click);
            // 
            // VoucherPayment
            // 
            this.AcceptButton = this.Btn_Ok;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.Btn_Close;
            this.ClientSize = new System.Drawing.Size(257, 178);
            this.Controls.Add(this.Btn_Close);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Btn_Ok);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "VoucherPayment";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Voucher Card Payment";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.CardPayment_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}

