using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraShakeManager
{
    static CameraShake _targetCam;
    CameraShake shakeScript;

    public void Init()
	{
        _targetCam = GameObject.Find("Camera").GetComponent<CameraShake>();
	}

    public CameraShake GetShakeScript()
	{
        return shakeScript;
	}

    public CameraShakeManager()
    {
        if (_targetCam == null)
        {
            Init();
        }
        shakeScript = _targetCam;
    }

}

public class CameraShake : MonoBehaviour
{
    Transform cameraTransform;

	//Shake Parameters

	public float shakeDuration = 0.5f;
	public float shakeAmount = 0.5f;

	private bool canShake = false;
	private float _shakeTimer;

	
	void Start()
    {
		cameraTransform = GetComponent<Transform>();
    }

    
    void FixedUpdate()
    {

		if (canShake)
		{
			//orignalCameraPos = cameraTransform.localPosition;
			StartCameraShakeEffect();
			
		}
		else
		{
			cameraTransform.position = Vector3.zero;
		}
	}

	public void ShakeCamera(float ShakeTime, float ShakeStrength)
	{
		if (shakeAmount > ShakeStrength && canShake)
		{
			return;
		}
		shakeDuration = ShakeTime;
		shakeAmount = ShakeStrength;
		canShake = true;
		_shakeTimer = shakeDuration;
	}

	public void StartCameraShakeEffect()
	{
		if (_shakeTimer > 0)
		{
			cameraTransform.localPosition = Vector3.zero + Random.insideUnitSphere * shakeAmount;
			_shakeTimer -= Time.deltaTime;
		}
		else
		{
			_shakeTimer = 0f;
			cameraTransform.position = Vector3.zero;
			canShake = false;
		}
	}

}
