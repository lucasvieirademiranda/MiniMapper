using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Practices.Unity;
using MiniMapper.Application.Forms;

namespace MiniMapper
{
    public partial class Main : Form
    {
        private IUnityContainer UnityContainer { get; set; }

        public Main(IUnityContainer unityContainer)
        {
            UnityContainer = unityContainer;
            InitializeComponent();
        }

        private void newDatabaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new FrmNewConnection();
            form.Show();
        }
    }
}
