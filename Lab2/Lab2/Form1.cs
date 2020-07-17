using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;

namespace Lab2
{
    public partial class Form1 : Form
    {
        SqlConnection connection;
        SqlDataAdapter dataAdapterParent, dataAdapterChild;
        DataSet dataSet;
        SqlCommandBuilder commandBuilder;
        BindingSource bindingSourceParent, bindingSourceChild;
        String parentTable;
        String childTable;

        private void button1_Click(object sender, EventArgs e)
        {
            dataAdapterChild.Update(dataSet, childTable);
        }

        public Form1()
        {
            InitializeComponent();

            String connectionString = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
            connection = new SqlConnection(connectionString);

            dataSet = new DataSet();
            String parentSelect = ConfigurationManager.AppSettings["SelectParents"];
            String childSelect = ConfigurationManager.AppSettings["SelectChildren"];
            dataAdapterParent = new SqlDataAdapter(parentSelect, connection);
            dataAdapterChild = new SqlDataAdapter(childSelect, connection);

            commandBuilder = new SqlCommandBuilder(dataAdapterChild);

            parentTable = ConfigurationManager.AppSettings["ParentTable"];
            childTable = ConfigurationManager.AppSettings["ChildTable"];
            dataAdapterParent.Fill(dataSet, parentTable);
            dataAdapterChild.Fill(dataSet, childTable);

            String fkName = ConfigurationManager.AppSettings["FKName"];
            String fkColumn = ConfigurationManager.AppSettings["FKColumn"];
            DataRelation dataRelation = new DataRelation(fkName, dataSet.Tables[parentTable].Columns[fkColumn],
                dataSet.Tables[childTable].Columns[fkColumn]);
            dataSet.Relations.Add(dataRelation);

            bindingSourceParent = new BindingSource();
            bindingSourceParent.DataSource = dataSet;
            bindingSourceParent.DataMember = parentTable;

            bindingSourceChild = new BindingSource();
            bindingSourceChild.DataSource = bindingSourceParent;
            bindingSourceChild.DataMember = fkName;

            dataGridView1.DataSource = bindingSourceParent;
            dataGridView2.DataSource = bindingSourceChild;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
