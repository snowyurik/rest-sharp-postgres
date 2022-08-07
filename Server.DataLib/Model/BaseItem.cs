using System;
using System.Linq;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Server.DataLib.Model {

    public class BaseItem : IItem  {
        [Key]
        public int Id { get; set; } // all Items have Id as key

        public override string ToString() {
            return JsonConvert.SerializeObject( this );
        }
    }
}