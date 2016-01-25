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

namespace SudokuGame
{
    public partial class MainWindow : Form, SudokuGameEngineLib.IGameUI
    {
        private IGameEngine _eng;

        public MainWindow(IGameEngine eng)
        {
            _eng = eng;
            InitializeComponent();
            eng.SetUI(this);
        }


        private void btnNewGame_Click(object sender, EventArgs e)
        {
            NewGameWizard wizard = new NewGameWizard();
            wizard.ShowDialog();
            NewGameWizard.NewGameSettings settings = wizard.Settings;

            try
            {
                switch (settings.type)
                {
                    case NewGameWizard.InputType.FILE:
                        if (!_eng.ReadGameFromFile(settings.fileName))
                        {
                            string msg = _eng.LastError().Message();
                            string title = "Error!";
                            MessageBoxButtons buttons = MessageBoxButtons.OK;
                            MessageBox.Show(msg, title, buttons);
                        }
                        break;

                    case NewGameWizard.InputType.USER:
                        if (!_eng.CreateGame(settings.initVals, settings.layout))
                        {
                            string msg = _eng.LastError().Message();
                            string title = "Error!";
                            MessageBoxButtons buttons = MessageBoxButtons.OK;
                            MessageBox.Show(msg, title, buttons);
                        }

                        break;
                    case NewGameWizard.InputType.NONE:
                        break;
                    default:
                        System.Diagnostics.Debug.Assert(false, "Unknown input type.");
                        break;
                }
            }
            catch (NotImplementedException)
            {
                string msg = "Not implemented yet!";
                string title = "Error!";
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                MessageBox.Show(msg, title, buttons);
            }
        }


        private void btnReset_Click(object sender, EventArgs e)
        {
            try
            {
                _eng.ResetGame();
            }
            catch (NotImplementedException)
            {
                string msg = "Not implemented yet!";
                string title = "Error!";
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                MessageBox.Show(msg, title, buttons);
            }
        }

        private void btnSolve_Click(object sender, EventArgs e)
        {
            try
            {
                if (!_eng.Solve())
                {
                    string msg = _eng.LastError().Message();
                    string title = "Error!";
                    MessageBoxButtons buttons = MessageBoxButtons.OK;
                    MessageBox.Show(msg, title, buttons);
                }
            }
            catch (NotImplementedException)
            {
                string msg = "Not implemented yet!";
                string title = "Error!";
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                MessageBox.Show(msg, title, buttons);
            }
        }

        private void btnHint_Click(object sender, EventArgs e)
        {
            try
            {
                if (!_eng.Hint())
                {
                    string msg = _eng.LastError().Message();
                    string title = "Error!";
                    MessageBoxButtons buttons = MessageBoxButtons.OK;
                    MessageBox.Show(msg, title, buttons);
                }
            }
            catch (NotImplementedException)
            {
                string msg = "Not implemented yet!";
                string title = "Error!";
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                MessageBox.Show(msg, title, buttons);
            }
        }


        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveFileDialog d = new SaveFileDialog();
            d.ShowDialog();

            try
            {
                if (!_eng.SaveGame(d.FileName))
                {
                    string msg = _eng.LastError().Message();
                    string title = "Error!";
                    MessageBoxButtons buttons = MessageBoxButtons.OK;
                    MessageBox.Show(msg, title, buttons);
                }
            }
            catch (NotImplementedException)
            {
                string msg = "Not implemented yet!";
                string title = "Error!";
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                MessageBox.Show(msg, title, buttons);
            }
        }

        public void setTableModel(ISudokuTableReadonlyModel model)
        {
            throw new NotImplementedException();
        }
    }
}
