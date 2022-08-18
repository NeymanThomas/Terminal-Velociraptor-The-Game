using System.IO;
using UnityEngine;
using System;

public static class SaveSystem
{
    // the file path where the stats are saved
    private static string savePath = Application.persistentDataPath + "/save.json";
    // the file path where User Preferences are saved
    private static string prefsPath = Application.persistentDataPath + "/userPrefs.json";

    #region SaveFile

    // This function creates a new SaveFile object and writes it to the filepath
    public static void CreateSaveFile() 
    {
        // Create a brand new SaveFile Object
        SaveFile stats = new SaveFile();
        // convert the stats string into Json string
        string json = JsonUtility.ToJson(stats);
        // Write the json string into the file
        File.WriteAllText(savePath, json);
    }

    // Return true if there is a save file in the directory, false otherwise
    public static bool DoesSaveExist() 
    {
        if (File.Exists(savePath))
            return true;
        return false;
    }

    // When Saving the game, this will update the current savefile
    public static void UpdateSaveFile(SaveFile newSave) 
    {
        if (!File.Exists(savePath)) 
        {
            CreateSaveFile();
        }
        SaveFile loadedSave = LoadSave();
        // Do the updating here
        string json = JsonUtility.ToJson(loadedSave);
        // Write the json string back into the file
        File.WriteAllText(savePath, json);
    }

    // returns the SaveFile object in the filepath
    public static SaveFile LoadSave() 
    {
        // read the file in as a string from the path
        string save = File.ReadAllText(savePath);
        // convert the string into SaveFile from json
        SaveFile loadedSave = JsonUtility.FromJson<SaveFile>(save);
        return loadedSave;
    }

    // Deletes the save file in the path and replaces it with a new one
    public static void DeleteSaveFile() {
        try {
            File.Delete(savePath);
            CreateSaveFile();
        }
        catch (Exception ex) {
            Debug.LogException(ex);
        }
    }

    #endregion
    #region Preferences

    public static void CreatePrefsFile() 
    {
        // Create a brand new UserPreferences object
        UserPreferences prefs = new UserPreferences();
        // convert the prefs string into Json string
        string json = JsonUtility.ToJson(prefs);
        // Write the json string into the file
        File.WriteAllText(prefsPath, json);
    }

    public static bool DoPrefsExist() 
    {
        if (File.Exists(prefsPath))
            return true;
        return false;
    }

    #endregion
}
