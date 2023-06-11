using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// �����_���l���g�p�����h��
/// </summary>
public class ShakeController : MonoBehaviour
{
    /// <summary>
    /// �h����
    /// </summary>

    [SerializeField] float myDuration;
    [SerializeField] float myStrength;
    [SerializeField] float myVibrato;
    private struct ShakeInfo
    {
        public ShakeInfo(float duration, float strength, float vibrato)
        {
            Duration = duration;
            Strength = strength;
            Vibrato = vibrato;
        }
        public float Duration { get; } // ����
        public float Strength { get; } // �h��̋���
        public float Vibrato { get; }  // �ǂ̂��炢�U�����邩
    }
    private ShakeInfo _shakeInfo;

    private Vector3 _initPosition; // �����ʒu
    private bool _isDoShake;       // �h����s�����H
    private float _totalShakeTime; // �h��o�ߎ���

    private void Start()
    {
        // �����ʒu��ێ�
        _initPosition = gameObject.transform.position;
        StartCoroutine(InvokeStart());
    }

    IEnumerator InvokeStart()
	{
        yield return new WaitForSeconds(1);

        StartShake(myDuration, myStrength, myVibrato);
	}

    private void Update()
    {
        if (!_isDoShake) return;
        
        // �h��ʒu���X�V
        gameObject.transform.position = UpdateShakePosition(
            gameObject.transform.position, 
            _shakeInfo, 
            _totalShakeTime, 
            _initPosition);
        
        // duration���̎��Ԃ��o�߂�����h�炷�̂��~�߂�
        _totalShakeTime += Time.deltaTime;
        if (_totalShakeTime >= _shakeInfo.Duration)
        {
            _isDoShake = false;
            _totalShakeTime = 0.0f;
            // �����ʒu�ɖ߂�
            gameObject.transform.position = _initPosition;
        }
    }

    /// <summary>
    /// �X�V��̗h��ʒu���擾
    /// </summary>
    /// <param name="currentPosition">���݂̈ʒu</param>
    /// <param name="shakeInfo">�h����</param>
    /// <param name="totalTime">�o�ߎ���</param>
    /// <param name="initPosition">�����ʒu</param>
    /// <returns>�X�V��̗h��ʒu</returns>>
    private Vector3 UpdateShakePosition(Vector3 currentPosition, ShakeInfo shakeInfo, float totalTime, Vector3 initPosition)
    {
        // -strength ~ strength �̒l�ŗh��̋������擾
        var strength = shakeInfo.Strength;
        var randomX = Random.Range(-1.0f * strength, strength);
        var randomY = Random.Range(-1.0f * strength, strength);
        
        // ���݂̈ʒu�ɉ�����
        var position = currentPosition;
        position.x += randomX;
        position.y += randomY;
        
        // �����ʒu-vibrato ~ �����ʒu+vibrato �̊ԂɎ��߂�
        var vibrato = shakeInfo.Vibrato;
        var ratio = 1.0f - totalTime / shakeInfo.Duration;
        vibrato *= ratio; // �t�F�[�h�A�E�g�����邽�߁A�o�ߎ��Ԃɂ��h��̗ʂ�����
        position.x = Mathf.Clamp(position.x, initPosition.x - vibrato, initPosition.x + vibrato);
        position.y = Mathf.Clamp(position.y, initPosition.y - vibrato, initPosition.y + vibrato);
        return position;
    }

    /// <summary>
    /// �h��J�n
    /// </summary>
    /// <param name="duration">����</param>
    /// <param name="strength">�h��̋���</param>
    /// <param name="vibrato">�ǂ̂��炢�U�����邩</param>
    public void StartShake(float duration, float strength, float vibrato)
    {
        // �h�����ݒ肵�ĊJ�n
        _shakeInfo = new ShakeInfo(duration, strength, vibrato);
        _isDoShake = true;
        _totalShakeTime = 0.0f;
    }
}