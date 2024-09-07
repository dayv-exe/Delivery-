using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
  public static void writeData()
  {
    BinaryFormatter formatter = new BinaryFormatter();
    FileStream stream = new FileStream(cache.gameDataDir(), FileMode.Create);
    PlayerData data = new PlayerData();
    formatter.Serialize(stream, data);
    stream.Close();
  }

  public static PlayerData readData()
  {
    if (File.Exists(cache.gameDataDir()))
    {
      try
      {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(cache.gameDataDir(), FileMode.Open);
        PlayerData data = formatter.Deserialize(stream) as PlayerData;
        stream.Close();
        return data;
      }
      catch (IOException)
      {
        Debug.Log("no saved data found!");
        File.Delete(cache.gameDataDir());
        cache.loadGameData();
        return null;
      }
    }
    else
    {
      Debug.Log("no saved data found!");
      return null;
    }
  }
}
