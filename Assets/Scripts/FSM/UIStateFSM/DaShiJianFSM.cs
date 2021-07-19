using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public class DaShiJianFSM : UIStateFSM
{

    private GameObject _gridGameObject;

    private Transform _gridTransform;

    private List<DaShiJiItem> items = new List<DaShiJiItem>();

   

    private Vector2 _gripSize = Vector2.one;

    private TouchEvent _touchEvent;

    Queue<DaShiJiItem> _queue = new Queue<DaShiJiItem>();

    WaitForEndOfFrame _waitForEndOfFrame = new WaitForEndOfFrame();

    private Coroutine _coroutine;

    private bool _isDrag = false;
    public DaShiJianFSM(Transform go,GameObject prefab,Transform parentGrid) : base(go)
    {
        _gridGameObject = prefab;
        _gridTransform = parentGrid;
        _touchEvent = go.GetComponent<TouchEvent>();
        go.transform.Find("Back").GetComponent<Button>().onClick.AddListener((() =>
        {
            Target.ChangeState(UIState.Close);
        }));

        go.transform.Find("BackLeft").GetComponent<Button>().onClick.AddListener((() =>
        {
            Target.ChangeState(UIState.Close);
        }));

        InitData();
    }

    public override void Enter()
    {
         Parent.gameObject.SetActive(true);
        Target.StartCoroutine( GetSize());
        _touchEvent.DragMoveEvent += _touchEvent_DragMoveEvent;
        _touchEvent.OnBeginDragEvent += _touchEvent_OnBeginDragEvent;
        _touchEvent.OnEndDragEvent += _touchEvent_OnEndDragEvent;

       _coroutine= Target.StartCoroutine(RandRot());
    }

    private void _touchEvent_OnEndDragEvent()
    {
        _isDrag = false;
    }

    private void _touchEvent_OnBeginDragEvent()
    {
        _isDrag = true;
    }

    public override void Excute()
    {
        base.Excute();

        if (!_isDrag)
        {
            _touchEvent_DragMoveEvent(-1f);
        }
    }

    /// <summary>
    /// 随机翻转某个图片
    /// </summary>
    /// <returns></returns>
    private IEnumerator RandRot()
    {
        while (true)
        {
            float time = Random.Range(1, 3);

            yield return new WaitForSeconds(time);

            int randIndex = Random.Range(0, items.Count);

            items[randIndex].Rotation();
        }
    }
    private void _touchEvent_DragMoveEvent(float delta)
    {
        //if (delta > 0) return;//目前不允许右滑
        if (items.Count < 21) return;//21个太少，不能滑动或者流动
        
      

      
       // Debug.Log(delta);
        foreach (DaShiJiItem daShiJiItem in items)
        {
            if (daShiJiItem.Move(delta, _gripSize.x))
            {
                _queue.Enqueue(daShiJiItem);
            }
        }


       

        //对超出左边边界的元素移动到右边，进行排列组合  //-412.5  -1287.5 -2162.5依次为rows的高
        while (_queue.Count > 0)
        {
            DaShiJiItem item = _queue.Dequeue();
            if (delta < 0)
            {
                DaShiJiItem endItem = items[items.Count - 1];
                if (items.Contains(item))
                {
                    items.Remove(item);

                    if (Math.Abs(endItem.RectTransform.anchoredPosition.y - -412.5f) < Mathf.Epsilon)
                    {
                        item.RectTransform.anchoredPosition = new Vector2(endItem.RectTransform.anchoredPosition.x, -1287.5f);
                    }
                    else if (Math.Abs(endItem.RectTransform.anchoredPosition.y - -1287.5f) < Mathf.Epsilon)
                    {
                        item.RectTransform.anchoredPosition = new Vector2(endItem.RectTransform.anchoredPosition.x, -2162.5f);
                    }
                    else if (Math.Abs(endItem.RectTransform.anchoredPosition.y - -2162.5f) < Mathf.Epsilon)
                    {
                        item.RectTransform.anchoredPosition = new Vector2(endItem.RectTransform.anchoredPosition.x + 50 + endItem.RectTransform.sizeDelta.x, -412.5f);
                    }
                    else throw new UnityException("数据不符合规范");


                    
                    items.Insert(items.Count, item);

                }
            }
            else
            {
                DaShiJiItem begItem = items[0];
                if (items.Contains(item))
                {
                    items.Remove(item);

                    if (Math.Abs(begItem.RectTransform.anchoredPosition.y - -412.5f) < Mathf.Epsilon)
                    {
                      
                        item.RectTransform.anchoredPosition = new Vector2(begItem.RectTransform.anchoredPosition.x - 50 - begItem.RectTransform.sizeDelta.x, -2162.5f);
                    }
                    else if (Math.Abs(begItem.RectTransform.anchoredPosition.y - -1287.5f) < Mathf.Epsilon)
                    {
                        item.RectTransform.anchoredPosition = new Vector2(begItem.RectTransform.anchoredPosition.x, -412.5f);
                    }
                    else if (Math.Abs(begItem.RectTransform.anchoredPosition.y - -2162.5f) < Mathf.Epsilon)
                    {
                        item.RectTransform.anchoredPosition = new Vector2(begItem.RectTransform.anchoredPosition.x, -1287.5f);
                    }
                    else throw new UnityException("数据不符合规范");


                   
                    items.Insert(0, item);

                }

            }
             
        }
    }

    

    private IEnumerator GetSize()
    {
        yield return new WaitForEndOfFrame();
        _gripSize = _gridTransform.GetComponent<RectTransform>().sizeDelta;
        Debug.Log(_gripSize);
        yield return null;
        _gridTransform.GetComponent<ContentSizeFitter>().enabled = false;
        yield return null;
        _gridTransform.GetComponent<GridLayoutGroup>().enabled = false;

    }

    private void InitData()
    {
        int n = 0;
        for (int i = 0; i < 3; i++)
        {
            foreach (YearsEvent yearsEvent in PictureHandle.Instance.DaShiJi)
            {
                foreach (Texture2D texture2D in yearsEvent.TexList)
                {
                    DaShiJiItem daShiJiItem = Object.Instantiate(_gridGameObject, _gridTransform).GetComponent<DaShiJiItem>();

                    items.Add(daShiJiItem);
                    daShiJiItem.GetComponent<Button>().onClick.AddListener((() =>
                    {
                        UIControl.Instance.ShowDaShiJiImage(texture2D, yearsEvent.Describe);
                    }));

                   

                    daShiJiItem.name = n.ToString();
                    daShiJiItem.Init(yearsEvent,texture2D);
                    n++;
                }
            }
        }
    }

   
    public override void Exit()
    {
        base.Exit();
        _touchEvent.DragMoveEvent -= _touchEvent_DragMoveEvent;
        _touchEvent.OnBeginDragEvent -= _touchEvent_OnBeginDragEvent;
        _touchEvent.OnEndDragEvent -= _touchEvent_OnEndDragEvent;


        if (_coroutine!=null)Target.StopCoroutine(_coroutine);
    }
}
