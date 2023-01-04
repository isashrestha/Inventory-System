using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory_Management_System.Data
{
    public class Item
    {

        public Guid Id { get; set; } = Guid.NewGuid();

        [Required(ErrorMessage = "Please provide the task name.")]
        public string ItemName { get; set; }

        [Required(ErrorMessage = "Please provide a due date.")]
        public DateTime AddedDate { get; set; } = DateTime.Today;
        public Guid AddedBy { get; set; }
        public int Price { get; set; }
        public int Quantity { get; set; }



    }
}
