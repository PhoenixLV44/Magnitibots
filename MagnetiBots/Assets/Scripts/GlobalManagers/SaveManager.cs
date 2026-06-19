using System.Collections.Generic;
using System.IO;
using UnityEngine;



public class SaveManager : MonoBehaviour
{
    [System.Serializable]
    public class SaveDataObject
    {
        public Dictionary<string, object> data = new Dictionary<string, object>();
    }
    SaveDataObject data;
    string json;
    string filePath;
    private void Awake()
    {
        filePath = Path.Combine(Application.persistentDataPath, "SaveData.json");
        if(File.Exists(filePath))
        {
            json = File.ReadAllText(filePath);
            data = JsonUtility.FromJson<SaveDataObject>(json);
        }
        else
        {
            data = new SaveDataObject();
            json = JsonUtility.ToJson(data);
            File.WriteAllText(filePath, json);
        }
    }
    public void LateAwake()
    {

    }
    /// <summary>
    /// Save the current stored data to the JSON file.
    /// </summary>
    public void SaveData()
    {
        json = JsonUtility.ToJson(data);
        File.WriteAllText(filePath, json);
    }
    /// <summary>
    /// Load the current stored data to the JSON file.
    /// </summary>
    public void LoadData() 
    {
        json = File.ReadAllText(filePath);
        JsonUtility.FromJsonOverwrite(json, data);
    }
    /// <summary>
    /// Get data of any datatype from the data object.
    /// </summary>
    /// <param name="name">The ID that was given to the data when it was added.</param>
    /// <returns>Anythign that is found attached to the ID in the data object.</returns>
    public bool GetData<T>(string name, out T value)
    {
        if(data.data.TryGetValue(name, out object obj)) {  value = (T)obj; return true; }
        Debug.Log("failed to find " + name);
        value = default(T);
        return false;
    }
    /// <summary>
    /// Add or update data to the data object.
    /// </summary>
    /// <param name="name">An ID that will identify the data within the data object.</param>
    /// <param name="data">The data to be added to the data object.</param>
    /// <param name="save">Whether to save the data after it is added. Defaults to true.</param>
    public void AddData<T>(string name, T newData, bool save = true)
    {
        //check for update vs create
        if (data.data.ContainsKey(name))
        {
            data.data[name] = newData;
            Debug.Log("updated!");
        }
        else
        {
            data.data.Add(name, newData);
            Debug.Log("saved!");
            Debug.Log(data.data[name]);
        }

        //save data by default
        if (save)
        {
            SaveData();
        }
    }
    /// <summary>
    /// Remove data from the data object.
    /// </summary>
    /// <param name="name">The ID that was assigned to the data.</param>
    /// <param name="save">Whether to save the data object after data is removed. Defaults to true.</param>
    public void RemoveData(string name, bool save = true)
    {
        if (data.data.ContainsKey(name))
        {
            data.data.Remove(name);
        }

        //save data by default
        if (save)
        {
            SaveData();
        }
    }
}
