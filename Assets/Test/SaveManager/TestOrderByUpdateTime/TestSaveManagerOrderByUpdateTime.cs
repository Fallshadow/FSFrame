using System;
using System.Collections.Generic;
using UnityEngine;

public class TestSaveManagerOrderByUpdateTime : MonoBehaviour
{
        [Serializable]
        private class TestSaveData
        {
                public string StrName = "lisi";
        }
        private void Start()
        {
                // for (int i = 0; i < 10; i++)
                // {
                //         SaveManager.CreateSaveItem();
                // }
                for (int i = 0; i < 10; i++)
                {
                        SaveManager.SaveObjectToSaveItem(new TestSaveData() {StrName = "张三"}, i);
                }
                SaveManager.SaveObjectToSaveItem(new TestSaveData() {StrName = "张三"}, 5);
                List<SaveItem> saveItems = SaveManager.GetSaveItemListByUpdateTime();
                foreach (SaveItem saveItem in saveItems)
                {
                        Debug.Log(saveItem.SaveID);
                }
        }

        private void TestSave()
        {
                List<SaveItem> SaveItems = SaveManager.GetAllSaveItemList(saveItem =>
                {
                        TestSaveData saveData = SaveManager.LoadObjectBySaveItem<TestSaveData>(saveItem);
                        if (saveData == null) return "";
                        return saveData.StrName;
                });

                foreach (SaveItem saveItem in SaveItems)
                {
                        Debug.Log(saveItem.SaveID);
                }
        }
}