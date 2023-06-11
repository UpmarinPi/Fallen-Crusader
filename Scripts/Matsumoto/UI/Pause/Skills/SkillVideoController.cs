using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

[System.Serializable]
class SkillVideos
{
	[SerializeField] List<VideoClip> magicClip = new List<VideoClip>();
	[SerializeField] List<VideoClip> passiveClip = new List<VideoClip>();

	public VideoClip GetSkillVideo(SkillEnum _skillEnum, int _num)
	{
		Debug.Log(_skillEnum + " : " + _num);
		switch(_skillEnum)
		{
			case SkillEnum.Magic:
				return magicClip[_num];
			case SkillEnum.Passive:
				return passiveClip[_num];
			default:
				return null;
		}
	}
	public VideoClip GetSkillVideo(SelectingSkillInPause _magicsInPause)
	{
		return GetSkillVideo(_magicsInPause.skill, _magicsInPause.num);
	}
}

public class SkillVideoController : MonoBehaviour
{
	[SerializeField] SkillVideos skillVideos;
	[SerializeField] VideoClip forNullVideo;
	VideoPlayer videoPlayer;
	[SerializeField] SkillsInPause skillsInPause;
	[SerializeField] Pause pause;

	bool playFlag;//�J�n�t���O�B�I���ɂȂ����u�ԍŏ�����Đ��A�I�t�̂Ƃ��͎~�߂��Ƃ��납��ꎞ��~

	private void Start()
	{
		videoPlayer = this.GetComponent<VideoPlayer>();
	}
	private void Update()
	{
		videoPlayer.clip = skillVideos.GetSkillVideo(skillsInPause.GetSelectingSkillInPause()) ?? forNullVideo;
		if(!pause.moveFlag)
		{
			playFlag = false;
			videoPlayer.Pause();
		}
		if(!playFlag && pause.moveFlag)
		{
			playFlag = true;
			videoPlayer.Play();
		}
	}

}
