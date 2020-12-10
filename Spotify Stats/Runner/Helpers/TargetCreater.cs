using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using System.Text.Json;
using System.Text.Json.Serialization;
using Spotify_Stats.Interfaces;

namespace Spotify_Stats.Runner.Helpers
{
    class TargetCreater
    {
        public static void Create(String token)
        {
            var toJSONString = new TokenFileType();
            toJSONString.token = token;
            toJSONString.updatedAt = DateTime.UtcNow;
            var json = JsonSerializer.Serialize(toJSONString);
            File.WriteAllText($@"{Path.GetDirectoryName(Application.ExecutablePath)}/Token.json",json);
        }
    }
}
