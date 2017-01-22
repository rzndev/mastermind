using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MasterMind
{
    public partial class FormStatistics : Form
    {
        public FormStatistics()
        {
            InitializeComponent();
        }

        private Statistics stat;

        public Statistics staticstics {
            get 
            { 
                return stat;
            }
            set 
            { 
                stat = value;
                textBox1.Text = stat.ToString(); 
            }
        }
    }
}
