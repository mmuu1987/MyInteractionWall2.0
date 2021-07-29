using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ZhongXinXieTongFSM : UIStateFSM
{
    private List<LogoItem> _frontItems = new List<LogoItem>();

    private List<LogoItem> _backItems = new List<LogoItem>();

    private int count = 100;

    private float Width = 500f;

    private float height = 309f;

    /// <summary>
    /// 所示的UI，布局在屏幕上，上下不能触及的范围
    /// </summary>
    private float heightReduce = 600;


    private bool _isFront = true;

    /// <summary>
    /// 外发光材质
    /// </summary>
    private Material _material;
    /// <summary>
    /// 填充的宽度的倍数
    /// </summary>
    private float scalar = 2.7f;

    private XieTongWeiHuoDong _xieTongWeiHuoDong;

    private List<RectTransform> posLeftList = new List<RectTransform>();
    private List<RectTransform> posRightList = new List<RectTransform>();

    private RectTransform centeRectTransform;

    private List<LogoItem> logos = new List<LogoItem>();

    private TouchMove _touchMove;
    public ZhongXinXieTongFSM(Transform go,GameObject logoPrefab,Material material) : base(go)
    {
        _material = material;


        

        RectTransform poss = this.Parent.Find("Positions").GetComponent<RectTransform>();

        _touchMove = this.Parent.Find("LogoParent").GetComponent<TouchMove>();

        RectTransform[] temps = poss.GetComponentsInChildren<RectTransform>(true);

        foreach (RectTransform rectTransform in temps)
        {
            if (rectTransform.parent == poss)
            {
                if(rectTransform.name.Contains("left"))
                    posLeftList.Add(rectTransform);
                else if(rectTransform.name.Contains("right"))
                    posRightList.Add(rectTransform);
                else
                    centeRectTransform = poss;
            }
        }
      
        Button switchBtn = go.transform.Find("Button").GetComponent<Button>();

        switchBtn.onClick.AddListener((() =>
        {
            _isFront = !_isFront;

            if (_isFront)
            {
                switchBtn.transform.Find("Image").GetComponent<RectTransform>()
                    .DOAnchorPos(new Vector2(-259, 0f), 0.55f);
                switchBtn.transform.Find("Text").GetComponent<RectTransform>()
                    .DOAnchorPos(new Vector2(101, 0f), 0.55f);
                switchBtn.transform.Find("Text").GetComponent<Text>().text = "非金融板块";
            }
            else
            {
               

                switchBtn.transform.Find("Image").GetComponent<RectTransform>()
                    .DOAnchorPos(new Vector2(245f, 0f), 0.55f);
                switchBtn.transform.Find("Text").GetComponent<RectTransform>()
                    .DOAnchorPos(new Vector2(-70f, 0f), 0.55f);
                switchBtn.transform.Find("Text").GetComponent<Text>().text = "金融板块";
            }


            int index = 0;
            foreach (LogoItem item in _frontItems)
            {
                float delay = 1f;

               // item.Move(_isFront);

                LogoItem oldItem = _backItems[index];
              //  oldItem.Move(!_isFront);
                index++;
            }
        }));
      

        List<Vector2> randPosLeft = GetVector2(true);

        List<Vector2> randPos2Right = GetVector2(false);

       

       
        

        //while (true)
        //{
          
        //    else break;
        //}
        Transform logoParent = go.transform.Find("LogoParent");
        int n = 0;

        foreach (Vector2 vector2 in randPosLeft)
        {

            YearsEvent ye = PictureHandle.Instance.FrontLogos[n];
            Vector2 pos = vector2;
            LogoItem image = Object.Instantiate(logoPrefab, logoParent).GetComponent<LogoItem>();
            _frontItems.Add(image);

            Material mat = Object.Instantiate(_material);
            image.name = "LogoLeft" + n;
            image.SetInfo(-2f, true, pos, ye, mat);
           

            n++;
            if (n >= PictureHandle.Instance.FrontLogos.Count)
            {
                if (image.RectTransform.anchoredPosition.x < 7680f / -2)
                {
                    _touchMove.MaxRight = 7680f / -2 - image.RectTransform.anchoredPosition.x;
                }
                break;
            }
        }
       

        n = 0;

        foreach (Vector2 vector2 in randPos2Right)
        {

            YearsEvent ye = PictureHandle.Instance.BackLogos[n];
            Vector2 pos = vector2;
            LogoItem image = Object.Instantiate(logoPrefab, logoParent).GetComponent<LogoItem>();
            _backItems.Add(image);
            Material mat = Object.Instantiate(_material);
            image.name = "LogoRight" + n;
            image.SetInfo(-2f, false, pos, ye, mat);
           

            n++;
            if (n >= PictureHandle.Instance.BackLogos.Count)
            {
                if (image.RectTransform.anchoredPosition.x > 7680f / 2)
                {
                    _touchMove.MaxLeft =  image.RectTransform.anchoredPosition.x - 7680f /2 + image.RectTransform.sizeDelta.x/2 ;
                }
                break;
            }
        }

        logos.AddRange(_backItems);
        logos.AddRange(_frontItems);



        _xieTongWeiHuoDong = go.transform.Find("XieTongWeiHuoDong").GetComponent<XieTongWeiHuoDong>();
        _xieTongWeiHuoDong.Init();

        go.transform.Find("leftBtn").GetComponent<Button>().onClick.AddListener((() =>
        {
            RectTransform rt = _xieTongWeiHuoDong.GetComponent<RectTransform>();

            if (rt.anchoredPosition.x <= -4840f)
            {
                rt.DOAnchorPos(new Vector2(-2304f, 0f), 0.35f);
            }
            else
            {
                rt.DOAnchorPos(new Vector2(-5270f, 0f), 0.35f);
            }
           
        }));

        go.transform.Find("Back").GetComponent<Button>().onClick.AddListener((() =>
        {
           Target.ChangeState(UIState.Close);

        }));

        go.transform.Find("XieTongWeiHuoDong/BackLeft").GetComponent<Button>().onClick.AddListener((() =>
        {
            Target.ChangeState(UIState.Close);

        }));

        go.transform.Find("XieTongWeiHuoDong/goBack").GetComponent<Button>().onClick.AddListener((() =>
        {
            RectTransform rt = _xieTongWeiHuoDong.GetComponent<RectTransform>();
            rt.DOAnchorPos(new Vector2(-5270f, 0f), 0.35f);

        }));
    }

    private Coroutine _scaleCoroutine;
    private IEnumerator RangeScale()
    {
        while (true)
        {
            yield return new WaitForSeconds(4.6f);

            
            _backItems.AddRange(_frontItems);


            int count = Random.Range(1, 3);

            for (int i = 0; i < count; i++)
            {
                LogoItem item = logos[Random.Range(0, logos.Count )];

                item.Scale();
            }
        }
    }
    private List<Vector2> GetVector2(bool isLeft)
    {

        List<Vector2> temps = new List<Vector2>();

        if (isLeft)
        {
            foreach (RectTransform transform in posLeftList)
            {
                temps.Add(transform.anchoredPosition);
            }
        }
        else
        {
            foreach (RectTransform transform in posRightList)
            {
                temps.Add(transform.anchoredPosition);
            }
        }
       
        //List<Vector2> temp = new List<Vector2>();

        //temp = Common.Sample2D(seed, Common.ContainerWidth * Common.Scale, Common.ContainerHeight, 750f, 50);

        //Debug.Log("个数是 " + temp.Count);
        return temps;
    }

    public override void Enter()
    {
        base.Enter();
        if(_scaleCoroutine!=null) Target.StopCoroutine(_scaleCoroutine);
       _scaleCoroutine = Target.StartCoroutine(RangeScale());
        foreach (LogoItem logo in _frontItems)
        {
            logo.gameObject.SetActive(true);
        }
        foreach (LogoItem logo in _backItems)
        {
            logo.gameObject.SetActive(true);
        }
    }

    public override void Exit()
    {
        base.Exit();
        if (_scaleCoroutine != null) Target.StopCoroutine(_scaleCoroutine);
        _xieTongWeiHuoDong.GetComponent<RectTransform>().anchoredPosition = new Vector2(-5270f, 0f);

      
    }
}
