using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;

public static class SaveSystem {

	public static List<Game> savedGames = new List<Game>();

	[SerializeField] private static string dirpath = Environment.GetFolderPath (Environment.SpecialFolder.ApplicationData) + "/ArenaTale/SaveData";
	[SerializeField] private static string filepath = "/savedata.at";

	//it's static so we can call it from anywhere
	public static void Save() {
		if (!Directory.Exists (dirpath))
			Directory.CreateDirectory (dirpath);
		SaveSystem.savedGames.Add(Game.current);
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create (dirpath+filepath);
		bf.Serialize(file, SaveSystem.savedGames);
		file.Close();
	}	

	public static void Load() {
		if(File.Exists(dirpath+filepath)) {
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(dirpath+filepath, FileMode.Open);
			SaveSystem.savedGames = (List<Game>)bf.Deserialize(file);
			file.Close();
			Debug.Log ("Finished loading");
		}
	}
}
