using System.Collections;
using System.Collections.Generic;
using System.Runtime.Hosting;
using DG.Tweening;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;


public enum UIState
{
    
    None,
    /// <summary>
    /// 关闭界面状态
    /// </summary>
    Close,
    /// <summary>
    /// 公司介绍
    /// </summary>
    CompanyIntroduction,
    /// <summary>
    /// 私享传家
    /// </summary>
    PrivateHeirs,
    /// <summary>
    /// 卓越风采
    /// </summary>
    OutstandingStyle,

    ZhongXinXieTong,

    YingXiangGuan,

    DaShiJi
}
/// <summary>
/// 大屏互动UI控制器
/// </summary>
public class UIControl : MonoBehaviour
{

    public static UIControl Instance;
    /// <summary>
    /// 荣誉墙按钮
    /// </summary>
    public Button HonorWallBtn;

    /// <summary>
    /// 公司介绍按钮
    /// </summary>
    public Button CompanyIntroductionBtn;

    /// <summary>
    /// 关闭公司介绍，私享传家  所共用的界面
    /// </summary>
    public Button CloseButton;
    /// <summary>
    /// 私享传家按钮
    /// </summary>
    public Button PrivateHeirsBtn;
    /// <summary>
    /// 卓越风采
    /// </summary>
    public Button YingSheGuanBtn;

    public Button ZhonXinXeiTongBtn;

    public Button DaShiJianBtn;


    public Button ShowImageParentBtn;

    public Button ShowDaShiJiRawImageBtn;

    public Button BackLeft;

    public Button BackRight;

    public RawImage ShowImage;

    public RawImage ShowDaShiJiRawImage;

    public VideoPlayManager VideoPlayManager;
    /// <summary>
    /// 荣誉墙
    /// </summary>
    public RectTransform HonorWall;

    public UIStateMachine _Machine;

    public Dictionary<UIState, UIStateFSM> DicUI;

    public Sprite HonorWallBtnLeft;

    public Sprite HonorWallBtnRight;

    public GameObject LogoGameObject;

    public GameObject GridPrefab;

    public Transform GridParent;

    public GameObject VideoPrefab;

    public GameObject VideoParent;

    public Material LogoIteMaterial;

    public GameObject DaShiJiPrefab;

    public GameObject DaShiJiParent;

    public VideoPlayer BGeffectVideo;

    /// <summary>
    /// 大事记图片显示时间
    /// </summary>
    public float ShowTime = 5f;
    private void Awake()
    {
        if(Instance!=null)throw new UnityException("已经设置了单例");
        Instance = this;

#if !UNITY_EDITOR_WIN
        Debug.unityLogger.logEnabled = false;    
#endif

        Screen.SetResolution(7680, 3240, true, 60);

        DicUI = new Dictionary<UIState, UIStateFSM>();

        _Machine = new UIStateMachine(this);

       



        DaShiJianBtn.onClick.AddListener((() =>
        {
            _Machine.ChangeState(DicUI[UIState.DaShiJi]);
        }));

        ZhonXinXeiTongBtn.onClick.AddListener((() =>
        {
            _Machine.ChangeState(DicUI[UIState.ZhongXinXieTong]);
        }));

        CompanyIntroductionBtn.onClick.AddListener((() =>
        {
            _Machine.ChangeState(DicUI[UIState.CompanyIntroduction]);
        }));

        PrivateHeirsBtn.onClick.AddListener((() =>
        {
            _Machine.ChangeState(DicUI[UIState.PrivateHeirs]);
        }));

        YingSheGuanBtn.onClick.AddListener((() =>
        {
            _Machine.ChangeState(DicUI[UIState.YingXiangGuan]);
        }));

        CloseButton.onClick.AddListener((() =>
        {
            _Machine.ChangeState(DicUI[UIState.Close]);
        }));

        ShowImageParentBtn.onClick.AddListener(() =>
        {
            ShowImageParentBtn.transform.DOScale(Vector3.zero, 0.55f).SetEase(Ease.InOutQuad).SetDelay(0.15f);
            ShowImageParentBtn.GetComponent<Image>().DOFade(0f, 0.55f);
        });

        ShowDaShiJiRawImageBtn.onClick.AddListener(() =>
        {
            ShowDaShiJiRawImageBtn.transform.DOScale(Vector3.zero, 0.55f).SetEase(Ease.InOutQuad).SetDelay(0.15f);
            ShowDaShiJiRawImageBtn.GetComponent<Image>().DOFade(0f, 0.55f);
        });

    }
    
	// Use this for initialization
	void Start ()
    {

        BGeffectVideo.url = Application.streamingAssetsPath + "/bgEffectVideo.mp4";
        BGeffectVideo.Play();

        HonorWallBtn.onClick.AddListener((ShowHonorWall));

        BackLeft.onClick.AddListener((ShowHonorWall));

        BackRight.onClick.AddListener((ShowHonorWall));



       // DicUI.Add(UIState.DaShiJi, new DaShiJianFSM(this.transform.Find("ZhongXinBaoCheng"), GridPrefab, GridParent));
        DicUI.Add(UIState.ZhongXinXieTong, new ZhongXinXieTongFSM(this.transform.Find("ZhongXinXieTongFSM"), LogoGameObject, LogoIteMaterial));
        DicUI.Add(UIState.CompanyIntroduction, new CompanyIntroductionFSM(this.transform.Find("ZhongXinBaoChengFSM/CompanyIntroduction")));
        DicUI.Add(UIState.PrivateHeirs, new PrivateHeirsFSM(this.transform.Find("ZhongXinBaoChengFSM/PrivateHeirs")));
        //DicUI.Add(UIState.OutstandingStyle, new OutstandingStyleFSM(this.transform.Find("ZhongXinBaoChengFSM/OutstandingStyle")));
        DicUI.Add(UIState.YingXiangGuan, new YingXiangGuanFSM(this.transform.Find("YinXiangGuanFSM")));
        DicUI.Add(UIState.DaShiJi, new DaShiJianFSM(this.transform.Find("GaoJingZhiDaShiJi"),DaShiJiPrefab,DaShiJiParent.transform));
        DicUI.Add(UIState.Close, new CloseFSM(null));

        _Machine.SetCurrentState(DicUI[UIState.Close]);
    }


    private void Update()
    {
        _Machine.SmUpdate();
    }
    private void ShowHonorWall()
    {
        if (HonorWall.position.x < 0)//打开荣誉墙
        {
            HonorWall.DOLocalMoveX(0f, 0.5f).SetEase(Ease.InOutQuad);

            Item[] items = this.transform.GetComponentsInChildren<Item>();

            foreach (Item item in items)
            {
                Destroy(item.gameObject);
            }
        }
        else//关闭荣誉墙
        {
            HonorWall.DOLocalMoveX(-7765, 0.5f).SetEase(Ease.InOutQuad);
         
            HonorWallBtn.transform.Find("Image").GetComponent<Image>().sprite = HonorWallBtnRight;

            HeadItem[] items = this.transform.GetComponentsInChildren<HeadItem>();

            foreach (HeadItem item in items)
            {
                Destroy(item.gameObject);
            }
        }

    }
 

    private Coroutine _coroutine;
    public void ShowDaShiJiImage(Texture tex,string description)
    {

        if(_coroutine!=null)StopCoroutine(_coroutine);
        _coroutine = null;

        Vector2 temp = new Vector2(tex.width, tex.height);


        //图片的容器的宽高
        Vector2 size = new Vector2(1600f, 1000f);

        float v2 = temp.x / temp.y;//图片的比率

        if (temp.x > temp.y)//如果图片宽大于高
        {
            if (temp.x > size.x)//如果图片宽大于容器的宽
            {
                temp.x = size.x;//以容器宽为准

                temp.y = size.x / v2;//把图片高按比例缩小

                if (temp.y > size.y)//如果图片的高还是大于容器的高
                {
                    temp.y = size.y;//则以容器的高为标准

                    temp.x = size.y * v2;//容器的高再度计算赋值

                    //一下逻辑同理
                }
            }
            else //如果图片宽小于容器的宽
            {

                if (temp.y > size.y)//如果图片的高还是大于容器的高
                {
                    temp.y = size.y;//则以容器的高为标准

                    temp.x = size.y * v2;//容器的高再度计算赋值


                }
            }
        }
        else if (temp.x <= temp.y)//如果图片的高大于宽 
        {
            if (temp.y > size.y)//如果图片高大于容器的高
            {
                temp.y = size.y;//以容器的高为准

                temp.x = size.y * v2;//重新计算图片的宽

                if (temp.x > size.x)//如果图片的宽还是大于容器的高
                {

                    temp.x = size.x;//则再次以容器的宽为标准

                    temp.y = size.x / v2;//再以容器的宽计算得到容器的高
                }
            }
            else //如果图片的高小于容器的高
            {
                //但是图片的宽大于容器的宽
                if (temp.x > size.x)
                {
                    temp.x = size.x;//以容器的宽为准
                    temp.y = size.x / v2;//再以容器的宽计算得到容器的高
                }

            }
        }

        ShowDaShiJiRawImage.texture = tex;
       
        ShowDaShiJiRawImage.rectTransform.sizeDelta = new Vector2(temp.x, temp.y);

        //如果缩放的图片太小，我们就把他放大一点

        ShowDaShiJiRawImage.rectTransform.parent.Find("Text").GetComponent<Text>().text = description;
        ShowDaShiJiRawImageBtn.transform.DOScale(1f, 0.55f).SetEase(Ease.InOutQuart);
        ShowDaShiJiRawImageBtn.GetComponent<Image>().DOFade(0f, 0.55f);

       _coroutine=  StartCoroutine(Common.WaitTime(ShowTime, (() =>
        {
            ShowDaShiJiRawImageBtn.onClick.Invoke();
        })));
    }

    public void ChangeState(UIState state)
    {
        _Machine.ChangeState(DicUI[state]);
    }
}
	
	// Update is called once per frame
