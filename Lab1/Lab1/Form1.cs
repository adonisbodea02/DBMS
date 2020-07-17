using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Lab1
{
    public partial class Form1 : Form
    {
        SqlConnection dbCon;
        SqlDataAdapter daATMs, daTransactions;
        DataSet ds;
        SqlCommandBuilder cb;
        BindingSource bsATMs, bsTransactions;

        private void button1_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow item in this.dataGridView2.SelectedRows)
            {
                dataGridView2.Rows.RemoveAt(item.Index);
            }
            daTransactions.Update(ds, "Tasks");
        }

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            dbCon = new SqlConnection("Data Source=DESKTOP-9IL2B76\\SQLEXPRESS;Initial Catalog=ExamPracticeDBMS;Integrated Security=True");
            ds = new DataSet();
            daATMs = new SqlDataAdapter("select * from Developers", dbCon);
            daTransactions = new SqlDataAdapter("select * from Tasks", dbCon);
            cb = new SqlCommandBuilder(daTransactions);


            daATMs.Fill(ds, "Developers");
            daTransactions.Fill(ds, "Tasks");

            DataRelation dr = new DataRelation("FK_Developers_Tasks", ds.Tables["Developers"].Columns["did"],
                ds.Tables["Tasks"].Columns["did"]);

            ds.Relations.Add(dr);

            bsATMs = new BindingSource();
            bsATMs.DataSource = ds;
            bsATMs.DataMember = "Developers";

            bsTransactions = new BindingSource();
            bsTransactions.DataSource = bsATMs;
            bsTransactions.DataMember = "FK_Developers_Tasks";

            dataGridView1.DataSource = bsATMs;
            dataGridView2.DataSource = bsTransactions;
        }
    }
}
