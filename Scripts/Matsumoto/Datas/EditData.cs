using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public class PlayerDataClass
{
	public List<bool> magicFlag = new List<bool>();

	public List<bool> passiveFlag = new List<bool>();

	public Vector3 savePosition;

	public Vector3 saveLowerLeftCamera;

	public string saveScene;
	public int savePlace;
}
public class EditData : MonoBehaviour
{

	[SerializeField]
	private MagicList magicList;
	public PlayerDataClass data;
	int countMagicBool;
	int countPassiveBool;

	string datapath;//データ保存先
	private void Awake()
	{
		datapath = Application.dataPath + "/SaveData.json";//datapathに保存先を指定
		countMagicBool = data.magicFlag.Count;
		countPassiveBool = data.passiveFlag.Count;
		SetPlayerData(Load());
	}


	private void Start()
	{

	}

	private void SetPlayerData(PlayerDataClass _playerData)
	{
		for(int i = 0; i < countMagicBool; i++)
		{
			data.magicFlag[i] = _playerData.magicFlag[i];
		}
		for(int i = 0; i < countPassiveBool; i++)
		{
			data.passiveFlag[i] = _playerData.passiveFlag[i];
		}
		data.savePosition = _playerData.savePosition;
		data.saveLowerLeftCamera = _playerData.saveLowerLeftCamera;
		data.saveScene = _playerData.saveScene;
		data.savePlace = _playerData.savePlace;
	}

	public void Save(PlayerDataClass _playerData)
	{
		string jsonstr = JsonUtility.ToJson(_playerData);
		StreamWriter writer = new StreamWriter(datapath, false);
		writer.WriteLine(jsonstr);
		writer.Flush();
		writer.Close();
	}

	public void NewGame()//メモ程度に残しとく
	{
		File.Delete(datapath);
	}

	private PlayerDataClass Load()
	{
		if(File.Exists(datapath))//"datapath"内のファイルが存在するなら
		{
			Debug.Log("EditData: SaveData.json データ読み込み");
			StreamReader reader = new StreamReader(datapath);
			string datastr = reader.ReadToEnd();
			reader.Close();
			return JsonUtility.FromJson<PlayerDataClass>(datastr);
		}
		else//存在しないなら
		{
			Debug.Log("EditData: SaveData.json データ新規作成");
			PlayerDataClass _playerData = new PlayerDataClass();
			countMagicBool = magicList.GetMagicCount();

			_playerData.magicFlag.Add(true);
			for(int i = 1; i < countMagicBool; i++)
			{
				_playerData.magicFlag.Add(false);
			}
			for(int i = 0; i < countPassiveBool; i++)
			{
				_playerData.passiveFlag.Add(false);
			}
			_playerData.savePosition = new Vector3(-30f, -9.9f, 0);
			_playerData.saveLowerLeftCamera = new Vector3(-55.2f, -54.4f, 0);
			_playerData.saveScene = "";
			_playerData.savePlace = 0;
			data = _playerData;
			Save(_playerData);
			//string jsonstr = JsonUtility.ToJson(playerData);
			//StreamWriter writer = new StreamWriter(datapath, false);
			//writer.WriteLine(jsonstr);
			//writer.Flush();
			//writer.Close();

			return _playerData;

		}

	}
	public bool dashAttackFlag
	{
		get
		{
		return data.passiveFlag[0];
		}
		set
		{
			data.passiveFlag[0] = value;
		}
	}
	public bool jumpAttackFlag
	{
		get
		{
			return data.passiveFlag[1];
		}
		set
		{
			data.passiveFlag[1] = value;
		}
	}
	public bool jumpMagicFlag
	{
		get
		{
			return data.passiveFlag[2];
		}
		set
		{
			data.passiveFlag[2] = value;
		}
	}
}
