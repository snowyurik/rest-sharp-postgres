using System;
using System.Linq;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Server.DataLib.Attributes;

namespace Server.DataLib.Model {

    [RestCollection]
    public class Book : BaseItem {
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public bool Read { get; set; } = false;
    }
}