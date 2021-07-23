using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using XHFrameWork;

public class PrivateHeirsFSM : UIStateFSM
{
    /// <summary>
    /// 品牌介绍
    /// </summary>
    public Button BrandIntroductionBtn;
    /// <summary>
    /// 大湾区高净值中心按钮
    /// </summary>
    public Button DawanDistrictBtn;
    /// <summary>
    /// 增值服务
    /// </summary>
    public Button ValueAddedServices;

    public Button ChuanJiaShiPinBtn;

    private List<Texture2D> _brandTex;

  

    public List<Texture2D> DawanTex;

    public List<Texture2D> ValueAddTex;

    public List<Texture2D> ChuanDiTex;

    /// <summary>
    /// 显示内容的贴图  
    /// </summary>
    private RawImage ShowImage;

    /// <summary>
    /// 当前选中的贴图集合  
    /// </summary>
    private List<Texture2D> _curTex;

    private int _curIndex;

  

   
    private List<Image> _highlights;

    private Transform _previous;
    private Transform _next;

    private Transform _previousTouch;

    private Transform _nextTouch;


    private TouchEvent _touchEvent;
    private Text _numberText;
    public PrivateHeirsFSM(Transform go) : base(go)
    {
        _highlights = new List<Image>();

     

        BrandIntroductionBtn = Parent.transform.Find("1品牌介绍").GetComponent<Button>();

        ValueAddedServices = Parent.transform.Find("2增值服务体系").GetComponent<Button>();

        DawanDistrictBtn = Parent.transform.Find("3传缔致远").GetComponent<Button>();


        ChuanJiaShiPinBtn = Parent.transform.Find("4传家视频").GetComponent<Button>();

        ShowImage = Parent.parent.Find("ShowImage").GetComponent<RawImage>();

        _brandTex = PictureHandle.Instance.PrivateHeirsAllTexList[0].TexInfo;

        DawanTex = PictureHandle.Instance.PrivateHeirsAllTexList[1].TexInfo;

        ValueAddTex = PictureHandle.Instance.PrivateHeirsAllTexList[2].TexInfo;

        ChuanDiTex = PictureHandle.Instance.PrivateHeirsAllTexList[3].TexInfo;





        BrandIntroductionBtn.onClick.AddListener((() =>
        {
            SetBtn(_brandTex);
            SetHighlight(BrandIntroductionBtn.transform);
        }));
        BrandIntroductionBtn.transform.Find("Text").GetComponent<Text>().text = SettingManager.Instance.GetDirectName(Direct.PhOne);

        DawanDistrictBtn.onClick.AddListener((() =>
        {
            SetBtn(DawanTex);
            SetHighlight(DawanDistrictBtn.transform);
        }));
        DawanDistrictBtn.transform.Find("Text").GetComponent<Text>().text = SettingManager.Instance.GetDirectName(Direct.PhTwo);

        ValueAddedServices.onClick.AddListener((() =>
        {
            SetBtn(ValueAddTex);
            SetHighlight(ValueAddedServices.transform);
        }));
        ValueAddedServices.transform.Find("Text").GetComponent<Text>().text = SettingManager.Instance.GetDirectName(Direct.PhThree);


        ChuanJiaShiPinBtn.transform.Find("Text").GetComponent<Text>().text = SettingManager.Instance.GetDirectName(Direct.PhFour);
        ChuanJiaShiPinBtn.onClick.AddListener((() =>
        {
            SetBtn(ChuanDiTex);
            SetHighlight(ChuanJiaShiPinBtn.transform);
        }));



        _highlights.Add(BrandIntroductionBtn.transform.Find("Image").GetComponent<Image>());
        _highlights.Add(ValueAddedServices.transform.Find("Image").GetComponent<Image>());
        _highlights.Add(DawanDistrictBtn.transform.Find("Image").GetComponent<Image>());
        _highlights.Add(ChuanJiaShiPinBtn.transform.Find("Image").GetComponent<Image>());


        SetHighlight(BrandIntroductionBtn.transform);


        AddVideoTex(_brandTex, PictureHandle.Instance.PrivateHeirsAllTexList[0].VideoInfo);
        AddVideoTex(DawanTex, PictureHandle.Instance.PrivateHeirsAllTexList[1].VideoInfo);
        AddVideoTex(ValueAddTex, PictureHandle.Instance.PrivateHeirsAllTexList[2].VideoInfo);
        AddVideoTex(ChuanDiTex, PictureHandle.Instance.PrivateHeirsAllTexList[3].VideoInfo);

        _touchEvent = ShowImage.GetComponent<TouchEvent>();

        go.transform.Find("Scroll View1").GetComponent<SiXiangChuanJiaHuoDong>().Init(PictureHandle.Instance.SiXiangChuanJia_ChuanJiaHui);

        go.transform.Find("Scroll View2").GetComponent<SiXiangChuanJiaHuoDong>().Init(PictureHandle.Instance.SiXiangChuanJia_GaoDuanHuoDong);

        


        go.transform.parent.Find("CloseBtn").GetComponent<Button>().onClick.RemoveAllListeners();
        go.transform.parent.Find("CloseBtn").GetComponent<Button>().onClick.AddListener((() =>
        {
            UIControl.Instance.ChangeState(UIState.Close);
        }));


    }

  

    public override void Enter()
    {
        base.Enter();

        _previous = Target.transform.Find("ZhongXinBaoChengFSM/Previous");

        _next = Target.transform.Find("ZhongXinBaoChengFSM/Next");


        _previousTouch = Target.transform.Find("ZhongXinBaoChengFSM/TouchPrevious"); ;

        _nextTouch = Target.transform.Find("ZhongXinBaoChengFSM/TouchNext"); ;

        _numberText = Target.transform.Find("ZhongXinBaoChengFSM/NumberTip").GetComponent<Text>();

        EventTriggerListener.Get(_previous.gameObject).SetEventHandle(EnumTouchEventType.OnClick, Previous);

        EventTriggerListener.Get(_next.gameObject).SetEventHandle(EnumTouchEventType.OnClick, Next);

        _curTex = _brandTex;
        BrandIntroductionBtn.onClick.Invoke();
        Parent.parent.gameObject.SetActive(true);//父级别也要显示


        _touchEvent.TouchMoveEvent += TouchMoveEvent;


    }

    private void TouchMoveEvent(bool isleft)
    {
        if (!isleft)
        {
            if (_previous.gameObject.activeInHierarchy)
                Previous(null, null);
        }
        else
        {
            if (_next.gameObject.activeInHierarchy)
                Next(null, null);
        }
    }
    private void SetBtn(List<Texture2D> texs)
    {
        _curTex = texs;
        _curIndex = 0;
        ShowImage.texture = _curTex[_curIndex];
        CheckVideoTex(_curTex[_curIndex], ShowImage.gameObject);

        if (_curTex.Count == 1)
        {
            _previous.gameObject.SetActive(false);
            _next.gameObject.SetActive(false);
            _nextTouch.gameObject.SetActive(false);
            _previousTouch.gameObject.SetActive(false);
            _numberText.text = "1/1";
        }
        else
        {

            _previous.gameObject.SetActive(false);
            _next.gameObject.SetActive(true);
            _nextTouch.gameObject.SetActive(true);
            _previousTouch.gameObject.SetActive(false);
            _numberText.text = "1/" + _curTex.Count;

        }

    }

    /// <summary>
    /// 设置高亮
    /// </summary>
    public void SetHighlight(Transform parent)
    {
        foreach (Image image in _highlights)
        {
            if (image.transform.parent == parent)
            {
                image.gameObject.SetActive(true);
            }
            else
            {
                image.gameObject.SetActive(false);
            }
        }
    }
    private void Next(GameObject _listener, object _args, params object[] _params)
    {
        _curIndex++;
        if (_curIndex >= _curTex.Count)
        {
            _curIndex--;
        }

        ShowImage.texture = _curTex[_curIndex];
        CheckVideoTex(_curTex[_curIndex], ShowImage.gameObject);
        

        if (_curIndex == _curTex.Count - 1)
        {
            _previous.gameObject.SetActive(true);
            _next.gameObject.SetActive(false);
            _nextTouch.gameObject.SetActive(false);
            _previousTouch.gameObject.SetActive(true);
            _numberText.text = (_curIndex + 1) + "/" + _curTex.Count;
        }
        else
        {
            _previous.gameObject.SetActive(true);
            _next.gameObject.SetActive(true);
            _nextTouch.gameObject.SetActive(true);
            _previousTouch.gameObject.SetActive(true);
            _numberText.text = (_curIndex + 1) + "/" + _curTex.Count;
        }
      
        
    }
   
    private void Previous(GameObject _listener, object _args, params object[] _params)
    {
        //Debug.Log("previous");
        _curIndex--;
        if (_curIndex < 0)
        {
            _curIndex = 0;
        }

        ShowImage.texture = _curTex[_curIndex];
        CheckVideoTex(_curTex[_curIndex], ShowImage.gameObject);
        if (_curIndex == 0)
        {
            _previous.gameObject.SetActive(false);
            _next.gameObject.SetActive(true);
            _nextTouch.gameObject.SetActive(true);
            _previousTouch.gameObject.SetActive(false);
            _numberText.text = "1/" + _curTex.Count;
        }
        else
        {
            _previous.gameObject.SetActive(true);
            _next.gameObject.SetActive(true);
            _nextTouch.gameObject.SetActive(true);
            _previousTouch.gameObject.SetActive(true);
            _numberText.text = (_curIndex + 1) + "/" + _curTex.Count;
        }

    }


    public override void Exit()
    {
        _videoPlayer.enabled = false;
        base.Exit();

        Parent.gameObject.SetActive(false);

        _touchEvent.TouchMoveEvent -= TouchMoveEvent;

    }
}
