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

	bool playFlag;//開始フラグ。オンになった瞬間最初から再生、オフのときは止めたところから一時停止

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
