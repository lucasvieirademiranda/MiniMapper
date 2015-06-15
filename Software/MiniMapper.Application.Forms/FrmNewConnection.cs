using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MiniMapper.Business.Entities.Core;
using MiniMapper.Business.Services.Implementations;

namespace MiniMapper.Application.Forms
{
    public partial class FrmNewConnection : Form
    {
        public FrmNewConnection()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Connection connection = new Connection()
            {
                Name = textBox1.Text,
                Provider = comboBox1.Text,
                Server =  textBox2.Text,
                Database = textBox3.Text,
                User = textBox4.Text,
                Password = textBox5.Text
            };

            var analyzer = AnalyzerFactory.Create(connection.Provider);

        }
    }
}
