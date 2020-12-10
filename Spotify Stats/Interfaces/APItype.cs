using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spotify_Stats.Interfaces
{
    public class APItype
    {
        public songmeta[] items { get; set; }
    }
    public class songmeta
    {
        public album album {get;set;}
        public string name { get; set; }
    }
    public class imagemeta
    {
        public string url { get; set; }
    }
    public class album
    {
        public imagemeta[] images { get; set; }
        public artistmeta[] artists { get; set; }
    }
    public class artistmeta
    {
        public string name { get; set; }
    }
    public class refreshtokentype
    {
        public string grant_type { get; set; }
        public string code { get; set; }
        public string redirect_uri { get; set; }
    }
}
