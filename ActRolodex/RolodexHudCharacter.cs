using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ACT_Plugin
{
    public partial class RolodexHudCharacter : UserControl
    {
        private RolodexCharacter _character = new RolodexCharacter();

        public RolodexHudCharacter()
        {
            InitializeComponent();
        }

        public void SetCharacter(RolodexCharacter character)
        {
            _character = character;
            if (string.IsNullOrEmpty(_character.Id))
            {
                lblChar.Enabled = false;
                lblGuild.Enabled = false;
            }
            lblChar.Text = $"{_character.Name}";
            lblGuild.Text = $"{_character.Guild}";
            lblClass.Text = $"{_character.Class}";
            lblRank.Text = $"{_character.Rank}";
        }

        private void lblChar_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start($"https://u.eq2wire.com/soe/character_detail/{_character.Id}");
        }

        private void lblGuild_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start($"https://u.eq2wire.com/soe/guild_detail/{_character.GuildId}");
        }

        private void lblRank_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start($"https://eq2reapp.github.io/eq2cmp-host/?char={_character.Server}.{_character.Name}");
        }
    }
}
