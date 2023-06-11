using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingSpikes : MonoBehaviour
{
    enum spikeDirection
    {
        up = 0,
        right, 
        left,
        down
    }


    [SerializeField] float distance = 0.5f;
    [SerializeField] float initalDelay = 0f;
    [SerializeField] float speed = 1f;
    [SerializeField] spikeDirection direction;
    GameObject player;
    SpikeSEController SE;

    
    void Start()
    {
        SE = GetComponent<SpikeSEController>();
        StartCoroutine(SpikeLoop());
        player = transform.parent.GetComponent<PlayerHolder>().player;
    }

    // constant spike loop 
    // Ç∆Ç∞ÇÃçUåÇÉãÅ[Év
    IEnumerator SpikeLoop()
    {
        yield return new WaitForSeconds(initalDelay);

        if (direction == spikeDirection.up || direction == spikeDirection.down)
            transform.Translate(new Vector2(0f, (-distance * 0.1f)));
        else
            transform.Translate(new Vector2((-distance * 0.1f), 0f));
        yield return new WaitForFixedUpdate();

        while (true)
        {
            int retractFrames = (int)(20 * (1/speed));
            int easeFrames = (int)(25 * (1 / speed));
            int stabFrames = 5;

            // the spikes retract
            // Ç∆Ç∞Ç™à¯Ç≠
            for (int i = 0; i < retractFrames; i++)
            {
                if (direction == spikeDirection.up || direction == spikeDirection.down)
                    transform.Translate(new Vector2(0f, (-(distance*0.9f)) / retractFrames));
                else
                    transform.Translate(new Vector2((-(distance*0.9f)) / retractFrames, 0f));

                yield return new WaitForFixedUpdate();
            }
            yield return new WaitForSeconds(1.5f * (1 / speed));

            // tip of the spike peeks out
            // Ç∆Ç∞ÇÃêÊÇ™èoÇÈ
            for (int i = 0; i < easeFrames; i++)
            {
                if (direction == spikeDirection.up || direction == spikeDirection.down)
                    transform.Translate(new Vector2(0f, (distance * 0.4f) / easeFrames));
                else
                    transform.Translate(new Vector2((distance * 0.4f) / easeFrames, 0f));

                yield return new WaitForFixedUpdate();
            }
            yield return new WaitForSeconds(0.5f * (1 / speed));
            if (Mathf.Abs(player.transform.position.x - transform.position.x ) < 8 && Mathf.Abs(player.transform.position.y - transform.position.y) < 8)
                SE.SpikeSE(SpikeStateSE.Normal);

            // spike stab 
            // Ç∆Ç∞Ç™éhÇ∑
            for (int i = 0; i < stabFrames; i++)
            {
                if (direction == spikeDirection.up || direction == spikeDirection.down)
                    transform.Translate(new Vector2(0f, (distance * 0.6f) / stabFrames));
                else
                    transform.Translate(new Vector2((distance * 0.6f) / stabFrames, 0f));

                yield return new WaitForFixedUpdate();
            }
            for (int i = 0; i < 5; i ++)
            {
                if (direction == spikeDirection.up || direction == spikeDirection.down)
                    transform.Translate(new Vector2(0f, (-distance * 0.1f) / 5));
                else
                    transform.Translate(new Vector2((-distance * 0.1f) / 5, 0f) );
                yield return new WaitForFixedUpdate();

            }
            yield return new WaitForSeconds(1.5f * (1 / speed));

        }

    }

}
