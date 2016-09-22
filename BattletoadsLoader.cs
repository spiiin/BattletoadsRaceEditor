using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace CadObjEditor
{
    class BattletoadsLoader : BaseLoader
    {
        const int objCount = 114;
        const int objSize = 11;
        const int startAddr = 0x1EDFE;
        public GameObjectList load(byte[] romdata)
        {
            var objects = new List<GameObject>();
            for (int i = 0; i < objCount; i++)
            {
                byte type = romdata[startAddr + i * objSize + 0];
                byte xScreen = romdata[startAddr + i * objSize + 1];
                byte x   = romdata[startAddr + i * objSize + 2];
                byte x1Screen   = romdata[startAddr + i * objSize + 3];
                byte x1 = romdata[startAddr + i * objSize + 4];
                byte yScreen   = romdata[startAddr + i * objSize + 5];
                byte y    = romdata[startAddr + i * objSize + 6];
                byte zScreen = romdata[startAddr + i * objSize + 7];
                byte z   = romdata[startAddr + i * objSize + 8];
                byte blinkTime  = romdata[startAddr + i * objSize + 9];
                byte jumpPower  = romdata[startAddr + i * objSize + 10];
                var obj = new BattletoadsGameObject(type, xScreen, x, x1Screen, x1, yScreen, y, zScreen, z, blinkTime, jumpPower);
                objects.Add(obj);
            }
            return new GameObjectList(objects);
        }

        public void save(byte[] romdata, GameObjectList objects)
        {
            for (int i = 0; i < objCount; i++)
            {
                var obj = (BattletoadsGameObject)objects.GetList()[i];
                romdata[startAddr + i * objSize + 0] = (byte)obj.type;
                romdata[startAddr + i * objSize + 1] = (byte)obj.xScreen;
                romdata[startAddr + i * objSize + 2] = (byte)obj.x;
                romdata[startAddr + i * objSize + 3] = (byte)obj.x1Screen;
                romdata[startAddr + i * objSize + 4] = (byte)obj.x1;
                romdata[startAddr + i * objSize + 5] = (byte)obj.yScreen;
                romdata[startAddr + i * objSize + 6] = (byte)obj.y;
                romdata[startAddr + i * objSize + 7] = (byte)obj.zScreen;
                romdata[startAddr + i * objSize + 8] = (byte)obj.z;
                romdata[startAddr + i * objSize + 9] = (byte)obj.blinkTime;
                romdata[startAddr + i * objSize +10] = (byte)obj.jumpPower;
            }
        }

        Dictionary<int, Color> colorDict = new Dictionary<int, Color>()
            {
                { 0x26, Color.Red },
                { 0x27, Color.Red },
                { 0x19, Color.Red },
                { 0x49, Color.Red },
                { 0x22, Color.Red },

                { 0x23, Color.FromArgb(128, 128, 110) },
                { 0x24, Color.FromArgb(128, 196, 150) },
                { 0x25, Color.FromArgb(128, 128, 190) },

                { 0x2A, Color.FromArgb(160, 128, 190) },

                { 0x28, Color.FromArgb(255, 255, 170) },
                { 0x2B, Color.FromArgb(255, 255, 130) },
            };

        public void cellFormatting(DataGridView dgvGameObjects, DataGridViewCellFormattingEventArgs e)
        {
            if (e.Value != null)
            {
                long value = 0;
                if (long.TryParse(e.Value.ToString(), out value))
                {
                    e.Value = "0x" + value.ToString("X");
                    e.FormattingApplied = true;
                }
            }

            //coloring
            if (e.ColumnIndex == 0)
            {
                var row = dgvGameObjects.Rows[e.RowIndex];
                int cell0 = Convert.ToInt32(row.Cells[0].Value);
                row.DefaultCellStyle.BackColor = colorDict.ContainsKey(cell0) ? colorDict[cell0] : Color.White;
            }
        }

        public void cellParsing(DataGridView dgvGameObjects, DataGridViewCellParsingEventArgs e)
        {
            if (e != null && e.Value != null && e.DesiredType.Equals(typeof(int)))
            {
                var s = e.Value.ToString();
                if (s.StartsWith("0x") || s.StartsWith("0X"))
                {
                    try
                    {
                        int hex = (int)Convert.ToInt32(s.ToString(), 16);
                        e.Value = hex;
                        e.ParsingApplied = true;
                    }
                    catch
                    {
                        // Not a Valid Hexadecimal
                    }
                }
            }
        }

        public void setFormText(FormHexTableEditor frmMain)
        {
            frmMain.Text = "Battletoads Turbo Tunnel Editor";
        }

        public void initDataSource(FormHexTableEditor frmMain)
        {
            frmMain.setDataSource<BattletoadsGameObject>();
        }
    }

    public class BattletoadsGameObject : GameObject
    {
        public BattletoadsGameObject(int type, int xScreen, int x, int x1Screen, int x1, int yScreen, int y, int zScreen, int z, int blinkTime, int jumpPower)
        {
            this.type = type;
            this.xScreen = xScreen;
            this.x = x;
            this.x1Screen = x1Screen;
            this.x1 = x1;
            this.yScreen = yScreen;
            this.y = y;
            this.zScreen = zScreen;
            this.z = z;
            this.blinkTime = blinkTime;
            this.jumpPower = jumpPower;
        }
        public int type { get; set; }
        public int xScreen { get; set; }
        public int x { get; set; }
        public int x1Screen { get; set; }
        public int x1 { get; set; }
        public int yScreen { get; set; }
        public int y { get; set; }
        public int zScreen { get; set; }
        public int z { get; set; }
        public int blinkTime { get; set; }
        public int jumpPower { get; set; }

    }
}
