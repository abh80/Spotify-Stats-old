using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using System.Text.Json;
using Spotify_Stats.Interfaces;

namespace Spotify_Stats.Runner.Helpers
{
    class ParseTarget
    {
        public static TokenFileType Parse() {
        if (!File.Exists($@"{Path.GetDirectoryName(Application.ExecutablePath)}/Token.json")) {
                return null;
        }
            String json =  File.ReadAllText($@"{Path.GetDirectoryName(Application.ExecutablePath)}/Token.json");
            return JsonSerializer.Deserialize<TokenFileType>(json);
        }
    }
}
