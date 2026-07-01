using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Rendering;
using static UnityEngine.Rendering.DebugUI;



public class SaveManager : MonoBehaviour
{
    [System.Serializable]
    public class SaveDataObject
    {
        public SerializedDictionary<string, int> ints = new SerializedDictionary<string, int>();
        public SerializedDictionary<string, float> floats = new SerializedDictionary<string, float>();
        public SerializedDictionary<string, string> strings = new SerializedDictionary<string, string>();
        public SerializedDictionary<string, Vector3> vectors = new SerializedDictionary<string, Vector3>();
    }
    [SerializeField] SaveDataObject data;
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
    /// Load the current stored data from the JSON file.
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
        System.Type type = typeof(T);

        if (type == typeof(int))
        {
            if (data.ints.TryGetValue(name, out int obj)) { value = (T)(object)obj; return true; }
        }
        if (type == typeof(string))
        {
            if (data.strings.TryGetValue(name, out string obj)) { value = (T)(object)obj; return true; }
        }
        if (type == typeof(float))
        {
            if (data.floats.TryGetValue(name, out float obj)) { value = (T)(object)obj; return true; }
        }
        if(type == typeof(Vector3))
        {
            if (data.vectors.TryGetValue(name, out Vector3 obj)) { value = (T)(object)obj; return true; }
        }
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

        System.Type type = typeof(T);

        if (type == typeof(int))
        {
            if (data.ints.ContainsKey(name))
            {
                data.ints[name] = (int)(object)newData;
                Debug.Log("updated!");
            }
            else
            {
                data.ints.Add(name, (int)(object)newData);
                Debug.Log("saved!");
                Debug.Log(data.ints[name]);
            }
        }
        if (type == typeof(string))
        {
            if (data.strings.ContainsKey(name))
            {
                data.strings[name] = (string)(object)newData;
                Debug.Log("updated!");
            }
            else
            {
                data.strings.Add(name, (string)(object)newData);
                Debug.Log("saved!");
                Debug.Log(data.strings[name]);
            }
        }
        if (type == typeof(float))
        {
            if (data.floats.ContainsKey(name))
            {
                data.floats[name] = (float)(object)newData;
                Debug.Log("updated!");
                Debug.Log(newData);
                Debug.Log(data.floats[name]);

            }
            else
            {
                data.floats.Add(name, (float)(object)newData);
                Debug.Log("saved!");
                Debug.Log(data.floats[name]);
            }
        }
        if (type == typeof(Vector3))
        {
            if (data.vectors.ContainsKey(name))
            {
                data.vectors[name] = (Vector3)(object)newData;
                Debug.Log("updated!");
            }
            else
            {
                data.vectors.Add(name, (Vector3)(object)newData);
                Debug.Log("saved!");
                Debug.Log(data.floats[name]);
            }
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
    public void RemoveData<T>(string name, bool save = true)
    {
        System.Type type = typeof(T);

        if (type == typeof(int))
        {
            if (data.ints.ContainsKey(name))
            {
                data.ints.Remove(name);
            }
        }
        if (type == typeof(string))
        {
            if (data.strings.ContainsKey(name))
            {
                data.strings.Remove(name);
            }
        }
        if (type == typeof(float))
        {
            if (data.floats.ContainsKey(name))
            {
                data.floats.Remove(name);
            }
        }
        if (type == typeof(Vector3))
        {
            if (data.vectors.ContainsKey(name))
            {
                data.vectors.Remove(name);
            }
        }


        //save data by default
        if (save)
        {
            SaveData();
        }
    }
}
