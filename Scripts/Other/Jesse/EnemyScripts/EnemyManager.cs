using UnityEngine;

public class PlayerFinder
{
    // a class for enemies to detect the player
    // 敵にプレイヤーを取得させるクラス


    static GameObject _target;
    GameObject player;
    void Init()
    {
        _target = GameObject.Find("Player");
    }

    public GameObject GetPlayer()
	{
        return player;
	}

    public PlayerFinder()
	{
        if(_target == null)
		{
            Init();
		}
        player = _target;
	}
}
