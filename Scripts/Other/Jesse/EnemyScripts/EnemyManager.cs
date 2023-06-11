using UnityEngine;

public class PlayerFinder
{
    // a class for enemies to detect the player
    // �G�Ƀv���C���[���擾������N���X


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
