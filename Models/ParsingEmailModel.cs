using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AngParser.Models
{
    public class ParsingEmailModel
    {
        public bool Sended { get; set; }

        public string Email { get; set; }

        public Uri HtmlAdres { get; set; }
    }
}
