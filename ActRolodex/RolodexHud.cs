// reference:System.Text..dll
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.IO;

namespace ACT_Plugin
{
    public partial class RolodexHud : Form
    {
        private const int MAX_SHOWN_CHARS = 35;
        private const int MIN_REFRESH_MINUTES = 60 * 5;
        private List<RolodexCharacter> _cached = new List<RolodexCharacter>();
        private List<RolodexCharacter> _pending = new List<RolodexCharacter>();
        private string _filter = "";
        private string _lastServer = "";
        private bool _listIsDirty = false;

        public RolodexHud()
        {
            InitializeComponent();
        }

        private RolodexCharacter GetCached(string server, string name)
        {
            RolodexCharacter retVal = null;
            lock(_cached)
            {
                RolodexCharacter cached = _cached.Find(c => c.Server == server && c.Name == name);
                if (cached != null && DateTime.Now.Subtract(cached.Updated).TotalMinutes <= MIN_REFRESH_MINUTES)
                {
                    retVal = cached;
                }
            }
            return retVal;
        }

        private void AddCached(RolodexCharacter character)
        {
            if (character != null)
            {
                lock (_cached)
                {
                    RolodexCharacter cached = _cached.Find(c => c.Server == character.Server && c.Name == character.Name);
                    if (cached != null)
                    {
                        _cached.Remove(cached);
                    }
                    _cached.Add(character);
                }
            }
        }

        private bool AddPending(string server, string name)
        {
            bool added = false;

            lock (_pending)
            {
                RolodexCharacter pending = _pending.Find(c => c.Server == server && c.Name == name);
                if (pending == null)
                {
                    _pending.Add(new RolodexCharacter() {
                        Server = server,
                        Name = name
                    });
                    added = true;
                }
            }

            return added;
        }

        private static async Task<RolodexCharacter> FetchCharacter(string server, string name)
        {
            RolodexCharacter fetchedChar = null;

            var apiUrl = $"https://census.daybreakgames.com/s:eq2cmp/json/get/eq2/character/?name.first_lower={name.ToLower()}&locationdata.world={server}";
            try
            {
                var request = WebRequest.Create(apiUrl);
                var response = (HttpWebResponse)await Task.Factory.FromAsync(
                    request.BeginGetResponse,
                    request.EndGetResponse,
                    null);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    using (var dataStream = response.GetResponseStream())
                    using (var reader = new StreamReader(dataStream))
                    {
                        string jsonText = reader.ReadToEnd();
                        var json = JObject.Parse(jsonText);
                        var potency = (double)json.SelectToken("character_list[0].stats.combat.basemodifier");
                        var cb = (double)json.SelectToken("character_list[0].stats.combat.critbonus");
                        var fervor = (double)json.SelectToken("character_list[0].stats.combat.fervor");
                        fetchedChar = new RolodexCharacter()
                        {
                            Server = server,
                            Name = name,
                            Guild = (string)json.SelectToken("character_list[0].guild.name") ?? "",
                            Class = (string)json.SelectToken("character_list[0].type.class") ?? "",
                            Level = (int)json.SelectToken("character_list[0].type.level"),
                            Id = (string)json.SelectToken("character_list[0].id") ?? "",
                            GuildId = (string)json.SelectToken("character_list[0].guild.id") ?? "",
                            Rank = (int)Math.Ceiling(2 * (potency / 100000.0) * (cb / 4000.0) * (fervor / 100.0)),
                            Updated = DateTime.Now
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                fetchedChar = null;
            }

            return fetchedChar;
        }

        private delegate void UpdateCharacterViewHandler();
        private void UpdateCharacterView()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new UpdateCharacterViewHandler(UpdateCharacterView));
            }
            else
            {
                if (_listIsDirty)
                {
                    List<RolodexCharacter> shownChars = new List<RolodexCharacter>();
                    lock (_cached)
                    {
                        foreach (var character in _cached)
                        {
                            if (string.IsNullOrEmpty(_filter) ||
                                character.Name.ToLower().Contains(_filter) ||
                                character.Guild.ToLower().Contains(_filter) ||
                                character.Class.ToLower().Contains(_filter))
                            {
                                shownChars.Add(character);
                            }
                        }
                    }
                    shownChars.Sort((x, y) => -x.Updated.CompareTo(y.Updated));

                    List<Control> controls = new List<Control>();
                    foreach (var character in shownChars)
                    {
                        if (string.IsNullOrEmpty(_filter) ||
                            character.Name.ToLower().Contains(_filter) ||
                            character.Guild.ToLower().Contains(_filter))
                        {
                            var pnlChar = new RolodexHudCharacter();
                            pnlChar.SetCharacter(character);
                            pnlChar.Left = 0;
                            pnlChar.Width = pnlChars.Width - 24;
                            controls.Add(pnlChar);
                        }
                    }
                    pnlChars.Controls.Clear();
                    pnlChars.Controls.AddRange(controls.ToArray());
                    _listIsDirty = false;
                }
            }
        }

        public void QueueCharacter(string server, string name)
        {
            Task.Run(() => AddCharacterAsync(server, name)).ConfigureAwait(false);
        }

        public async Task AddCharacterAsync(string server, string name)
        {
            // Save the server so ad-hoc lookups can guess the server
            _lastServer = server;

            var cached = GetCached(server, name);
            if (cached != null)
            {
                cached.Updated = DateTime.Now;
            }
            else
            {
                // If the char is already being searched, just return
                if (!AddPending(server, name))
                {
                    return;
                }

                // Let's get the char info then cache it
                var fetchedChar = await FetchCharacter(server, name);
                if (fetchedChar != null)
                {
                    AddCached(fetchedChar);
                }
            }

            _listIsDirty = true;
        }

        private void RolodexHud_Load(object sender, EventArgs e)
        {
            timerUpdateList.Enabled = true;
        }

        private void RolodexHud_FormClosing(object sender, FormClosingEventArgs e)
        {
            timerUpdateList.Enabled = false;
        }

        private void RolodexHud_Resize(object sender, EventArgs e)
        {
            UpdateCharacterView();
        }

        private void txtFilter_TextChanged(object sender, EventArgs e)
        {
            _filter = txtFilter.Text.Trim().ToLower();
            _listIsDirty = true;
            UpdateCharacterView();
        }

        private void txtFilter_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                txtFilter.Text = "";
            }
        }

        private async void btnAdd_Click(object sender, EventArgs e)
        {
            btnAdd.Enabled = false;
            await AddCharacterAsync(_lastServer, txtAdhoc.Text.Trim());
            btnAdd.Enabled = true;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            lock (_cached)
            {
                _cached.Clear();
            }
            txtAdhoc.Clear();
        }

        private void txtAdhoc_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnAdd.PerformClick();
            }
        }

        private void timerUpdateList_Tick(object sender, EventArgs e)
        {
            UpdateCharacterView();
        }

        private void chkParse_CheckedChanged(object sender, EventArgs e)
        {
            timerUpdateList.Enabled = chkParse.Enabled;
        }
    }
}
