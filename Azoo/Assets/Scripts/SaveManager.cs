using System.Collections.Generic;
using System.Text;

public class SaveManager : ConsistantSingleton<SaveManager>
{

        public Dictionary<string, int> SavedData;
        public StringBuilder Logs;

        [System.Serializable]
        public struct AwakeValue
        {
                public string key;
                public int value;
        }

        public List<AwakeValue> AwakeValues = new();

        protected override void Awake()
        {
                base.Awake();
                Logs = new();
                SavedData = new();
                foreach (AwakeValue value in AwakeValues)
                {
                        if (SavedData.ContainsKey(value.key))
                                SavedData[value.key] = value.value;
                        else
                                SavedData.Add(value.key, value.value);
                }
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