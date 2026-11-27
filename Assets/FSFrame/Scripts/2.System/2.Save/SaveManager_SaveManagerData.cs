using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public static partial class SaveManager
{
    [Serializable]
    private class SaveManagerData
    {
        public int currID;

        // 存档没有很多所以无所谓了
        public List<SaveItem> saveItemList = new List<SaveItem>();
    }

    private static SaveManagerData saveManagerData = new SaveManagerData();

    private static void InitSaveManagerData()
    {
        saveManagerData = LoadFile<SaveManagerData>(saveDirpath + "/SaveManagerData");
        if (saveManagerData == null)
        {
            saveManagerData = new SaveManagerData();
            SaveSaveMangerData();
        }
    }

    public static void SaveSaveMangerData()
    {
        SaveFile(saveManagerData, saveDirpath + "/SaveManagerData");
    }

    public static SaveItem GetSaveItem(int saveID)
    {
        for (int i = 0; i < saveManagerData.saveItemList.Count; i++)
        {
            if (saveManagerData.saveItemList[i].SaveID == saveID)
            {
                return saveManagerData.saveItemList[i];
            }
        }

        return null;
    }

    public static SaveItem CreateSaveItem()
    {
        SaveItem saveItem = new SaveItem(saveManagerData.currID, DateTime.Now);
        saveManagerData.saveItemList.Add(saveItem);
        saveManagerData.currID++;
        SaveSaveMangerData();
        return saveItem;
    }

    public static void DeleteSaveItem(int saveID)
    {
        string saveDir = GetSavePath(saveID);
        // 如果路径存在 递归删除文件
        if (saveDir != null) Directory.Delete(saveDir, true);
        saveManagerData.saveItemList.Remove(GetSaveItem(saveID));
        RemoveCache(saveID);
        SaveSaveMangerData();
    }

    public static void DeleteSaveItem(SaveItem saveItem)
    {
        string saveDir = GetSavePath(saveItem.SaveID);
        // 如果路径存在 递归删除文件
        if (saveDir != null) Directory.Delete(saveDir, true);
        saveManagerData.saveItemList.Remove(saveItem);
        RemoveCache(saveItem.SaveID);
        SaveSaveMangerData();
    }

    public static List<SaveItem> GetSaveItemList()
    {
        return saveManagerData.saveItemList;
    }

    public static List<SaveItem> GetSaveItemListByCreateTime()
    {
        int count = saveManagerData.saveItemList.Count;
        List<SaveItem> saveItems = new List<SaveItem>(count);
        for (int i = 0; i < count; i++)
        {
            saveItems[i] = saveManagerData.saveItemList[count - i - 1];
        }

        return saveItems;
    }

    public static List<SaveItem> GetSaveItemListByUpdateTime()
    {
        int count = saveManagerData.saveItemList.Count;
        List<SaveItem> saveItems = new List<SaveItem>(count);
        for (int i = 0; i < count; i++)
        {
            saveItems.Add(saveManagerData.saveItemList[i]);
        }

        OrderByUpdateTimeComparer orderBy = new OrderByUpdateTimeComparer();
        saveItems.Sort(orderBy);
        return saveItems;
    }
    
    // 大的在前面，意味着新的在前面
    private class OrderByUpdateTimeComparer : IComparer<SaveItem>
    {
        public int Compare(SaveItem x, SaveItem y)
        {
            if (ReferenceEquals(x, y)) return 0;
            if (y is null) return -1;
            if (x is null) return 1;
            return -x.LastSaveTime.CompareTo(y.LastSaveTime);
        }
    }

    public static List<SaveItem> GetAllSaveItemList<T>(Func<SaveItem, T> orderFunc, bool isDescending = false)
    {
        if (isDescending)
        {
            return saveManagerData.saveItemList.OrderByDescending(orderFunc).ToList();
        }
        else
        {
            return saveManagerData.saveItemList.OrderBy(orderFunc).ToList();
        }
    }
}