using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ZhongXinXieTongFSM : UIStateFSM
{
    private List<LogoItem> _logos = new List<LogoItem>();

    private int count = 100;

    private float Width = 500f;

    private float height = 309f;

    /// <summary>
    /// 所示的UI，布局在屏幕上，上下不能触及的范围
    /// </summary>
    private float heightReduce = 600;

    /// <summary>
    /// 填充的宽度的倍数
    /// </summary>
    private float scalar = 2.7f;
    public ZhongXinXieTongFSM(Transform go,GameObject logoPrefab) : base(go)
    {


        var randomPos = Common.Sample2D(7680f* scalar, 3240f- heightReduce * 2, Width, 30);

        Debug.Log("随机分散的个数是：" + randomPos.Count);

        for (int i = 0; i < count; i++)
        {
            LogoItem image = UIControl.Instantiate(logoPrefab, go).GetComponent<LogoItem>();

             int index = Random.Range(0, randomPos.Count);
             Vector2 randPos = randomPos[index];

             image.RectTransform.anchoredPosition = new Vector2(randPos.x,randPos.y+ heightReduce);

             image.RectTransform.sizeDelta = new Vector2(Width,height);

             randomPos.RemoveAt(index);
             image.name = i.ToString();
             image.SetInfo(-2f,scalar);

            _logos.Add(image);
            
        }
    }

    public override void Enter()
    {
        base.Enter();
        foreach (LogoItem logo in _logos)
        {
            logo.gameObject.SetActive(true);
        }
    }
}
