using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class TextMeshProAnimator : MonoBehaviour
{
    const float maxAmpdif = 49f;
    const float minAmp = 0f;//0.1f;

    const byte maxAlpha = 255;

    [SerializeField] private Gradient gradientColor;
    private TMP_Text textComponent;
    TMP_TextInfo textInfo;

    [SerializeField, Range(0.0f, 100.0f)] float amp = 50;
    [SerializeField, Range(0.001f, 49f)] float ampdif = 0.001f;

    [SerializeField] float yPos = -100;
    [SerializeField] float maxYPos = 0;
    [SerializeField] float speed = 200;
    [SerializeField] float acceleration = 1;

    [SerializeField] byte alphaPer = 1;
    [SerializeField] int interval = 1;
    [SerializeField] byte nextInterval = 30;

    class TextMeshCounter
    {
        public int plusCount = 0;
        public byte alpha = 0;
        public float amps = 0;
        public float ampdif = 0;
        public float yPos = 0;
        public float speed = 0;
    }
    List<TextMeshCounter> textMeshCounter = new List<TextMeshCounter>();

    int alphaCount = 0;
    int intervalCount = 0;

    private void Start()
    {
        if (this.textComponent == null)
            this.textComponent = GetComponent<TMP_Text>();
        CreateTextMeshCounter();
        InitTextMeshCounter();
    }

    private void OnEnable()
    {
        InitTextMeshCounter();
    }

    void CreateTextMeshCounter()
    {
        int j = 0;
        textMeshCounter.Add(new TextMeshCounter());
        j++;
        textMeshCounter.Add(new TextMeshCounter());
        for (int i = 1; i < textComponent.text.Length; i++)
        {
            if (textComponent.text[i] == ' ')
            {
                textMeshCounter[j].plusCount++;
            }
            else
            {
                textMeshCounter[j].plusCount++;
                j++;
                textMeshCounter.Add(new TextMeshCounter());
                textMeshCounter[j].plusCount = textMeshCounter[j - 1].plusCount; 
            }
        }
    }

    void InitTextMeshCounter()
    {
        alphaCount = 0;
        intervalCount = 0;

        for (int i = 0; i < textMeshCounter.Count; i++)
        {
            textMeshCounter[i].alpha = 0;
            textMeshCounter[i].amps = amp;
            textMeshCounter[i].ampdif = ampdif;
            textMeshCounter[i].yPos = yPos;
            textMeshCounter[i].speed = speed;
        }
    }


    private void Update()
    {
		UpdateAnimation();
    }

    private void UpdateAnimation()
    {
        // ① メッシュを再生成する（リセット）
        this.textComponent.ForceMeshUpdate(true);
        this.textInfo = textComponent.textInfo;

        // ②頂点データを編集した配列の作成
        var count = Mathf.Min(this.textInfo.characterCount, this.textInfo.characterInfo.Length);
        for(int i = 0; i < count; i++)
        {
            var charInfo = this.textInfo.characterInfo[i];
            if(!charInfo.isVisible)
                continue;

            int materialIndex = charInfo.materialReferenceIndex;
            int vertexIndex = charInfo.vertexIndex;

            // Gradient
            Color32[] colors = textInfo.meshInfo[materialIndex].colors32;
   //         float timeOffset = -0.5f * i;
			//float time1 = Mathf.PingPong(timeOffset + Time.realtimeSinceStartup, 1.0f);
   //         float time2 = Mathf.PingPong(timeOffset + Time.realtimeSinceStartup - 0.1f, 1.0f);
			//colors[vertexIndex + 0] = gradientColor.Evaluate(time1); // 左下
			//colors[vertexIndex + 1] = gradientColor.Evaluate(time1); // 左上
			//colors[vertexIndex + 2] = gradientColor.Evaluate(time2); // 右上
			//colors[vertexIndex + 3] = gradientColor.Evaluate(time2); // 右下

            byte alpha = 0;
            float amps = amp;
            float sinWave = yPos;

            for(int j = 0; j <= alphaCount; j++)
            {
                if(i == textMeshCounter[j].plusCount)
                {
                    alpha = textMeshCounter[j].alpha;
                    amps = Mathf.Max(textMeshCounter[j].amps - textMeshCounter[j].ampdif, minAmp);
                    sinWave = textMeshCounter[j].yPos;
                    break;
                }
            }

            if (alpha <= maxAlpha)
            {
                colors[vertexIndex + 0].a = alpha; // 左下
                colors[vertexIndex + 1].a = alpha; // 左上
                colors[vertexIndex + 2].a = alpha; // 右上
                colors[vertexIndex + 3].a = alpha; // 右下
            }

            // Wave
            Vector3[] verts = textInfo.meshInfo[materialIndex].vertices;


            //float sinWaveOffset = 0.5f * i;
            //float sinWave = Mathf.Sin(sinWaveOffset + Time.realtimeSinceStartup * Mathf.PI) * amps;
            verts[vertexIndex + 0].y += sinWave;    // 左下
            verts[vertexIndex + 1].y += sinWave;    // 左上
            verts[vertexIndex + 2].y += sinWave;    // 右上
            verts[vertexIndex + 3].y += sinWave;    // 右下
            //verts[vertexIndex + 0].x += sinWave;    // 左下
            //verts[vertexIndex + 1].x += sinWave;    // 左上
            //verts[vertexIndex + 2].x += sinWave;    // 右上
            //verts[vertexIndex + 3].x += sinWave;    // 右下
        }

        intervalCount++;

        for(int i = 0; i <= alphaCount; i++)
        {
            if (textMeshCounter[i].alpha < maxAlpha && intervalCount % interval == 0)
            {
                //textMeshCounter[i].alpha += alphaPer;
                textMeshCounter[i].alpha = (byte)Mathf.Min(textMeshCounter[i].alpha + alphaPer, maxAlpha);
            }
            if (textMeshCounter[alphaCount].alpha >= nextInterval)
            {
                alphaCount = Mathf.Min(alphaCount + 1, textMeshCounter.Count - 1);
            }
        }

        for(int i = 0; i <= alphaCount; i++)
        {
            textMeshCounter[i].ampdif += textMeshCounter[i].ampdif * Time.deltaTime;
            textMeshCounter[i].ampdif = Mathf.Min(textMeshCounter[i].ampdif , maxAmpdif);
        }

        for(int i = 0; i <= alphaCount; i++)
        {
            textMeshCounter[i].yPos += Time.deltaTime * textMeshCounter[i].speed;
            textMeshCounter[i].yPos = Mathf.Min(textMeshCounter[i].yPos, maxYPos);
            textMeshCounter[i].speed -= Time.deltaTime * acceleration;
            textMeshCounter[i].speed = Mathf.Max(textMeshCounter[i].speed, 0);
        }

        // ③ メッシュを更新
        for(int i = 0; i < this.textInfo.materialCount; i++)
        {
            if(this.textInfo.meshInfo[i].mesh == null)
            {
                continue;
            }

            this.textInfo.meshInfo[i].mesh.colors32 = this.textInfo.meshInfo[i].colors32;
            textInfo.meshInfo[i].mesh.vertices = textInfo.meshInfo[i].vertices;  // 変更
            textComponent.UpdateGeometry(this.textInfo.meshInfo[i].mesh, i);
        }
    }
}
