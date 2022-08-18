using System;
using System.Linq;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Server.DataLib.Attributes;

namespace Server.DataLib.Model {

    [RestCollection]
    public class Shelf : BaseItem {
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
    }
}