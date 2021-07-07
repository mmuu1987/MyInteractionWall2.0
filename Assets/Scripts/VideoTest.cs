using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoTest : MonoBehaviour
{

    public VideoPlayer VideoPlayer;

    public RawImage RawImage;
    // Start is called before the first frame update
    void Start()
    {
        //VideoPlayer.url = Application.streamingAssetsPath + "/映像馆/「致·新二十」中信保诚人寿广分成长纪录片（内勤篇）.mp4";

      
        //VideoPlayer.time = 0.1f;
        //VideoPlayer.prepareCompleted += VideoPlayer_prepareCompleted;
        //VideoPlayer.Prepare();


        videoFrameTexture = new Texture2D(2, 2);
        
        VideoPlayer.url = Application.streamingAssetsPath + "/映像馆/「致·新二十」中信保诚人寿广分成长纪录片（内勤篇）.mp4";
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

   
    Texture2D videoFrameTexture;
    RenderTexture renderTexture;
   
    int framesValue = 0;//获得视频第几帧的图片
    void OnNewFrame(VideoPlayer source, long frameIdx)
    {
        framesValue++;
        if (framesValue == 1)
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
            VideoPlayer.frameReady -= OnNewFrame;
            VideoPlayer.sendFrameReadyEvents = false;

            RawImage.texture = videoFrameTexture;

            VideoPlayer.Stop();
           // RawImage.SetNativeSize();
        }
    }

    void OnDisable()
    {
        if (!File.Exists(Application.persistentDataPath + "/temp.jpg"))
        {
           // ScaleTexture(videoFrameTexture, 800, 400, (Application.persistentDataPath + "/temp.jpg"));
        }
    }
    //生成缩略图
    void ScaleTexture(Texture2D source, int targetWidth, int targetHeight, string savePath)
    {

        Texture2D result = new Texture2D(targetWidth, targetHeight, TextureFormat.ARGB32, false);

        for (int i = 0; i < result.height; ++i)
        {
            for (int j = 0; j < result.width; ++j)
            {
                Color newColor = source.GetPixelBilinear((float)j / (float)result.width, (float)i / (float)result.height);
                result.SetPixel(j, i, newColor);
            }
        }
        result.Apply();
        File.WriteAllBytes(savePath, result.EncodeToJPG());
    }
}
