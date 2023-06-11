using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicHeal : MonoBehaviour, IDamageMagic
{
    GameObject Player;
    PlayerController playerController;
    PlayerFinder playerFinder;
    CameraShakeManager camManager;
    CameraShake CamShake;

    bool enemyHit = false;
    bool healFlag = false;

    
    void Start()
    {
        camManager = new CameraShakeManager();
        CamShake = camManager.GetShakeScript();
        playerFinder = new PlayerFinder();
        Player = playerFinder.GetPlayer();
        playerController = Player.GetComponent<PlayerController>();
        StartCoroutine(Heal());
    }

    // if hit enemy and not healed yet, heal
    // ìGÇ∆ê⁄êGÇµÇΩéûÇ≈Ç‹ÇæâÒïúÇµÇƒÇ»Ç¢Ç∆Ç´ÅAâÒïúÇ∑ÇÈ
    private void FixedUpdate()
    {
        if (enemyHit && !healFlag)
        {
            healFlag = true;
            playerController.PlayerHP += 60;
        }
    }

    // on collision wityh enemy, activate flag
    // ìGÇ∆ê⁄êGÇµÇΩÇÁÉtÉâÉOÇÇ†Ç∞ÇÈ
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Enemies"))
        {
            enemyHit = true;
        }
    }

    // magic heal attack info
    // ñÇñ@âÒïúçUåÇèÓïÒ
    public void GetDamage(out int damage, out float stun, out Vector2 knockback, out int scaleX)
    {
        damage = 8;
        stun = 0.35f;
        knockback = new Vector2(1.5f, 1.5f);
        if(transform.localScale.x > 0)
        {
            scaleX = 1;
        }
        else
        {
            scaleX = -1;
        }
    }

    // heal function
    // âÒïúä÷êî
    IEnumerator Heal()
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(this.gameObject);
    }

    void shake()
	{
        CamShake.ShakeCamera(0.2f, 0.2f);
	}


}
