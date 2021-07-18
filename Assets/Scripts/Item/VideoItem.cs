using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoItem : MonoBehaviour
{
    public VideoPlayer VideoPlayer;

    public RawImage RawImage;

    public string VideoName;

    public Text VideoNameText;

    Texture2D videoFrameTexture;
    RenderTexture renderTexture;


    public string VideoPath;
    public Button PlayButton;
    // Start is called before the first frame update
    void Start()
    {

        videoFrameTexture = new Texture2D(2, 2);
    }


    public void GetScaleTexture(string path)
    {
        DirectoryInfo info = new DirectoryInfo(path);

        try
        {
            VideoName = info.Name.Replace(info.Extension, "");

            VideoName = VideoName.Substring(3, VideoName.Length - 1);

            VideoNameText.text = VideoName;
        }
        catch (Exception e)
        {
            VideoNameText.text = "视频名字不符合规定格式";
        }
       

        VideoPath = "file://"+ path;
        VideoPlayer.url = path;
        VideoPlayer.playOnAwake = false;
        VideoPlayer.waitForFirstFrame = true;

        VideoPlayer.sendFrameReadyEvents = true;
        VideoPlayer.frameReady += OnNewFrame;
        VideoPlayer.Play();
    }

    private void VideoPlayer_prepareCompleted(VideoPlayer source)
    {
        source.Pause();
        //source.Stop();

    }

    // Update is called once per frame
    void Update()
    {

    }


    
    int framesValue = 0;//获得视频第几帧的图片
    void OnNewFrame(VideoPlayer source, long frameIdx)
    {
        framesValue++;
        if (framesValue == 2)
        {
            renderTexture = source.texture as RenderTexture;
            if (videoFrameTexture.width != renderTexture.width || videoFrameTexture.height != renderTexture.height)
            {
                videoFrameTexture.Resize(renderTexture.width, renderTexture.height);
            }
            RenderTexture.active = renderTexture;
            videoFrameTexture.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
            videoFrameTexture.Apply();
            RenderTexture.active = null;
            
            VideoPlayer.sendFrameReadyEvents = false;

            RawImage.texture = videoFrameTexture;
            VideoPlayer.Stop();

            VideoPlayer.frameReady -= OnNewFrame;
            Destroy(VideoPlayer);
            VideoPlayer = null;

            // RawImage.SetNativeSize();
        }
    }

}
