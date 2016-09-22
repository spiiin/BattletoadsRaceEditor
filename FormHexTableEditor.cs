using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace CadObjEditor
{
    public partial class FormHexTableEditor : Form
    {
        BaseLoader loader = new BattletoadsLoader();
        byte[] romdata = new byte[0];

        public FormHexTableEditor()
        {
            InitializeComponent();
        }

        public void setDataSource<T>()
            where T : GameObject
        {
            var gameObjects = loader.load(romdata);
            dgvGameObjects.DataSource = gameObjects.GetList().ConvertAll(x => x as T);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            loader.setFormText(this);
            loadRom();
            dgvGameObjects.AutoGenerateColumns = true;
            loader.initDataSource(this);
            for (int i = 0; i < dgvGameObjects.Columns.Count; i++)
            {
                dgvGameObjects.Columns[i].Width = 55;
            }
        }

        private void loadRom()
        {
            if (ofDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {

                    var Filename = ofDialog.FileName;
                    int size = (int)new FileInfo(Filename).Length;
                    using (FileStream f = File.OpenRead(Filename))
                    {
                        romdata = new byte[size];
                        f.Read(romdata, 0, size);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                this.Close();
            }
        }

        private void saveRom()
        {
            try
            {
                var Filename = ofDialog.FileName;
                using (FileStream f = File.OpenWrite(Filename))
                {
                    f.Write(romdata, 0, romdata.Length);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dgvGameObjects_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            loader.cellFormatting(dgvGameObjects, e);
        }

        private void dgvGameObjects_CellParsing(object sender, DataGridViewCellParsingEventArgs e)
        {
            loader.cellParsing(dgvGameObjects, e);
        }

        private void tbSave_Click(object sender, EventArgs e)
        {
            var lo = (List<BattletoadsGameObject>)dgvGameObjects.DataSource;
            loader.save(romdata, new GameObjectList(lo.ConvertAll(x=>x as GameObject)));
            saveRom();
            tbSave.Enabled = false;
        }

        private void dgvGameObjects_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            tbSave.Enabled = true;
        }
    }
}
