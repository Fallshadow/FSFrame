using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

// 存档编号和时间 
// 一个存档下面对应的是一个文件夹下的所有 object
// 在删除存档的时候，应该要删除所有对应 ID 缓存
[Serializable]
public class SaveItem
{
    public int SaveID { get; private set; }
    public DateTime LastSaveTime  { get; private set; }

    public SaveItem(int saveID, DateTime lastSaveTime)
    {
        this.SaveID = saveID;
        this.LastSaveTime = lastSaveTime;
    }

    public void UpdateTime(DateTime lastSaveTime)
    {
        this.LastSaveTime = lastSaveTime;
    }
}

public static partial class SaveManager
{
    private const string saveDirName = "saveData";
    private const string settingDirName = "setting";

    private static readonly string saveDirpath;
    private static readonly string settingDirPath;
    
    static SaveManager()
    {
        saveDirpath = Application.persistentDataPath + "/" + saveDirName;
        settingDirPath = Application.persistentDataPath + "/" + settingDirName;

        if (Directory.Exists(saveDirpath) == false)
        {
            Directory.CreateDirectory(saveDirpath);
        }

        if (Directory.Exists(settingDirPath) == false)
        {
            Directory.CreateDirectory(settingDirPath);
        }

        InitSaveManagerData();
    }

    #region 全局设置

    public static void SaveGlobalSetting(object saveObject, string fileName)
    {
        SaveObjectToFile(saveObject, settingDirPath + "/" + fileName);
    }
    
    public static void SaveGlobalSetting(object saveObject)
    {
        SaveGlobalSetting(saveObject, saveObject.GetType().Name);
    }

    public static T LoadGlobalSetting<T>(string fileName) where T : class
    {
        return LoadObjectByFile<T>(settingDirPath + "/" + fileName);
    }
    
    public static T LoadGlobalSetting<T>() where T : class
    {
        return LoadGlobalSetting<T>(typeof(T).Name);
    }

    #endregion


    #region 关于缓存
    
    // 缓存存档中的对象
    private static Dictionary<int, Dictionary<string, object>> cacheDic =
        new Dictionary<int, Dictionary<string, object>>();
    
    private static void SetCache(int saveID, string fileName, object saveObject)
    {
        if (cacheDic.TryGetValue(saveID, out Dictionary<string, object> saveDic))
        {
            if (saveDic.ContainsKey(fileName))
            {
                cacheDic[saveID][fileName] = saveObject;
            }
            else
            {
                cacheDic[saveID].Add(fileName, saveObject);
            }
        }
        else
        {
            cacheDic.Add(saveID, new Dictionary<string, object>(){{fileName, saveObject}});
        }
    }

    private static T GetCache<T>(int saveID, string fileName) where T : class
    {
        if (cacheDic.TryGetValue(saveID, out Dictionary<string, object> saveDic))
        {
            if (saveDic.ContainsKey(fileName))
            {
                return cacheDic[saveID][fileName] as T;
            }
            else
            {
                return null;
            }
        }
        else
        {
            return null;
        }
    }

    private static void RemoveCache(int saveID)
    {
        cacheDic.Remove(saveID);
    }

    #endregion
    
    #region 关于对象
    
    // 使用对象名称直接保存到指定 ID 存档
    public static void SaveObjectToSaveItem(object saveObject, int saveID = 0)
    {
        SaveObjectToSaveItem(saveObject, saveObject.GetType().Name, saveID);
    }
    
    public static void SaveObjectToSaveItem(object saveObject, SaveItem saveItem)
    {
        SaveObjectToSaveItem(saveObject, saveObject.GetType().Name, saveItem);
    }

    // 保存对象到 ID 存档并起名
    public static void SaveObjectToSaveItem(object saveObject, string saveFileName, int saveID = 0)
    {
        string dirPath = GetSaveItemSavePath(saveID, true);
        string savePath = dirPath + "/" + saveFileName;
        SaveObjectToFile(saveObject, savePath);
        
        GetSaveItem(saveID).UpdateTime(DateTime.Now);                                                   
        SetCache(saveID, saveFileName, saveObject);
        SaveSaveMangerData();
    }
    
    public static void SaveObjectToSaveItem(object saveObject, string saveFileName, SaveItem saveItem)
    {
        string dirPath = GetSaveItemSavePath(saveItem.SaveID, true);
        string savePath = dirPath + "/" + saveFileName;
        SaveObjectToFile(saveObject, savePath);
        
        saveItem.UpdateTime(DateTime.Now);                                                   
        SetCache(saveItem.SaveID, saveFileName, saveObject);
        SaveSaveMangerData();
    }

    // 根据 ID 和 对象名 加载对象
    public static T LoadObjectBySaveItem<T>(int saveID = 0) where T : class
    {
        return LoadObjectBySaveItem<T>(typeof(T).Name, saveID);
    }
    
    public static T LoadObjectBySaveItem<T>(SaveItem saveItem) where T : class
    {
        return LoadObjectBySaveItem<T>(typeof(T).Name, saveItem);
    }
    
    // 根据 ID 和 名称 加载对象
    public static T LoadObjectBySaveItem<T>(string saveFileName, int saveID = 0) where T : class
    {
        T obj = GetCache<T>(saveID, saveFileName);
        if (obj != null) return obj;
        
        string dirPath = GetSaveItemSavePath(saveID);
        if(dirPath == null) return null;
        string savePath = dirPath + "/" + saveFileName;
        obj = LoadObjectByFile<T>(savePath);
        // 如果本地有存档，然后一开始游戏就加载，这个时候拿出来也要缓存下
        SetCache(saveID, saveFileName, obj);
        return obj;
    }
    
    public static T LoadObjectBySaveItem<T>(string saveFileName, SaveItem saveItem) where T : class
    {
        return LoadObjectBySaveItem<T>(saveFileName, saveItem.SaveID);
    }

    #endregion
    
    #region 工具函数
    
    private static BinaryFormatter binaryFormatter = new BinaryFormatter();

    private static string GetSaveItemSavePath(int saveID, bool createDir = true)
    {
        if (GetSaveItem(saveID) == null) throw new Exception("JK:saveID 存档不存在");
        
        string saveDir = saveDirpath + "/" + saveID;
        // 确定文件夹是否存在
        if (!Directory.Exists(saveDir))
        {
            if (createDir)
            {
                Directory.CreateDirectory(saveDir);
            }
            else
            {
                return null;
            }
        }

        return saveDir;
    }

    private static void SaveObjectToFile(object saveObject, string filePath)
    {
        using FileStream f = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None);
        binaryFormatter.Serialize(f, saveObject);
    }

    private static T LoadObjectByFile<T>(string filePath) where T : class
    {
        if (!File.Exists(filePath)) return null;
        FileStream f = new FileStream(filePath, FileMode.Open);
        try
        {
            // 将内容解码成对象
            T saveObject = (T)binaryFormatter.Deserialize(f);
            f.Dispose();
            return saveObject;
        }
        catch (Exception e)
        {
            Debug.LogException(e);
            return null;
        }
    }

    #endregion
}
