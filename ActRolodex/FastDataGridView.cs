using System.Windows.Forms;

namespace ACT_Plugin
{
    class FastDataGridView : DataGridView
    {
        public FastDataGridView() : base()
        {
            this.DoubleBuffered = true;
        }
    }
}
