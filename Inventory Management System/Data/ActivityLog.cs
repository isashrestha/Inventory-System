using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory_Management_System.Data;

public class ActivityLog
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid OrderedItem { get; set; }
    public DateTime ApprovedDate { get; set; } = DateTime.Today;
    public Guid AddedBy { get; set; }
    public Guid ApprovedBy { get; set; }
    public bool ApprovalStatus { get; set; }
    public int QuantityRequested { get; set; }

}
