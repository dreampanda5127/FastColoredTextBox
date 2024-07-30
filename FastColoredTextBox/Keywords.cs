using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace FastColoredTextBoxNS
{
    public partial class Keywords : Form
    {
        string keywordspath = "keywords.txt";
        List<string> keywords = new List<string>();
        FastColoredTextBox tb;
        public Keywords(FastColoredTextBox tb)
        {
            this.tb = tb;
            InitializeComponent();
            Load_Keywords();
        }
        private void Load_Keywords()
        {
            try
            {
                // Clear existing columns and rows

                DataTable dataTable = new DataTable();
                dataTable.Columns.Add("Keyword", typeof(string));

                // Read the file and populate the DataTable
                if (File.Exists(keywordspath))
                {
                    string line;
                    using (StreamReader reader = new StreamReader(this.keywordspath))
                    {
                        // Read the file line by line
                        while ((line = reader.ReadLine()) != null)
                        {
                            if (!string.IsNullOrWhiteSpace(line))
                            {
                                DataRow row = dataTable.NewRow();
                                row["Keyword"] = line;
                                dataTable.Rows.Add(row);
                                keywords.Add(line);
                            }
                        }
                    }

                    // Bind DataTable to DataGridView
                    dataGridView1.DataSource = dataTable;

                    // Add button column for actions if it doesn't already exist
                    if (!dataGridView1.Columns.Contains("ActionColumn"))
                    {
                        DataGridViewButtonColumn buttonColumn = new DataGridViewButtonColumn
                        {
                            Name = "ActionColumn",
                            HeaderText = "Action",
                            Text = "Remove",
                            UseColumnTextForButtonValue = true
                        };

                        dataGridView1.Columns.Add(buttonColumn);
                    }
                    tb.ClearStyle(StyleIndex.All);
                    Console.WriteLine(string.Join("|", keywords));
                    Console.WriteLine(keywords.Count);
                    tb.OnTextChanged();
                    // tb.Handle_Bold_By_Keyword(keywords);

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            Console.WriteLine("Hello world");
            if (e.ColumnIndex == dataGridView1.Columns["ActionColumn"].Index && e.RowIndex >= 0)
            {
                // Get the keyword from the row that contains the button clicked
                string keyword = dataGridView1.Rows[e.RowIndex].Cells["Keyword"].Value.ToString();

                // Remove the keyword from the file
                RemoveKeyword(keyword);

                
            }
        }
        private void RemoveKeyword(string keyword)
        {
            // Read all lines from the file
            string[] lines = File.ReadAllLines(keywordspath);
            // Write back only lines that do not match the keyword
            File.WriteAllLines(keywordspath, Array.FindAll(lines, line => line.Trim() != keyword.Trim()));
            // Refresh the DataGridView
            Load_Keywords();
        }
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.tbx_keyword = new System.Windows.Forms.TextBox();
            this.btn_add = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            dataGridView1.CellContentClick += dataGridView1_CellContentClick;
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.label1.Location = new System.Drawing.Point(30, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(48, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Keyword";
            // 
            // tbx_keyword
            // 
            this.tbx_keyword.Location = new System.Drawing.Point(84, 11);
            this.tbx_keyword.Name = "tbx_keyword";
            this.tbx_keyword.Size = new System.Drawing.Size(170, 20);
            this.tbx_keyword.TabIndex = 1;
            // 
            // btn_add
            // 
            this.btn_add.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btn_add.Location = new System.Drawing.Point(260, 9);
            this.btn_add.Name = "btn_add";
            this.btn_add.Size = new System.Drawing.Size(75, 23);
            this.btn_add.TabIndex = 2;
            this.btn_add.Text = "Add";
            this.btn_add.UseVisualStyleBackColor = true;
            this.btn_add.Click += new System.EventHandler(this.btn_add_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(33, 42);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(302, 266);
            this.dataGridView1.TabIndex = 3;
            // 
            // Keywords
            // 
            this.ClientSize = new System.Drawing.Size(368, 331);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.btn_add);
            this.Controls.Add(this.tbx_keyword);
            this.Controls.Add(this.label1);
            this.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Name = "Keywords";
            this.Text = "Keywords";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void btn_add_Click(object sender, EventArgs e)
        {
            string keyword = this.tbx_keyword.Text.Trim();
            if (!string.IsNullOrEmpty(keyword))
            {
                try
                {
                    // Check if the keyword already exists
                    bool keywordExists = false;
                    if (File.Exists(keywordspath))
                    {
                        using (StreamReader reader = new StreamReader(keywordspath))
                        {
                            string line;
                            while ((line = reader.ReadLine()) != null)
                            {
                                if (line.Trim().Equals(keyword, StringComparison.OrdinalIgnoreCase))
                                {
                                    keywordExists = true;
                                    break;
                                }
                            }
                        }
                    }

                    // If the keyword does not exist, append it to the file
                    if (!keywordExists)
                    {
                        using (StreamWriter writer = new StreamWriter(keywordspath, append: true))
                        {
                            writer.WriteLine(keyword);
                        }
                        // Clear the text box and reload keywords
                        tbx_keyword.Text = "";
                        Load_Keywords();
                    }
                    else
                    {
                        MessageBox.Show("Keyword already exists!", "Duplicate Entry", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Input keyword!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
