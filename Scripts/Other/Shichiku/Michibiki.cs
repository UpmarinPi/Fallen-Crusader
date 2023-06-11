using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class Michibiki : MonoBehaviour
{
    Light2D light2D;

    [SerializeField] float maxIntensity = 1f;
    [SerializeField] float minIntensity = 0.7f;
    [SerializeField] float maxOuterRadius = 0.4f;
    [SerializeField] float minOuterRadius = 0.2f;
    [SerializeField] float maxSpeed = 15f;
    [SerializeField] GameObject Player;
    MahoSE SE;
    PlayerController playerController;
    bool followFlag = false;
    bool onceFlag = false;
    [SerializeField] float speed = 1f;
    int direction = 0;
    Rigidbody2D rbody;
    
    void Start()
    {
        SE = transform.parent.GetComponent<MahoSE>();
        playerController = Player.GetComponent<PlayerController>();
        rbody = GetComponent<Rigidbody2D>();
        light2D = GetComponent<Light2D>();
        StartCoroutine(IntensityLoop());
    }

    private void FixedUpdate()
    {
        if (followFlag)
        {
            if (speed < maxSpeed)
			{
                speed = speed + 0.35f;
			}
			Vector3 dir = (Player.transform.position - transform.position).normalized;
            if (dir.x > 0)
                direction = 1;
            else
                direction = -1;
			float vx = dir.x * speed + 0.5f * direction;
            float vy = dir.y * speed + 0.5f * direction;

            rbody.velocity = new Vector2(vx, vy);
        }
    }

    IEnumerator IntensityLoop()
    {
        while (true)
        {
            for (float i = 0; i <= 1; i += 0.01f * Mathf.Pow(2, i))
            {
                light2D.intensity = Mathf.Lerp(minIntensity, maxIntensity, i);
                light2D.pointLightOuterRadius = Mathf.Lerp(minOuterRadius, maxOuterRadius, i);
                yield return new WaitForSeconds(0.02f);
            }

            for (float i = 1; i >= 0; i -= 0.01f * Mathf.Pow(2, i))
            {
                light2D.intensity = Mathf.Lerp(minIntensity, maxIntensity, i);
                light2D.pointLightOuterRadius = Mathf.Lerp(minOuterRadius, maxOuterRadius, i);
                yield return new WaitForSeconds(0.02f);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Vector2 pos = collision.gameObject.transform.position - transform.position;
            if (Mathf.Abs(pos.x) < 0.3f && Mathf.Abs(pos.y) < 0.3f && !onceFlag)
            {
                SE.DamageSE();
                onceFlag = true;
                playerController.PlayerHP += 10;
                Destroy(gameObject);
            }
            followFlag = true;
        }
    }

}
