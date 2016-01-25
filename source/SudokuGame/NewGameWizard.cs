using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SudokuGameEngineLib;
using SudokuLib;

namespace SudokuGame
{
    public partial class NewGameWizard : Form
    {
        public enum InputType
        {
            FILE, USER, NONE
        };

        public struct NewGameSettings
        {
            public InputType type;
            public string fileName;
            public IDictionary<Coordinate, int> initVals;
            public SquareRegions layout;
        }

        private NewGameSettings _result;


        public NewGameWizard()
        {
            _result = new NewGameSettings();
            _result.type = InputType.NONE;
            _result.fileName = "";
            _result.initVals = null;
            _result.layout = null;
            InitializeComponent();
        }


        public NewGameSettings Settings 
        {
            get { return _result; }
        }


        private void btnBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog d = new OpenFileDialog();
            d.InitialDirectory = ".";
            d.RestoreDirectory = true;

            if (d.ShowDialog() == DialogResult.OK)
            {
                this.txtFileName.Text = d.SafeFileName;
            }

        }

        private void btnFileOK_Click(object sender, EventArgs e)
        {
            _result.fileName = this.txtFileName.Text;
            _result.type = InputType.FILE;
            this.Close();
        }

        private void btnFileCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        private void btnEditorOK_Click(object sender, EventArgs e)
        {
            _result.type = InputType.USER;
            _result.initVals = new Dictionary<Coordinate, int>();
            _result.layout = new SquareRegions();
            this.Close();
        }


        private void btnEditorCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
