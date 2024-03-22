// Decompiled with JetBrains decompiler
// Type: EncryptDecrypt.frmMain
// Assembly: EncryptDecrypt, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AEC36668-A4C2-41A0-AF71-BB422E4FB67D
// Assembly location: C:\Program Files (x86)\Kharl Lampaoug Corporation\Crypthography\EncryptDecrypt.exe

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace EncryptDecrypt
{
    public class frmMain : Form
    {
        private IContainer components = (IContainer)null;
        private TabControl tabControl1;
        private TabPage tbSingle;
        private Label label1;
        private TextBox txtValue;
        private TabPage tbMultiple;
        private SplitContainer splitContainer1;
        private Button btnDecrypt;
        private TableLayoutPanel tableLayoutPanel1;
        private Label lblResult;
        private TextBox txtResult;
        private Label label2;
        private Button btnEncrypt;
        private DataGridView dgvList;
        private ErrorProvider errorProvider1;
        private DataGridViewTextBoxColumn ValueColumn;
        private DataGridViewTextBoxColumn ResultColumn;
        private DataGridViewTextBoxColumn StatusColumn;
        private Button btnCopy;
        private Panel panel1;
        private CheckBox chkAutoCopy;
        private ToolTip toolTip1;
        private DataGridViewButtonColumn RemoveColumn;

        public frmMain()
        {
            this.InitializeComponent();
            this.DelegateEvents();
        }

        private void DelegateEvents()
        {
            this.Activated += new EventHandler(this.FrmMain_Activated);
            this.btnEncrypt.Click += new EventHandler(this.ProcessAction);
            this.btnDecrypt.Click += new EventHandler(this.ProcessAction);
            this.txtResult.TextChanged += (EventHandler)((s, e) => this.lblResult.Visible = false);
            this.dgvList.CellClick += new DataGridViewCellEventHandler(this.DgvList_CellClick);
            this.tabControl1.SelectedIndexChanged += new EventHandler(this.TabControl1_SelectedIndexChanged);
            this.btnCopy.Click += BtnCopy_Click;
        }

        private void BtnCopy_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtResult.Text.Trim())) return;
            Clipboard.SetText(txtResult.Text);
        }

        private void FrmMain_Activated(object sender, EventArgs e)
        {
            this.txtValue.Focus();
        }

        private void TabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((Enumerables.TabSelected)Enum.Parse(typeof(Enumerables.TabSelected), this.tabControl1.SelectedIndex.ToString()) == Enumerables.TabSelected.Single)
                this.txtValue.Focus();
            else
                this.dgvList.Focus();
        }

        private void DgvList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex != this.RemoveColumn.Index || this.dgvList.Rows[e.RowIndex].IsNewRow)
                return;
            this.dgvList.Rows.RemoveAt(e.RowIndex);
        }

        private void ProcessAction(object sender, EventArgs e)
        {
            string action = ((Control)sender).Tag.ToString();
            if (!this.ValidToProceed(action))
                return;
            this.ProcessCrytoGraphy((Enumerables.CryptoType)Enum.Parse(typeof(Enumerables.CryptoType), action), this.GetListValues());
        }

        private void ProcessCrytoGraphy(Enumerables.CryptoType actionType, List<string> values)
        {
            List<Tuple<string, Enumerables.ProcessResult>> results = new List<Tuple<string, Enumerables.ProcessResult>>();
            Crypto crypto = new Crypto();
            foreach (string str in values)
            {
                try
                {
                    results.Add(Tuple.Create<string, Enumerables.ProcessResult>(actionType == Enumerables.CryptoType.Encrypt ? crypto.EncryptToString(str) : crypto.DecryptString(str), Enumerables.ProcessResult.Success));
                }
                catch
                {
                    results.Add(Tuple.Create<string, Enumerables.ProcessResult>(string.Empty, Enumerables.ProcessResult.Failed));
                }
            }
            this.ShowResult(results);
        }

        private void ShowResult(
          List<Tuple<string, Enumerables.ProcessResult>> results)
        {
            if ((Enumerables.TabSelected)Enum.Parse(typeof(Enumerables.TabSelected), this.tabControl1.SelectedIndex.ToString()) == Enumerables.TabSelected.Single)
            {
                Tuple<string, Enumerables.ProcessResult> tuple = results.FirstOrDefault<Tuple<string, Enumerables.ProcessResult>>();
                this.txtResult.Text = tuple.Item1;
                this.lblResult.Text = tuple.Item2.ToString();

                if (chkAutoCopy.Checked) BtnCopy_Click(null, null);
            }
            else
            {
                for (int index = 0; index <= results.Count - 1; ++index)
                {
                    Tuple<string, Enumerables.ProcessResult> result = results[index];
                    this.dgvList.Rows[index].Cells[this.ResultColumn.Index].Value = (object)result.Item1;
                    this.dgvList.Rows[index].Cells[this.StatusColumn.Index].Value = (object)result.Item2;
                }
            }
            this.SetResultControlState(true);
            this.SetResultForeColor();
        }

        private void SetResultControlState(bool controlstate)
        {
            if ((Enumerables.TabSelected)Enum.Parse(typeof(Enumerables.TabSelected), this.tabControl1.SelectedIndex.ToString()) != Enumerables.TabSelected.Single)
                return;
            this.lblResult.Visible = controlstate;
        }

        private void SetResultForeColor()
        {
            if ((Enumerables.TabSelected)Enum.Parse(typeof(Enumerables.TabSelected), this.tabControl1.SelectedIndex.ToString()) == Enumerables.TabSelected.Single)
                this.lblResult.ForeColor = this.GetStatusForeColor(this.lblResult.Text);
            else
                this.dgvList.Rows.OfType<DataGridViewRow>().Where<DataGridViewRow>((Func<DataGridViewRow, bool>)(r => !r.IsNewRow)).ToList<DataGridViewRow>().ForEach((Action<DataGridViewRow>)(r => r.Cells[this.StatusColumn.Index].Style.ForeColor = this.GetStatusForeColor(r.Cells[this.StatusColumn.Index].Value.ToString())));
        }

        private Color GetStatusForeColor(string value)
        {
            return (Enumerables.ProcessResult)Enum.Parse(typeof(Enumerables.ProcessResult), value) == Enumerables.ProcessResult.Failed ? Color.Red : Color.Green;
        }

        private bool ValidToProceed(string action = "")
        {
            bool flag = true;
            this.errorProvider1.Clear();
            if ((Enumerables.TabSelected)Enum.Parse(typeof(Enumerables.TabSelected), this.tabControl1.SelectedIndex.ToString()) == Enumerables.TabSelected.Single)
            {
                if (string.IsNullOrEmpty(this.txtValue.Text))
                {
                    this.errorProvider1.SetIconPadding((Control)this.txtValue, -20);
                    this.errorProvider1.SetError((Control)this.txtValue, "Value Required.");
                    flag = false;
                }
            }
            else if (this.dgvList.Rows.Count - 1 == 0)
            {
                this.errorProvider1.SetIconPadding((Control)this.dgvList, this.dgvList.Width / 2 * -1);
                this.errorProvider1.SetError((Control)this.dgvList, string.Format("List has no value to {0}", (object)action));
                flag = false;
            }
            return flag;
        }

        private List<string> GetListValues()
        {
            List<string> stringList = new List<string>();
            if ((Enumerables.TabSelected)Enum.Parse(typeof(Enumerables.TabSelected), this.tabControl1.SelectedIndex.ToString()) == Enumerables.TabSelected.Single)
                stringList.Add(this.txtValue.Text);
            else
                stringList = this.dgvList.Rows.OfType<DataGridViewRow>().Where<DataGridViewRow>((Func<DataGridViewRow, bool>)(r => !r.IsNewRow)).Select<DataGridViewRow, string>((Func<DataGridViewRow, string>)(r => r.Cells[this.ValueColumn.Index].Value.ToString())).ToList<string>();
            return stringList;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && this.components != null)
                this.components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tbSingle = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.btnCopy = new System.Windows.Forms.Button();
            this.lblResult = new System.Windows.Forms.Label();
            this.txtResult = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtValue = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tbMultiple = new System.Windows.Forms.TabPage();
            this.dgvList = new System.Windows.Forms.DataGridView();
            this.ValueColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ResultColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.StatusColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RemoveColumn = new System.Windows.Forms.DataGridViewButtonColumn();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.btnEncrypt = new System.Windows.Forms.Button();
            this.btnDecrypt = new System.Windows.Forms.Button();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.chkAutoCopy = new System.Windows.Forms.CheckBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.tabControl1.SuspendLayout();
            this.tbSingle.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tbMultiple.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tbSingle);
            this.tabControl1.Controls.Add(this.tbMultiple);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(6, 5);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(683, 144);
            this.tabControl1.TabIndex = 5;
            // 
            // tbSingle
            // 
            this.tbSingle.Controls.Add(this.tableLayoutPanel1);
            this.tbSingle.Location = new System.Drawing.Point(4, 27);
            this.tbSingle.Name = "tbSingle";
            this.tbSingle.Padding = new System.Windows.Forms.Padding(3);
            this.tbSingle.Size = new System.Drawing.Size(675, 113);
            this.tbSingle.TabIndex = 0;
            this.tbSingle.Text = "Single";
            this.tbSingle.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 56F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 74F));
            this.tableLayoutPanel1.Controls.Add(this.lblResult, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.txtResult, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.txtValue, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 2, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 34F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(669, 102);
            this.tableLayoutPanel1.TabIndex = 7;
            // 
            // btnCopy
            // 
            this.btnCopy.Location = new System.Drawing.Point(0, 1);
            this.btnCopy.Name = "btnCopy";
            this.btnCopy.Size = new System.Drawing.Size(59, 25);
            this.btnCopy.TabIndex = 7;
            this.btnCopy.Tag = "Encrypt";
            this.btnCopy.Text = "&Copy";
            this.btnCopy.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnCopy.UseVisualStyleBackColor = true;
            // 
            // lblResult
            // 
            this.lblResult.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.lblResult, 2);
            this.lblResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblResult.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblResult.Location = new System.Drawing.Point(3, 60);
            this.lblResult.Name = "lblResult";
            this.lblResult.Size = new System.Drawing.Size(589, 42);
            this.lblResult.TabIndex = 3;
            this.lblResult.Text = "Success";
            this.lblResult.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblResult.Visible = false;
            // 
            // txtResult
            // 
            this.txtResult.BackColor = System.Drawing.Color.Silver;
            this.txtResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtResult.Location = new System.Drawing.Point(59, 29);
            this.txtResult.Name = "txtResult";
            this.txtResult.ReadOnly = true;
            this.txtResult.Size = new System.Drawing.Size(533, 26);
            this.txtResult.TabIndex = 1;
            this.txtResult.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Location = new System.Drawing.Point(3, 26);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(50, 34);
            this.label2.TabIndex = 1;
            this.label2.Text = "Result :";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtValue
            // 
            this.txtValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtValue.Location = new System.Drawing.Point(59, 3);
            this.txtValue.Name = "txtValue";
            this.txtValue.Size = new System.Drawing.Size(533, 26);
            this.txtValue.TabIndex = 0;
            this.txtValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(50, 26);
            this.label1.TabIndex = 6;
            this.label1.Text = "Value";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tbMultiple
            // 
            this.tbMultiple.Controls.Add(this.dgvList);
            this.tbMultiple.Location = new System.Drawing.Point(4, 27);
            this.tbMultiple.Name = "tbMultiple";
            this.tbMultiple.Padding = new System.Windows.Forms.Padding(3);
            this.tbMultiple.Size = new System.Drawing.Size(675, 113);
            this.tbMultiple.TabIndex = 1;
            this.tbMultiple.Text = "Multiple";
            this.tbMultiple.UseVisualStyleBackColor = true;
            // 
            // dgvList
            // 
            this.dgvList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ValueColumn,
            this.ResultColumn,
            this.StatusColumn,
            this.RemoveColumn});
            this.dgvList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvList.Location = new System.Drawing.Point(3, 3);
            this.dgvList.Name = "dgvList";
            this.dgvList.Size = new System.Drawing.Size(669, 107);
            this.dgvList.TabIndex = 0;
            // 
            // ValueColumn
            // 
            this.ValueColumn.HeaderText = "Value";
            this.ValueColumn.Name = "ValueColumn";
            this.ValueColumn.Width = 200;
            // 
            // ResultColumn
            // 
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.Silver;
            dataGridViewCellStyle5.ForeColor = System.Drawing.Color.Black;
            this.ResultColumn.DefaultCellStyle = dataGridViewCellStyle5;
            this.ResultColumn.HeaderText = "Result";
            this.ResultColumn.Name = "ResultColumn";
            this.ResultColumn.ReadOnly = true;
            this.ResultColumn.Width = 200;
            // 
            // StatusColumn
            // 
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.Silver;
            dataGridViewCellStyle6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.StatusColumn.DefaultCellStyle = dataGridViewCellStyle6;
            this.StatusColumn.HeaderText = "Status";
            this.StatusColumn.Name = "StatusColumn";
            this.StatusColumn.ReadOnly = true;
            // 
            // RemoveColumn
            // 
            this.RemoveColumn.HeaderText = "";
            this.RemoveColumn.Name = "RemoveColumn";
            this.RemoveColumn.ReadOnly = true;
            this.RemoveColumn.Text = "Remove";
            this.RemoveColumn.UseColumnTextForButtonValue = true;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer1.IsSplitterFixed = true;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.tabControl1);
            this.splitContainer1.Panel1.Padding = new System.Windows.Forms.Padding(6, 5, 6, 5);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.btnEncrypt);
            this.splitContainer1.Panel2.Controls.Add(this.btnDecrypt);
            this.splitContainer1.Panel2.Padding = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.splitContainer1.Size = new System.Drawing.Size(695, 193);
            this.splitContainer1.SplitterDistance = 154;
            this.splitContainer1.TabIndex = 1;
            this.splitContainer1.TabStop = false;
            // 
            // btnEncrypt
            // 
            this.btnEncrypt.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnEncrypt.Location = new System.Drawing.Point(515, 5);
            this.btnEncrypt.Name = "btnEncrypt";
            this.btnEncrypt.Size = new System.Drawing.Size(87, 25);
            this.btnEncrypt.TabIndex = 2;
            this.btnEncrypt.Tag = "Encrypt";
            this.btnEncrypt.Text = "&Encrypt";
            this.btnEncrypt.UseVisualStyleBackColor = true;
            // 
            // btnDecrypt
            // 
            this.btnDecrypt.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnDecrypt.Location = new System.Drawing.Point(602, 5);
            this.btnDecrypt.Name = "btnDecrypt";
            this.btnDecrypt.Size = new System.Drawing.Size(87, 25);
            this.btnDecrypt.TabIndex = 3;
            this.btnDecrypt.Tag = "Decrypt";
            this.btnDecrypt.Text = "&Decrypt";
            this.btnDecrypt.UseVisualStyleBackColor = true;
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.chkAutoCopy);
            this.panel1.Controls.Add(this.btnCopy);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(598, 29);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(68, 28);
            this.panel1.TabIndex = 7;
            // 
            // chkAutoCopy
            // 
            this.chkAutoCopy.AutoSize = true;
            this.chkAutoCopy.Location = new System.Drawing.Point(9, 7);
            this.chkAutoCopy.Name = "chkAutoCopy";
            this.chkAutoCopy.Size = new System.Drawing.Size(18, 17);
            this.chkAutoCopy.TabIndex = 8;
            this.toolTip1.SetToolTip(this.chkAutoCopy, "Auto Copy");
            this.chkAutoCopy.UseVisualStyleBackColor = true;
            // 
            // toolTip1
            // 
            this.toolTip1.IsBalloon = true;
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(695, 193);
            this.Controls.Add(this.splitContainer1);
            this.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MinimumSize = new System.Drawing.Size(711, 232);
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Cryptography";
            this.tabControl1.ResumeLayout(false);
            this.tbSingle.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tbMultiple.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvList)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }
    }
}
