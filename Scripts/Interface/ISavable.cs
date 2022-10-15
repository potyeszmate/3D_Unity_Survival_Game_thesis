using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISavable
{
    // Data to save
    string GetJsonDataToSave();
    // Data to load
    void LoadJsonData(string jsonData);
}
