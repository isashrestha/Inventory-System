using System.Linq;
using System.Text.Json;

namespace Inventory_Management_System.Data;

public static class InventoryService
{
    public static void SaveAll(List<Item> inventory)
    {
        string appDataDirectoryPath = Utils.GetAppDirectoryPath();
        string InventoryFilePath = Utils.GetAppInventoryFilePath();

        if (!Directory.Exists(appDataDirectoryPath))
        {
            Directory.CreateDirectory(appDataDirectoryPath);
        }

        var json = JsonSerializer.Serialize(inventory);
        File.WriteAllText(InventoryFilePath, json);
    }

    public static List<Item> GetAll()
    {
        string inventoryFilePath = Utils.GetAppInventoryFilePath();
        if (!File.Exists(inventoryFilePath))
        {
            return new List<Item>();
        }

        var json = File.ReadAllText(inventoryFilePath);

        return JsonSerializer.Deserialize<List<Item>>(json);
    }

    public static List<Item> Create(string itemName, int quantity, Guid createdBy, int price)
    {


        List<Item> inventory = GetAll();
        inventory.Add(new Item
        {
            ItemName = itemName,
            Quantity = quantity,
            AddedBy = createdBy,
            Price = price
        });
        SaveAll(inventory);
        return inventory;
    }

    public static List<Item> Delete(Guid userId, Guid id)
    {
        List<Item> inventory = GetAll();
        Item item = inventory.FirstOrDefault(x => x.Id == id);

        if (item == null)
        {
            throw new Exception("Item not found.");
        }

        inventory.Remove(item);
        SaveAll(inventory);
        return inventory;
    }

    public static void DeleteByUserId(Guid userId)
    {
        string InventoryFilePath = Utils.GetAppInventoryFilePath();
        if (File.Exists(InventoryFilePath))
        {
            File.Delete(InventoryFilePath);
        }
    }

    public static List<Item> Update(Guid userId, Guid itemId, string itemName, int qty, int price)
    {
        List<Item> inventory = GetAll();
        Item itemToUpdate = inventory.FirstOrDefault(x => x.Id == itemId);


        itemToUpdate.ItemName = itemName;
        itemToUpdate.Quantity = qty;
        itemToUpdate.Price = price;
        SaveAll(inventory);
        return inventory;
    }
    

}