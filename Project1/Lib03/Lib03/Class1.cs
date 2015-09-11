using System;
using System.Drawing;
using System.Windows.Forms;
using Autodesk.AutoCAD.Runtime;

[assembly: CommandClass(typeof(Lib03.Wform))]

namespace Lib03
{
    public class Wform : Form {

        public Wform()
        {
            Text = "Modal dialog Book03";
            Size = new Size(400, 200);
            MaximumSize = new Size(600, 400);
            BackColor = Color.BlanchedAlmond;
        }

        [CommandMethod("BOOK03")]
        static public void book03() {
            Application.Run(new Wform());
        }
    }
}
