 using RestaurantManager.BusinessModels.Payments; 
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
namespace RestaurantManager.UserInterface.TicketPayments
{
    public class PaymentsUI : Form
    {
        //255, 255, 192
        private static decimal GrossAmount;
        public List<PaymentMethod> Payments;
        
        public PaymentsUI(decimal TotalAmount, List<PaymentMethod> pm)
        {
            this.InitializeComponent();
            GrossAmount = TotalAmount;
            this.Payments = pm;
        }

        #region
        private IContainer components = null;
        private Button Btn_CompleteTransaction;
        private Label label12;
        private Label label13;
        public TextBox Txt_AmountPaidTotal;
        public TextBox Txt_Balance;
        private DataGridView Payments_Gridview;
        private ContextMenuStrip ContextMenuStrip_paymentsGridview;
        private ToolStripMenuItem deleteToolStripMenuItem;
        private Button Btn_Close;
        private DataGridViewTextBoxColumn Column1;
        private DataGridViewTextBoxColumn Column2;
        private DataGridViewTextBoxColumn Column3;
        private DataGridViewTextBoxColumn Column4;
        public Button Btn_Cash;
        public Button Btn_Mpesa;
        public Button Btn_Card;
        public Button Button_Voucher;
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.Btn_CompleteTransaction = new System.Windows.Forms.Button();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.Txt_AmountPaidTotal = new System.Windows.Forms.TextBox();
            this.Txt_Balance = new System.Windows.Forms.TextBox();
            this.Payments_Gridview = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ContextMenuStrip_paymentsGridview = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.Btn_Cash = new System.Windows.Forms.Button();
            this.Btn_Close = new System.Windows.Forms.Button();
            this.Btn_Mpesa = new System.Windows.Forms.Button();
            this.Btn_Card = new System.Windows.Forms.Button();
            this.Button_Voucher = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.Payments_Gridview)).BeginInit();
            this.ContextMenuStrip_paymentsGridview.SuspendLayout();
            this.SuspendLayout();
            // 
            // Btn_CompleteTransaction
            // 
            this.Btn_CompleteTransaction.BackColor = System.Drawing.Color.ForestGreen;
            this.Btn_CompleteTransaction.FlatAppearance.BorderSize = 0;
            this.Btn_CompleteTransaction.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Btn_CompleteTransaction.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Btn_CompleteTransaction.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.Btn_CompleteTransaction.Location = new System.Drawing.Point(292, 350);
            this.Btn_CompleteTransaction.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Btn_CompleteTransaction.Name = "Btn_CompleteTransaction";
            this.Btn_CompleteTransaction.Size = new System.Drawing.Size(265, 50);
            this.Btn_CompleteTransaction.TabIndex = 2;
            this.Btn_CompleteTransaction.Text = "Complete";
            this.Btn_CompleteTransaction.UseVisualStyleBackColor = false;
            this.Btn_CompleteTransaction.Click += new System.EventHandler(this.Btn_CompleteTransaction_Click);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(12, 236);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(179, 31);
            this.label12.TabIndex = 24;
            this.label12.Text = "Amount Paid";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.Location = new System.Drawing.Point(12, 294);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(127, 31);
            this.label13.TabIndex = 25;
            this.label13.Text = "Balance ";
            // 
            // Txt_AmountPaidTotal
            // 
            this.Txt_AmountPaidTotal.Font = new System.Drawing.Font("Microsoft Sans Serif", 25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Txt_AmountPaidTotal.Location = new System.Drawing.Point(292, 236);
            this.Txt_AmountPaidTotal.Multiline = true;
            this.Txt_AmountPaidTotal.Name = "Txt_AmountPaidTotal";
            this.Txt_AmountPaidTotal.ReadOnly = true;
            this.Txt_AmountPaidTotal.Size = new System.Drawing.Size(268, 43);
            this.Txt_AmountPaidTotal.TabIndex = 26;
            this.Txt_AmountPaidTotal.Text = "0.00";
            this.Txt_AmountPaidTotal.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.Txt_AmountPaidTotal.WordWrap = false;
            this.Txt_AmountPaidTotal.TextChanged += new System.EventHandler(this.Txt_AmountPaidTotal_TextChanged);
            // 
            // Txt_Balance
            // 
            this.Txt_Balance.Font = new System.Drawing.Font("Microsoft Sans Serif", 25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Txt_Balance.Location = new System.Drawing.Point(292, 296);
            this.Txt_Balance.Name = "Txt_Balance";
            this.Txt_Balance.ReadOnly = true;
            this.Txt_Balance.Size = new System.Drawing.Size(268, 45);
            this.Txt_Balance.TabIndex = 27;
            this.Txt_Balance.Text = "0.00";
            this.Txt_Balance.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.Txt_Balance.WordWrap = false;
            // 
            // Payments_Gridview
            // 
            this.Payments_Gridview.AllowUserToAddRows = false;
            this.Payments_Gridview.AllowUserToDeleteRows = false;
            this.Payments_Gridview.AllowUserToResizeColumns = false;
            this.Payments_Gridview.AllowUserToResizeRows = false;
            this.Payments_Gridview.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.Payments_Gridview.BackgroundColor = System.Drawing.SystemColors.ButtonHighlight;
            this.Payments_Gridview.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Payments_Gridview.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2,
            this.Column3,
            this.Column4});
            this.Payments_Gridview.ContextMenuStrip = this.ContextMenuStrip_paymentsGridview;
            this.Payments_Gridview.Location = new System.Drawing.Point(7, 62);
            this.Payments_Gridview.Name = "Payments_Gridview";
            this.Payments_Gridview.ReadOnly = true;
            this.Payments_Gridview.RowHeadersVisible = false;
            this.Payments_Gridview.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.Payments_Gridview.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.Payments_Gridview.Size = new System.Drawing.Size(554, 167);
            this.Payments_Gridview.TabIndex = 4;
            this.Payments_Gridview.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.Payments_Gridview_RowsAdded);
            this.Payments_Gridview.RowsRemoved += new System.Windows.Forms.DataGridViewRowsRemovedEventHandler(this.Payments_Gridview_RowsRemoved);
            this.Payments_Gridview.UserAddedRow += new System.Windows.Forms.DataGridViewRowEventHandler(this.Payments_Gridview_UserAddedRow);
            this.Payments_Gridview.UserDeletedRow += new System.Windows.Forms.DataGridViewRowEventHandler(this.Payments_Gridview_UserDeletedRow);
            this.Payments_Gridview.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Payments_Gridview_KeyDown);
            // 
            // Column1
            // 
            this.Column1.FillWeight = 60F;
            this.Column1.HeaderText = "PayMode";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            // 
            // Column2
            // 
            this.Column2.HeaderText = "Amount";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            // 
            // Column3
            // 
            this.Column3.HeaderText = "Refference";
            this.Column3.Name = "Column3";
            this.Column3.ReadOnly = true;
            // 
            // Column4
            // 
            this.Column4.HeaderText = "SecRefference";
            this.Column4.Name = "Column4";
            this.Column4.ReadOnly = true;
            // 
            // ContextMenuStrip_paymentsGridview
            // 
            this.ContextMenuStrip_paymentsGridview.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.deleteToolStripMenuItem});
            this.ContextMenuStrip_paymentsGridview.Name = "ContextMenuStrip_paymentsGridview";
            this.ContextMenuStrip_paymentsGridview.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.ContextMenuStrip_paymentsGridview.Size = new System.Drawing.Size(108, 26);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.deleteToolStripMenuItem.Text = "Delete";
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.DeleteToolStripMenuItem_Click);
            // 
            // Btn_Cash
            // 
            this.Btn_Cash.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Btn_Cash.Location = new System.Drawing.Point(4, 8);
            this.Btn_Cash.Name = "Btn_Cash";
            this.Btn_Cash.Size = new System.Drawing.Size(130, 40);
            this.Btn_Cash.TabIndex = 0;
            this.Btn_Cash.Text = "Cash";
            this.Btn_Cash.UseVisualStyleBackColor = true;
            this.Btn_Cash.Click += new System.EventHandler(this.Btn_Cash_Click);
            // 
            // Btn_Close
            // 
            this.Btn_Close.BackColor = System.Drawing.Color.Red;
            this.Btn_Close.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Btn_Close.FlatAppearance.BorderSize = 0;
            this.Btn_Close.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Btn_Close.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Btn_Close.ForeColor = System.Drawing.Color.White;
            this.Btn_Close.Location = new System.Drawing.Point(12, 350);
            this.Btn_Close.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Btn_Close.Name = "Btn_Close";
            this.Btn_Close.Size = new System.Drawing.Size(172, 50);
            this.Btn_Close.TabIndex = 3;
            this.Btn_Close.Text = "Close";
            this.Btn_Close.UseVisualStyleBackColor = false;
            this.Btn_Close.Click += new System.EventHandler(this.Btn_Close_Click);
            // 
            // Btn_Mpesa
            // 
            this.Btn_Mpesa.BackColor = System.Drawing.SystemColors.Control;
            this.Btn_Mpesa.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.Btn_Mpesa.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Btn_Mpesa.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Btn_Mpesa.Location = new System.Drawing.Point(148, 8);
            this.Btn_Mpesa.Name = "Btn_Mpesa";
            this.Btn_Mpesa.Size = new System.Drawing.Size(130, 40);
            this.Btn_Mpesa.TabIndex = 1;
            this.Btn_Mpesa.Text = "Mpesa";
            this.Btn_Mpesa.UseVisualStyleBackColor = false;
            this.Btn_Mpesa.Click += new System.EventHandler(this.Btn_Mpesa_Click);
            // 
            // Btn_Card
            // 
            this.Btn_Card.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Btn_Card.Location = new System.Drawing.Point(292, 8);
            this.Btn_Card.Name = "Btn_Card";
            this.Btn_Card.Size = new System.Drawing.Size(130, 40);
            this.Btn_Card.TabIndex = 28;
            this.Btn_Card.Text = "Card";
            this.Btn_Card.UseVisualStyleBackColor = true;
            this.Btn_Card.Click += new System.EventHandler(this.Btn_Card_Click);
            // 
            // Button_Voucher
            // 
            this.Button_Voucher.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Button_Voucher.Location = new System.Drawing.Point(436, 8);
            this.Button_Voucher.Name = "Button_Voucher";
            this.Button_Voucher.Size = new System.Drawing.Size(130, 40);
            this.Button_Voucher.TabIndex = 32;
            this.Button_Voucher.Text = "Voucher";
            this.Button_Voucher.UseVisualStyleBackColor = true;
            this.Button_Voucher.Click += new System.EventHandler(this.Button_Voucher_Click);
            // 
            // PaymentsUI
            // 
            this.AcceptButton = this.Btn_CompleteTransaction;
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.CancelButton = this.Btn_Close;
            this.ClientSize = new System.Drawing.Size(570, 411);
            this.ControlBox = false;
            this.Controls.Add(this.Button_Voucher);
            this.Controls.Add(this.Btn_Card);
            this.Controls.Add(this.Btn_Mpesa);
            this.Controls.Add(this.Btn_Close);
            this.Controls.Add(this.Btn_Cash);
            this.Controls.Add(this.Payments_Gridview);
            this.Controls.Add(this.Txt_Balance);
            this.Controls.Add(this.Txt_AmountPaidTotal);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.Btn_CompleteTransaction);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PaymentsUI";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Complete Transaction";
            this.Enter += new System.EventHandler(this.Transactions_Enter);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Control_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.Payments_Gridview)).EndInit();
            this.ContextMenuStrip_paymentsGridview.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private void Btn_Card_Click(object sender, EventArgs e)
        {
            try
            {
                CardPayment payment = new CardPayment();
                payment.ShowDialog(this);
                decimal amount = payment.Amount;
                string refference = payment.Refference;
                if ((amount != 0M) && (amount > 0M))
                {
                    string str2 = "C-" + GlobalVariables.SharedVariables.CurrentDate().ToString("yyyyMMddHHmmssff");
                    object[] values = new object[] { "Card-" + refference, amount, str2, "" };
                    this.Payments_Gridview.Rows.Add(values);
                }
            }
            catch (Exception exception1)
            {
                MessageBox.Show(exception1.Message, "ERROR MESSAGE", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            base.ActiveControl = this.Btn_CompleteTransaction;
        }

        private void Btn_Cash_Click(object sender, EventArgs e)
        {
            try
            {
                CashPayment payment = new CashPayment();
                payment.ShowDialog(this);
                decimal amount = payment.Amount;
                if ((amount != 0M) && (amount > 0M))
                {
                    object[] values = new object[] { "Cash", amount, "", "" };
                    this.Payments_Gridview.Rows.Add(values);
                }
            }
            catch (Exception exception1)
            {
                MessageBox.Show(exception1.Message, "ERROR MESSAGE", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            base.ActiveControl = this.Btn_CompleteTransaction;
        }

        private void Btn_Mpesa_Click(object sender, EventArgs e)
        {
            try
            {
                MpesaPayment payment = new MpesaPayment();
                payment.ShowDialog(this);
                decimal amount = payment.Amount;
                string refference = payment.Refference;
                if ((amount != 0M) && (amount > 0M))
                {
                    object[] values = new object[] { "Mpesa", amount, refference, "" };
                    this.Payments_Gridview.Rows.Add(values);
                }
            }
            catch (Exception exception1)
            {
                MessageBox.Show(exception1.Message, "ERROR MESSAGE", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            base.ActiveControl = this.Btn_CompleteTransaction;
        }

        private void Button_Voucher_Click(object sender, EventArgs e)
        {
            try
            {
                VoucherPayment v = new VoucherPayment();
                v.ShowDialog(this);
                if (v.SelectedVoucher!=null)
                {
                     object[] values = new object[] { "Voucher", v.SelectedVoucher.VoucherAmount, v.SelectedVoucher.VoucherNumber, v.SelectedVoucher.VoucherBatchNo };
                    this.Payments_Gridview.Rows.Add(values);
                }
                
            }
            catch (Exception exception1)
            {
                MessageBox.Show(exception1.Message, "ERROR MESSAGE", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            base.ActiveControl = this.Btn_CompleteTransaction;
           
        }

        private void Btn_Close_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to Quit ?", "MessageBox", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                this.Payments.Clear();
                base.Close();
            }
        }

        private void Btn_CompleteTransaction_Click(object sender, EventArgs e)
        {
            try
            {
                if (Convert.ToDecimal(this.Txt_AmountPaidTotal.Text) < GrossAmount)
                {
                    base.DialogResult = DialogResult.No;
                    MessageBox.Show("Insufficient Amount Paid !!", "Transaction Response", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                int count = this.Payments_Gridview.Rows.Count;
                int num2 = 0;
                int num3 = 0;
                while (true)
                {
                    if (num3 >= count)
                    {
                        this.DialogResult = (num2 <= 0) ? DialogResult.OK : DialogResult.Cancel;
                        this.Close();
                        break;
                    }
                    try
                    {
                        string secondary = Payments_Gridview.Rows[num3].Cells[3].Value.ToString();
                        string primary = Payments_Gridview.Rows[num3].Cells[2].Value.ToString();
                        PaymentMethod p = new PaymentMethod
                        {
                            PaymentMethodName = Payments_Gridview.Rows[num3].Cells[0].Value.ToString(),
                            Amount = Convert.ToDecimal(Payments_Gridview.Rows[num3].Cells[1].Value.ToString()),
                            PrimaryRefference = primary != "" ? primary : "None",
                            SecondaryRefference = secondary != "" ? secondary : "None"
                        };
                        this.Payments.Add(p);
                    }
                    catch (Exception exception1)
                    {
                        num2 = 1;
                        MessageBox.Show(exception1.Message, "ERROR MESSAGE", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                        break;
                    }
                    
                    
                    num3++;
                }
            }
            catch (Exception exception3)
            {
                MessageBox.Show(exception3.Message, "Accounting Response");
            }
        }
 
        private void Control_KeyDown(object sender, KeyEventArgs e)
        {
            this.Functionkeys(e);
        }

        private void DeleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Payments_Gridview.Rows.Remove(this.Payments_Gridview.CurrentRow);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void FindTotal()
        {
            try
            {
                int count = this.Payments_Gridview.Rows.Count;
                if (count <= 0)
                {
                    double num4 = 0.0;
                    this.Txt_AmountPaidTotal.Text = num4.ToString("N2");
                    this.Txt_Balance.Text = num4.ToString("N2");
                }
                else
                {
                    double num2 = 0.0;
                    int num3 = 0;
                    while (true)
                    {
                        if (num3 >= count)
                        {
                            break;
                        }
                        try
                        {
                            num2 += Convert.ToDouble(this.Payments_Gridview.Rows[num3].Cells[1].Value);
                        }
                        catch (Exception exception1)
                        {
                            MessageBox.Show(exception1.Message, "ERROR MESSAGE", MessageBoxButtons.OK);
                        }
                        this.Txt_AmountPaidTotal.Text = num2.ToString("N2");
                        num3++;
                    }
                }
            }
            catch (Exception exception3)
            {
                MessageBox.Show(exception3.Message, "Error message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        public void Functionkeys(KeyEventArgs a)
        {
            Keys keyCode = a.KeyCode;
            if (keyCode == Keys.F2)
            {
                this.Btn_Close_Click(new object(), new EventArgs());
            }
            else
            {
                switch (keyCode)
                {
                    case Keys.F5:
                        this.Btn_Cash_Click(new object(), new EventArgs());
                        break;

                    case Keys.F6:
                    case Keys.F7:
                        break;

                    case Keys.F8:
                        this.Btn_Mpesa_Click(new object(), new EventArgs());
                        break;

                    case Keys.F9:
                        this.Btn_Card_Click(new object(), new EventArgs());
                        break;

                    default:
                        if (keyCode == Keys.F12)
                        {
                            this.Btn_CompleteTransaction_Click(new object(), new EventArgs());
                        }
                        break;
                }
            }
        }
        
        private void InputAmountPaid(object sender, KeyPressEventArgs e)
        {
            e.Handled = !(((e.KeyChar.ToString() == ".") || char.IsControl(e.KeyChar)) || char.IsNumber(e.KeyChar));
        }

        private void Payments_Gridview_KeyDown(object sender, KeyEventArgs e)
        {
            if (this.Payments_Gridview.Rows.Count <= 0)
            {
                MessageBox.Show("You have no item to delete !!", "ERROR MESSAGE", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            else if (e.KeyCode == Keys.Delete)
            {
                this.Payments_Gridview.Rows.Remove(this.Payments_Gridview.CurrentRow);
            }
        }

        private void Payments_Gridview_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            this.FindTotal();
        }

        private void Payments_Gridview_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            this.FindTotal();
        }

        private void Payments_Gridview_UserAddedRow(object sender, DataGridViewRowEventArgs e)
        {
            this.FindTotal();
        }

        private void Payments_Gridview_UserDeletedRow(object sender, DataGridViewRowEventArgs e)
        {
            this.FindTotal();
        }

        private void Transactions_Enter(object sender, EventArgs e)
        {
            while (base.Visible)
            {
            }
        }
 
        private void Txt_AmountPaidTotal_TextChanged(object sender, EventArgs e)
        {
            try
            {
                decimal num3 = Convert.ToDecimal(this.Txt_AmountPaidTotal.Text) - GrossAmount;
                this.Txt_Balance.Text = num3.ToString("N2");
                if (num3 > 0M)
                {
                    base.AcceptButton = this.Btn_CompleteTransaction;
                }
            }
            catch
            {
            }
        }

        private void Txt_PhoneNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !(char.IsNumber(e.KeyChar) || char.IsControl(e.KeyChar));
        }

       
    }
}

