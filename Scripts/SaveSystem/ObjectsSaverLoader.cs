using System.IO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Newtonsoft.Json;

public static class ObjectsSaverLoader
{
    
  // Saving and loading the objects in map

  //path
  public static string path = Application.persistentDataPath + "/savedObjectsInMap.json";

  // Saves the objects to the path in a .json file
  public static void SaveGameObjectInMap(Objects objects)
  {
      
      File.WriteAllText(path, JsonUtility.ToJson(objects));

  }

  // loads the objects from json file
  public static Objects LoadGameObjectInMap(string objectName)
  {
   
      Objects objects = JsonUtility.FromJson<Objects>(System.IO.File.ReadAllText(path));
      return objects;
      
  }

 
  

}
