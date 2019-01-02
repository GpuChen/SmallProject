namespace RegexControl
{
    partial class Form1
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置受控資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.btnSubmit = new System.Windows.Forms.Button();
            this.tbInput = new System.Windows.Forms.TextBox();
            this.lbRegexRule = new System.Windows.Forms.Label();
            this.lbResult = new System.Windows.Forms.Label();
            this.cbRegex = new System.Windows.Forms.ComboBox();
            this.tbNewKey = new System.Windows.Forms.TextBox();
            this.tbNewRule = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btnRemove = new System.Windows.Forms.Button();
            this.btnCreate = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnSubmit
            // 
            this.btnSubmit.Location = new System.Drawing.Point(369, 97);
            this.btnSubmit.Name = "btnSubmit";
            this.btnSubmit.Size = new System.Drawing.Size(75, 23);
            this.btnSubmit.TabIndex = 0;
            this.btnSubmit.Text = "Submit";
            this.btnSubmit.UseVisualStyleBackColor = true;
            // 
            // tbInput
            // 
            this.tbInput.Location = new System.Drawing.Point(82, 98);
            this.tbInput.Name = "tbInput";
            this.tbInput.Size = new System.Drawing.Size(281, 22);
            this.tbInput.TabIndex = 1;
            // 
            // lbRegexRule
            // 
            this.lbRegexRule.AutoSize = true;
            this.lbRegexRule.Location = new System.Drawing.Point(16, 25);
            this.lbRegexRule.Name = "lbRegexRule";
            this.lbRegexRule.Size = new System.Drawing.Size(89, 12);
            this.lbRegexRule.TabIndex = 2;
            this.lbRegexRule.Text = "正規表示規則：";
            // 
            // lbResult
            // 
            this.lbResult.AutoSize = true;
            this.lbResult.Location = new System.Drawing.Point(16, 63);
            this.lbResult.Name = "lbResult";
            this.lbResult.Size = new System.Drawing.Size(41, 12);
            this.lbResult.TabIndex = 3;
            this.lbResult.Text = "結果：";
            // 
            // cbRegex
            // 
            this.cbRegex.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbRegex.FormattingEnabled = true;
            this.cbRegex.Location = new System.Drawing.Point(82, 138);
            this.cbRegex.Name = "cbRegex";
            this.cbRegex.Size = new System.Drawing.Size(281, 20);
            this.cbRegex.TabIndex = 4;
            // 
            // tbNewKey
            // 
            this.tbNewKey.Location = new System.Drawing.Point(82, 207);
            this.tbNewKey.Name = "tbNewKey";
            this.tbNewKey.Size = new System.Drawing.Size(281, 22);
            this.tbNewKey.TabIndex = 5;
            // 
            // tbNewRule
            // 
            this.tbNewRule.Location = new System.Drawing.Point(82, 235);
            this.tbNewRule.Name = "tbNewRule";
            this.tbNewRule.Size = new System.Drawing.Size(281, 22);
            this.tbNewRule.TabIndex = 6;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 192);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 7;
            this.label1.Text = "定義新規則";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 102);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 8;
            this.label2.Text = "輸入內文";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(16, 141);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 9;
            this.label3.Text = "選用正規式";
            // 
            // btnRemove
            // 
            this.btnRemove.Location = new System.Drawing.Point(369, 136);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(75, 23);
            this.btnRemove.TabIndex = 10;
            this.btnRemove.Text = "Remove";
            this.btnRemove.UseVisualStyleBackColor = true;
            // 
            // btnCreate
            // 
            this.btnCreate.Location = new System.Drawing.Point(369, 233);
            this.btnCreate.Name = "btnCreate";
            this.btnCreate.Size = new System.Drawing.Size(75, 23);
            this.btnCreate.TabIndex = 11;
            this.btnCreate.Text = "Create";
            this.btnCreate.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(23, 212);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(24, 12);
            this.label4.TabIndex = 12;
            this.label4.Text = "Key";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(23, 238);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(32, 12);
            this.label5.TabIndex = 13;
            this.label5.Text = "Value";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(459, 269);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.btnCreate);
            this.Controls.Add(this.btnRemove);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbNewRule);
            this.Controls.Add(this.tbNewKey);
            this.Controls.Add(this.cbRegex);
            this.Controls.Add(this.lbResult);
            this.Controls.Add(this.lbRegexRule);
            this.Controls.Add(this.tbInput);
            this.Controls.Add(this.btnSubmit);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSubmit;
        private System.Windows.Forms.TextBox tbInput;
        private System.Windows.Forms.Label lbRegexRule;
        private System.Windows.Forms.Label lbResult;
        private System.Windows.Forms.ComboBox cbRegex;
        private System.Windows.Forms.TextBox tbNewKey;
        private System.Windows.Forms.TextBox tbNewRule;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnRemove;
        private System.Windows.Forms.Button btnCreate;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
    }
}

