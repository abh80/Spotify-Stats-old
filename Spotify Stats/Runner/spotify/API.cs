using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Windows.Forms;
using Spotify_Stats.Interfaces;

namespace Spotify_Stats.Runner.spotify
{
    class API
    {
        public static async Task<APItype> GetTopSongs(String token)
        {
            var client = new HttpClient();
            var auth = new AuthenticationHeaderValue("Bearer",token);
            client.DefaultRequestHeaders.Authorization = auth;
            var res = await client.GetStringAsync("https://api.spotify.com/v1/me/top/tracks");
            try
            {
                return JsonSerializer.Deserialize<APItype>(res);
            }
            catch {
                MessageBox.Show("Something happened while getting your top songs!");
                return null;
            }
        }
    }
}
