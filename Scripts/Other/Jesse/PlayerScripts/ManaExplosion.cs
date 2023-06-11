using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaExplosion : MonoBehaviour, IDamageMagic
{
	Rigidbody2D rbody2d;
	bool explodeFlag = false;
	Animator anim;
	CameraShakeManager camManager;
	CameraShake CamShake;
	MahoSE SE;

	private void Start()
	{
		camManager = new CameraShakeManager();
		CamShake = camManager.GetShakeScript();
		SE = GetComponent<MahoSE>();
		anim = GetComponent<Animator>();
		rbody2d = GetComponent<Rigidbody2D>();
		StartCoroutine(DestroyAfter());
		anim.enabled = false;

	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		// on contact with enemy or ground explode
		// �n�ʂ�G�ƐڐG�����甚������
		if ((collision.gameObject.CompareTag("Enemies") || collision.gameObject.CompareTag("Ground")) && !explodeFlag)
		{
			explodeFlag = true;
			StartCoroutine(Explosion());
		}

	}

	// explosion damage info
	// �����̍U�����
	public void GetDamage(out int damage, out float stun, out Vector2 knockback, out int scaleX)
	{
		damage = 20;
		stun = 0.25f;
		float randomX = Random.Range(0, 1f);
		float randomY = Random.Range(0, 1f);
		knockback = new Vector2(3f + randomX, 2.5f + randomY);
		if (transform.localScale.x > 0)
		{
			scaleX = 1;
		}
		else
		{
			scaleX = -1;
		}
	}

	
	// explosion function
	// �����֐�
	IEnumerator Explosion()
	{
		SE.AttackSE();
		anim.enabled = true;
		rbody2d.velocity = new Vector2(0, 0);
		yield return null;
		yield return new WaitForAnimation(anim, 0);
		Destroy(gameObject);
	}

	// after a set time destroy object
	// ��莞�Ԍ� destroy
	IEnumerator DestroyAfter()
	{
		yield return new WaitForSeconds(6f);
		Destroy(this.gameObject);
	}

	// camera shake
	// �J�����̗h��
	void shake()
	{
		CamShake.ShakeCamera(0.2f, 0.2f);
	}


}
