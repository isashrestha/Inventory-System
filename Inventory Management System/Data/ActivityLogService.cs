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
    public static List<ActivityLog> Order(Guid Id, DateTime ApprovedDate, Guid AddedBy, Guid ApprovedBy, Guid ApprovalStatus)
    {


        List<ActivityLog> activitylog = GetAll();
        activitylog.Add(new ActivityLog
        {
            ApprovedDate = ApprovedDate,
            AddedBy = AddedBy,
            ApprovedBy = ApprovedBy,
            ApprovalStatus = ApprovalStatus,
        });
        SaveAll(activitylog);
        return activitylog;
    }

}

