using System.Collections.Generic;
using System.Text;

public class SaveManager : ConsistantSingleton<SaveManager>
{

        public Dictionary<string, int> SavedData;
        public StringBuilder Logs;

        protected override void Awake()
        {
                base.Awake();
                Logs = new();
                SavedData = new();
        }

        public int Get(string key)
        {
                if (SavedData.ContainsKey(key))
                        return SavedData[key];
                else
                        return 0;
        }

        public void Set(string key, int value)
        {
                if (SavedData.ContainsKey(key))
                        SavedData[key] = value;
                else
                        SavedData.Add(key, value);
        }

}