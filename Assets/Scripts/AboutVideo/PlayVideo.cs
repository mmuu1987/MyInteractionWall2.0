using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class PlayVideo : MonoBehaviour
{



    public VideoClip videoClip;         // 视频的文件 参数

    public MySlider videoTimeSlider;      // 视频的时间 Slider

    //定义参数获取VideoPlayer组件和RawImage组件

    internal VideoPlayer videoPlayer;

    private RawImage rawImage;

    // Use this for initialization

    void Start()
    {

        //获取场景中对应的组件

        videoPlayer = this.GetComponent<VideoPlayer>();

        rawImage = this.GetComponent<RawImage>();

        videoPlayer.clip = videoClip;

        //videoNameText.text = videoClip.name;

        clipHour = (int)videoPlayer.clip.length / 3600;

        clipMinute = (int)(videoPlayer.clip.length - clipHour * 3600) / 60;

        clipSecond = (int)(videoPlayer.clip.length - clipHour * 3600 - clipMinute * 60);

        videoPlayer.Play();


       

        videoTimeSlider.OnChangeValue += OnChangeValue;

        videoPlayer.frameReady += VideoPlayer_frameReady;
    }

    private bool _isRead = false;

    private bool _isDrag = false;
    private void VideoPlayer_frameReady(VideoPlayer source, long frameIdx)
    {
        _isRead = true;
    }

  
    private void OnChangeValue(float value,bool isDrag)
    {
        _isRead = false;
        _isDrag = isDrag;
        float oldVal = (float)videoPlayer.time / (float)videoPlayer.clip.length;
       
        Debug.Log("oldVal is:" +oldVal+"    value is:" + value);
        if (Math.Abs(oldVal - value) < 0.055f) return;
        videoPlayer.time = value * (float)videoPlayer.clip.length;
        videoPlayer.Play();

    }

    // Update is called once per frame

    void Update()
    {
        
        if (videoPlayer.texture == null)
        {
            return;
        }

        rawImage.texture = videoPlayer.texture;

        ShowVideoTime();

    }

    /// <summary>
    /// 显示当前视频的时间
    /// </summary>

    private void ShowVideoTime()
    {
        // 当前的视频播放时间

        currentHour = (int)videoPlayer.time / 3600;

        currentMinute = (int)(videoPlayer.time - currentHour * 3600) / 60;

        currentSecond = (int)(videoPlayer.time - currentHour * 3600 - currentMinute * 60);

        // 把当前视频播放的时间显示在 Text 上

        //videoTimeText.text = string.Format("{0:D2}:{1:D2}:{2:D2} / {3:D2}:{4:D2}:{5:D2}", currentHour, currentMinute, currentSecond, clipHour, clipMinute, clipSecond);

        // 把当前视频播放的时间比例赋值到 Slider 上
        if(_isRead && !_isDrag)
         videoTimeSlider.SetValue((float)(videoPlayer.time / videoPlayer.clip.length));

    }

   

    // 当前视频的总时间值和当前播放时间值的参数

    private int currentHour;

    private int currentMinute;

    private int currentSecond;

    private int clipHour;

    private int clipMinute;

    private int clipSecond;

}
