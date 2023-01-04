using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Inventory_Management_System.Data;

public static class ActivityLogService
{
    private static void SaveAll(List<ActivityLog> activitylog)
    {
        string appDataDirectoryPath = Utils.GetAppDirectoryPath();
        string ActivityLogFilePath = Utils.GetAppActivityLogFilePath();

        if (!Directory.Exists(appDataDirectoryPath))
        {
            Directory.CreateDirectory(appDataDirectoryPath);
        }

        var json = JsonSerializer.Serialize(activitylog);
        File.WriteAllText(ActivityLogFilePath, json);
    }

    public static List<ActivityLog> GetAll()
    {
        string activityFilePath = Utils.GetAppActivityLogFilePath();
        if (!File.Exists(activityFilePath))
        {
            return new List<ActivityLog>();
        }

        var json = File.ReadAllText(activityFilePath);

        return JsonSerializer.Deserialize<List<ActivityLog>>(json);
    }
    public static List<ActivityLog> Order(Guid Itemid, Guid AddedBy, int quantity)
    {


        List<ActivityLog> activitylog = GetAll();
        activitylog.Add(new ActivityLog
        {
            AddedBy = AddedBy,
            ApprovalStatus = false,
            OrderedItem = Itemid,
            QuantityRequested = quantity
        });
        SaveAll(activitylog);
        return activitylog;
    }
    public static List<ActivityLog> ApproveOrder(Guid id,Guid approvedBy)
    {


        List<ActivityLog> activitylog = GetAll();
        var items = InventoryService.GetAll();
        var order = activitylog.FirstOrDefault(x => x.Id == id);
        var item = items.FirstOrDefault(x => x.Id == order.OrderedItem);
        order.ApprovedBy = approvedBy;
        order.ApprovalStatus = true;
        order.ApprovedDate = DateTime.Now;
        item.Quantity = item.Quantity - order.QuantityRequested;


       
        SaveAll(activitylog);
        InventoryService.SaveAll(items);
        return activitylog;
    }

}

