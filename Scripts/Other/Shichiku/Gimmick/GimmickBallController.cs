using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IReflectBaller
{

}

public class GimmickBallController : MonoBehaviour, IReflectBaller
{
    [SerializeField] float swellingMaxSize = 1;
    [SerializeField] float swellingSpeed = 1;

    [SerializeField] float firePeriod = 3f;
    [SerializeField] float accelerationMax = 100f;

    GameObject beforeReflectTarget;
    GameObject afterReflectTarget;

    Rigidbody2D rbody2D;

    const int groundLayer = 9;
    const int playerHitLayer = 11;

    Coroutine currentCoroutine;

    public void Begin(GameObject firstTarget, GameObject secondTarget)
    {
        beforeReflectTarget = firstTarget;
        afterReflectTarget = secondTarget;

        rbody2D = GetComponent<Rigidbody2D>();
        GetComponent<Collider2D>().enabled = false;

        var currentCoroutine = StartCoroutine(BallSwelling(beforeReflectTarget));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<IDamageSword>() != null)
        {
            transform.GetChild(0).gameObject.layer = playerHitLayer;
            StopCoroutine(currentCoroutine);
            currentCoroutine = StartCoroutine(BallFiring(afterReflectTarget));
        }
        if (collision.transform.GetComponent<PlayerDamage>() != null
            || collision.gameObject.layer == groundLayer)
        {
            StopCoroutine(currentCoroutine);
            rbody2D.velocity = Vector2.zero;
            StartCoroutine(Explosion());
        }
    }

    public IEnumerator Explosion()
    {
        yield return null;
        Destroy(gameObject);
    }

    IEnumerator BallSwelling(GameObject target)
    {
        var ballSize = transform.localScale.x;
        while(swellingMaxSize > ballSize)
        {
            ballSize += swellingSpeed * Time.deltaTime;
            ballSize = Mathf.Min(ballSize, swellingMaxSize);
            transform.localScale = new Vector3(ballSize, ballSize, 1);
            yield return null;
        }
        GetComponent<Collider2D>().enabled = true;

        currentCoroutine = StartCoroutine(BallFiring(target));
    }

    IEnumerator BallFiring(GameObject target)
    {
        var velocity = Vector2.zero;
        var position = (Vector2)transform.position;
        var period = firePeriod;

        Vector2 diff = target.transform.position - transform.position;
        
        while (true)
        {
            var acceleration = Vector2.zero;
            acceleration += 2f * (diff - velocity * period) / period * period;
            period -= Time.deltaTime;

            if(acceleration.magnitude > accelerationMax)
            {
                acceleration = acceleration.normalized * 100f;
            }

            velocity += acceleration * Time.deltaTime;
            position += velocity * Time.deltaTime;
            transform.position = position;

            yield return new WaitForFixedUpdate();
        }
    }
}
