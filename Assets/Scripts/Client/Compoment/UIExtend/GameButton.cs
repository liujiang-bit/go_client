using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Skyunion;

public class GameButton : Button
{
    [HideInInspector] [SerializeField] public bool AutoPlaySound = true;
    [HideInInspector] [SerializeField] public string SoundAssetName = "Sound_Ui_CommonClickButton";
    [HideInInspector] [SerializeField] public bool AutoPlayRewardSound = false;
    [HideInInspector] [SerializeField] public string RewardSounedAssetName = "Sound_Ui_CommonReward";

    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);
        if (AutoPlaySound)
        {
            CoreUtils.audioService.PlayOneShot(SoundAssetName);
        }

        if (AutoPlayRewardSound)
        {
            CoreUtils.audioService.PlayOneShot(RewardSounedAssetName);
        }
    }

    public void SetSound(string soundName)
    {
        SoundAssetName = soundName;
    }

    public void SetMute()
    {
        this.AutoPlaySound = false;
    }
}