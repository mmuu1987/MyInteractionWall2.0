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

    public Slider BigSliderLeft;

    public Slider BigSliderRight;

   public AudioSource AudioSource;

    public Button FullScaleButtonLeft;

  

    public Button SmallScreenButtonLeft;

    public Button SmallScreenButtonRight;

    public Button CloseButton;

    public RenderTexture RT;

    private RectTransform _rectTransform;



    private void Start()
    {
        SliderSmall.onValueChanged.AddListener((arg0 =>
        {
            AudioSource.volume = arg0;
        }));

        BigSliderLeft.onValueChanged.AddListener((arg0 =>
        {
            AudioSource.volume = arg0;
        }));


        BigSliderRight.onValueChanged.AddListener((arg0 =>
        {
            AudioSource.volume = arg0;
        }));

        FullScaleButtonLeft.onClick.AddListener((() =>
        {
            this._rectTransform.DOAnchorPos(new Vector2(0f, 0f), 0.55f);
            this._rectTransform.DOSizeDelta(new Vector2(7680f, 3240f), 0.55f);
            SliderSmall.gameObject.SetActive(false);
            FullScaleButtonLeft.gameObject.SetActive(false);
            BigSliderRight.transform.parent.gameObject.SetActive(true);
            BigSliderLeft.transform.parent.gameObject.SetActive(true);
           
        }));


       

        SmallScreenButtonLeft.onClick.AddListener((() =>
        {
            this._rectTransform.DOAnchorPos(new Vector2(-1842f, 0f), 0.55f);
            this._rectTransform.DOSizeDelta(new Vector2(1920, 1080f), 0.55f);
            SliderSmall.gameObject.SetActive(true);
            FullScaleButtonLeft.gameObject.SetActive(true);
            BigSliderLeft.transform.parent.gameObject.SetActive(false);
            BigSliderRight.transform.parent.gameObject.SetActive(false);

        }));

        SmallScreenButtonRight.onClick.AddListener((() =>
        {
            this._rectTransform.DOAnchorPos(new Vector2(-1842f, 0f), 0.55f);
            this._rectTransform.DOSizeDelta(new Vector2(1920, 1080f), 0.55f);
            SliderSmall.gameObject.SetActive(true);
            FullScaleButtonLeft.gameObject.SetActive(true);
            BigSliderLeft.transform.parent.gameObject.SetActive(false);
            BigSliderRight.transform.parent.gameObject.SetActive(false);

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
        FullScaleButtonLeft.gameObject.SetActive(true);
        BigSliderLeft.transform.parent.gameObject.SetActive(false);
    }
}
