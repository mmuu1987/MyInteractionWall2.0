﻿using System.Collections;
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
    public ZhongXinXieTongFSM(Transform go,GameObject logoPrefab,Material material) : base(go)
    {
        _material = material;

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

                item.Move(_isFront);

                LogoItem oldItem = _backItems[index];
                oldItem.Move(!_isFront);
                index++;
            }
        }));
      

        List<Vector2> _randPos1 = GetVector2(1);

        List<Vector2> _randPos2 = GetVector2(2);

        int k = 2;


        if (_randPos2.Count < PictureHandle.Instance.FrontLogos.Count)
        {
            _randPos2 = GetVector2(k);
            k++;
        }
        //while (true)
        //{
        //    else break;
        //}

        k++;
        if (_randPos1.Count < PictureHandle.Instance.BackLogos.Count)
        {
            _randPos1 = GetVector2(k);
            k++;
        }

        //while (true)
        //{
          
        //    else break;
        //}
        Transform logoParent = go.transform.Find("LogoParent");
        int n = 0;
        foreach (YearsEvent meshRenderer in PictureHandle.Instance.FrontLogos)
        {
            int randPosIndex = Random.Range(0, _randPos1.Count);
            Vector2 pos = _randPos1[randPosIndex];
            LogoItem image = Object.Instantiate(logoPrefab, logoParent).GetComponent<LogoItem>();
            _frontItems.Add(image);

            Material mat = Object.Instantiate(_material);
            image.name = "LogoFront" + n;
            image.SetInfo(-2f , true,  pos,meshRenderer, mat);
            _randPos1.RemoveAt(randPosIndex);
           
            n++;
            if (n >= PictureHandle.Instance.FrontLogos.Count) n = 0;

        }

        n = 0;
        foreach (YearsEvent meshRenderer in PictureHandle.Instance.BackLogos)
        {
            int randPosIndex = Random.Range(0, _randPos2.Count);
            Vector2 pos = _randPos2[randPosIndex];
            LogoItem image = Object.Instantiate(logoPrefab, logoParent).GetComponent<LogoItem>();
            _backItems.Add(image);
            Material mat = Object.Instantiate(_material);
            image.name = "LogoBack" + n;
            image.SetInfo(-2f, false, pos, meshRenderer, mat);
            _randPos2.RemoveAt(randPosIndex);
           
            n++;
            if (n >= PictureHandle.Instance.BackLogos.Count) n = 0;


        }

        for (int i = 0; i < _frontItems.Count; i++)
        {
            _frontItems[i].RectTransform.SetAsLastSibling();
            _frontItems[i].name = "front "+i;
        }

        for (int i = 0; i < _backItems.Count; i++)
        {
            _backItems[i].RectTransform.SetAsFirstSibling();
            _backItems[i].name = "back " + i;
        }


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

        go.transform.Find("BackLeft").GetComponent<Button>().onClick.AddListener((() =>
        {
            Target.ChangeState(UIState.Close);

        }));
    }

    private List<Vector2> GetVector2(int seed)
    {
        List<Vector2> temp = new List<Vector2>();

        temp = Common.Sample2D(seed, Common.ContainerWidth * Common.Scale, Common.ContainerHeight, 750f, 50);

        Debug.Log("个数是 " + temp.Count);
        return temp;
    }

    public override void Enter()
    {
        base.Enter();
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

       _xieTongWeiHuoDong.GetComponent<RectTransform>().anchoredPosition = new Vector2(-5270f, 0f);

      
    }
}
