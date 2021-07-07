using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoPlayManager : MonoBehaviour
{
    public VideoPlayer VideoPlayer;

    public Slider SliderSmall;

    public Slider BigSlider;

   public AudioSource AudioSource;

    public Button FullScaleButton;

    public Button SmallScreenButton;

    public Button CloseButton;

    public RenderTexture RT;

    private RectTransform _rectTransform;



    private void Start()
    {
        SliderSmall.onValueChanged.AddListener((arg0 =>
        {
            AudioSource.volume = arg0;
        }));

        BigSlider.onValueChanged.AddListener((arg0 =>
        {
            AudioSource.volume = arg0;
        }));

        FullScaleButton.onClick.AddListener((() =>
        {
            this._rectTransform.DOAnchorPos(new Vector2(0f, 0f), 0.55f);
            this._rectTransform.DOSizeDelta(new Vector2(7680f, 3240f), 0.55f);
            SliderSmall.gameObject.SetActive(false);
            FullScaleButton.gameObject.SetActive(false);
            BigSlider.transform.parent.gameObject.SetActive(true);
           
        }));

        SmallScreenButton.onClick.AddListener((() =>
        {
            this._rectTransform.DOAnchorPos(new Vector2(-1842f, 0f), 0.55f);
            this._rectTransform.DOSizeDelta(new Vector2(1920, 1080f), 0.55f);
            SliderSmall.gameObject.SetActive(true);
            FullScaleButton.gameObject.SetActive(true);
            BigSlider.transform.parent.gameObject.SetActive(false);

        }));

        CloseButton.onClick.AddListener((Stop));

        _rectTransform = this.GetComponent<RectTransform>();
    }

    public void Play(string path)
    {
        this.gameObject.SetActive(true);
        VideoPlayer.url = path;
        VideoPlayer.Play();
    }

    public void Stop()
    {
        RT.Release();
        
        this.gameObject.SetActive(false);
        SliderSmall.gameObject.SetActive(true);
        FullScaleButton.gameObject.SetActive(true);
        BigSlider.transform.parent.gameObject.SetActive(false);
    }
}
