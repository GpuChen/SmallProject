using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RegexControl
{
    public partial class Form1 : Form
    {
        bool result;
        //Regex regex = new Regex();

        public Form1()
        {
            InitializeComponent();

            List<regexItem> regexList = new List<regexItem>();
            //Dictionary<string, string> regexList = new Dictionary<string, string>();
            regexList.Add(new regexItem { key = "Null", regexRule = @"" });
            regexList.Add(new regexItem { key = "日期格式 yyyy/mm/dd", regexRule = @"^\d{4}/\d{2}/\d{2}$" });
            regexList.Add(new regexItem { key = "日期限制 3000/12/31", regexRule = @"^[1-3]{1}[0-9]{3}/[0-1][0-9]/[0-3][0-9]$" });
            regexList.Add(new regexItem { key = "身分證", regexRule = @"^\w[12]\d{8}$" });

            cbRegex.DataSource = regexList;

            //cbRegex.DataSource = new BindingSource(regexList, null);
            //cbRegex.ValueMember = "key";
            //cbRegex.DisplayMember = "value";

            cbRegex.SelectedIndexChanged += (object senter, EventArgs args) =>
            {
                var obj = from data in regexList
                          where data.key == cbRegex.Text
                          select data;
                foreach (regexItem item in obj) {
                    lbRegexRule.Text = string.Format("正規表示規則：{0}", item.regexRule);
                }
            };


            btnSubmit.Click += (object senter, EventArgs args) =>
            {
                var obj = from data in regexList
                          where data.key == cbRegex.Text
                          select data;
                foreach (regexItem item in obj)
                {
                    result = Regex.IsMatch(tbInput.Text, item.regexRule); // Default before start
                }

                lbResult.Text = string.Format("結果：{0}", result.ToString());

            };
            /*
            btnRemove.Click += (object senter, EventArgs args) =>
            {
                var obj = from data in regexList
                          where data.key == cbRegex.Text
                          select data;
                foreach (regexItem item in obj)
                {
                    if (item.key.Equals("Null")) return;
                    regexList.Remove(item);
                }
                cbRegex.DataSource = regexList;
            };
            btnCreate.Click += (object senter, EventArgs args) =>
            {
                if (tbNewKey.Text.Equals("") || tbNewRule.Text.Equals("")) return;
                regexList.Add(new regexItem { key = tbNewKey.Text, regexRule = tbNewRule.Text });
                cbRegex.DataSource = regexList;
            };
            */
        }

    }

    class regexItem
    {
        public string key { set; get; }
        public string regexRule { set; get; }

        public override string ToString()
        {
            return key.ToString();
        }
    }
}
