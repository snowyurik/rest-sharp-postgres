using System;
using System.Linq;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Server.Model {

    public class Book {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
    }
}