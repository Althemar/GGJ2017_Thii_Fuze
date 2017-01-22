using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class HighScore : MonoBehaviour 
{
	public static HighScore hs;

	public int[] highScores = new int[10];

	void Awake()
	{
		Load();
		if(hs == null)
		{
			DontDestroyOnLoad(gameObject);
			hs = this;
		}
		else if(hs != this)
		{
			Destroy(gameObject);
		}
	}

	public void Save()
	{
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create(Application.persistentDataPath + "/highScores.dat");

		PlayerData data = new PlayerData();
		data.highScores = highScores;

		bf.Serialize(file, data);
		file.Close();
	}

	public void Load()
	{
		if(File.Exists(Application.persistentDataPath + "/highScores.dat"))
		{
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(Application.persistentDataPath + "/highScores.dat", FileMode.Open);

			PlayerData data = (PlayerData)bf.Deserialize(file);
			file.Close();

			highScores = data.highScores;
		}
	}

	public void NewScore(int lastScore)
	{
		for(int i = highScores.Length - 1; i >= 0; i--)
		{
			if(lastScore < highScores[i])
			{
				if(i < 9)
				{
					highScores[i + 1] = highScores[i];
				}
				highScores[i] = lastScore;
			}
		}
		Save();
	}
}

[Serializable]
class PlayerData
{
	public int[] highScores = new int[10];
}