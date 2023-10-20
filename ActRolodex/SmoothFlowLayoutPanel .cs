using System.Windows.Forms;

namespace ACT_Plugin
{
    public partial class SmoothFlowLayoutPanel : FlowLayoutPanel
    {
        public SmoothFlowLayoutPanel() : base()
        {
            this.DoubleBuffered = true;
        }
    }
}
