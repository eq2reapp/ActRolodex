using System;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Advanced_Combat_Tracker;

namespace ACT_Plugin
{
    public class Rolodex : IActPluginV1
    {
        private Label _lblPluginPage = null;
        private TabPage _tabPluginPage = null;
        private readonly Regex _channelRegex= new Regex(@".*\] \\aPC [\-0-9]+ .* (tells|says to the) .*,");
        private readonly RolodexHud _hud = new RolodexHud();
        private string _server = "";
        private string _pluginDir = "";
        private readonly RolodexSettings _settings = new RolodexSettings();

        public Rolodex()
        {
            AppDomain currentDomain = AppDomain.CurrentDomain;
            currentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
        }

        private Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            if (args.Name.StartsWith("Newtonsoft.Json"))
            {
                return Assembly.LoadFrom($"{_pluginDir}\\Newtonsoft.Json.dll");
            }
            return null;
        }

        void IActPluginV1.DeInitPlugin()
        {
            ActGlobals.oFormActMain.OnLogLineRead -= OFormActMain_OnLogLineRead;
            ActGlobals.oFormActMain.LogFileChanged -= OFormActMain_LogFileChanged;

            _settings.WindowLocationX = _hud.Location.X;
            _settings.WindowLocationY = _hud.Location.Y;
            _settings.WindowWidth = _hud.Width;
            _settings.WindowHeight = _hud.Height;
            _settings.SaveSettings();

            _hud.Close();
            _lblPluginPage.Text = "Plugin Stopped";
        }

        void IActPluginV1.InitPlugin(TabPage pluginScreenSpace, Label pluginStatusText)
        {
            _lblPluginPage = pluginStatusText;
            _lblPluginPage.Text = "Plugin Started";
            _tabPluginPage = pluginScreenSpace;

            // Since ACT loads this plugin from its own dir, and we don't have all our
            // dependencies in the GAC, we'll need to register a method to locate our
            // dependencies.
            // See: https://github.com/EQAditu/AdvancedCombatTracker/wiki/Plugin-Creation-Tips#adding-references-not-included-by-act
            var thisPlugin = ActGlobals.oFormActMain.ActPlugins.Find((plugin) => plugin.lblPluginStatus == pluginStatusText);
            _pluginDir = thisPlugin.pluginFile.DirectoryName;

            DetectServer();
            _settings.LoadSettings();

            ActGlobals.oFormActMain.OnLogLineRead += OFormActMain_OnLogLineRead;
            ActGlobals.oFormActMain.LogFileChanged += OFormActMain_LogFileChanged;

            _hud.Show(ActGlobals.oFormActMain);
            _hud.SetDesktopLocation(_settings.WindowLocationX, _settings.WindowLocationY);
            _hud.Width = _settings.WindowWidth;
            _hud.Height = _settings.WindowHeight;
        }

        private void DetectServer()
        {
            _server = Directory.GetParent(ActGlobals.oFormActMain.LogFilePath).Name;
        }

        private void OFormActMain_LogFileChanged(bool IsImport, string NewLogFileName)
        {
            DetectServer();
        }

        private void OFormActMain_OnLogLineRead(bool isImport, LogLineEventArgs logInfo)
        {
            if (!isImport && _hud.chkParse.Checked)
            {
                if (_channelRegex.IsMatch(logInfo.logLine))
                {
                    var name = GetPlayerFromChatMessage(logInfo.logLine);
                    if (name != null)
                    {
                        _hud.QueueCharacter(_server, name);
                    }
                }
            }
        }

        private string GetPlayerFromChatMessage(string logLine)
        {
            /* Examples:
                (1625867016)[Fri Jul  9 17:43:36 2021] \aPC -1 Player1:Player1\/a tells you, "Blah"
                (1625190537)[Thu Jul  1 21:48:57 2021] \aPC -1 Player2:Player2\/a tells General (3), "Blah blah"
            */
            var str = logLine;
            var idx = str.IndexOf(']');            //^
            if (idx >= 0)
            {
                str = str.Substring(idx + 1);
                idx = str.IndexOf(':');                             //^
                if (idx >= 0)
                {
                    str = str.Substring(idx + 1);
                    idx = str.IndexOf('\\');                                //^
                    if (idx >= 0)
                    {
                        return str.Substring(0, idx);
                    }
                }
            }
            return null;
        }

    }
}
