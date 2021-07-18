using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class SiXiangChuanJiaHuoDong : MonoBehaviour
{
    public GameObject ImageIemPrefab;

    public RectTransform Parent;

   

    private YearsEvent _curYearsEvent;

    private TouchEvent _touchEvent;
    public RawImage ShowRawImage;

    public Image ShowRawImageParent;

   

    

    

    private Dictionary<string, string> _descDic = new Dictionary<string, string>();

    private List<SiXiangChuanJiaItem> _curRawImages = new List<SiXiangChuanJiaItem>();

    private int _curIndex = 0;

    private bool _isDrag = false;

    private Vector2 _gripSize = Vector2.one;

    

    // Start is called before the first frame update
    void Start()
    {
        ShowRawImageParent.GetComponent<Button>().onClick.AddListener((() =>
        {
            ShowRawImageParent.rectTransform.DOKill();
            ShowRawImageParent.rectTransform.DOScale(Vector3.zero, 0.35f);
        }));

        ShowRawImageParent.rectTransform.DOScale(Vector3.zero, 0f);


      

        _gripSize = Parent.GetComponent<RectTransform>().sizeDelta;

        _touchEvent = this.GetComponent<TouchEvent>();

        _touchEvent.DragMoveEvent += _touchEvent_DragMoveEvent;
        _touchEvent.OnBeginDragEvent += _touchEvent_OnBeginDragEvent;
        _touchEvent.OnEndDragEvent += _touchEvent_OnEndDragEvent;
    }


    private void OnEnable()
    {
       StartCoroutine(GetSize());
    }
    private void _touchEvent_OnEndDragEvent()
    {
        _isDrag = false;
    }

    private void _touchEvent_OnBeginDragEvent()
    {
        _isDrag = true;
    }

    private void _touchEvent_DragMoveEvent(float delta)
    {
        //if (delta > 0) return;//目前不允许右滑

        // Debug.Log(delta);
        foreach (SiXiangChuanJiaItem daShiJiItem in _curRawImages)
        {
            if (daShiJiItem.Move(delta, _gripSize.x))
            {
                
            }
        }
    }


    private IEnumerator GetSize()
    {
        yield return new WaitForEndOfFrame();
        _gripSize = Parent.GetComponent<RectTransform>().sizeDelta;
        Debug.Log(_gripSize);
        yield return null;
        Parent.GetComponent<ContentSizeFitter>().enabled = false;
        yield return null;
        Parent.GetComponent<GridLayoutGroup>().enabled = false;

    }

    // Update is called once per frame
    void Update()
    {
        if (!_isDrag)
        {
            _touchEvent_DragMoveEvent(-0.35f);
        }
    }
    public void Init(List<YearsEvent> yearsEvents)
    {

        foreach (YearsEvent yearsEvent in yearsEvents)
        {
            foreach (Texture2D texture2D in yearsEvent.TexList)
            {
                SiXiangChuanJiaItem item = Instantiate(ImageIemPrefab, Parent).GetComponent<SiXiangChuanJiaItem>();


                item.SetInfo(yearsEvent);
                RawImage rawImage = item.GetComponent<RawImage>();

                rawImage.texture = texture2D;

                string str = texture2D.name;

                Text text = item.transform.Find("Text").GetComponent<Text>();
                try
                {
                    string[] temp = str.Split(new string[] { "---" }, StringSplitOptions.None);

                    text.text = "<size=35>" + temp[0]+ "</size>"  + "\r\n"+ "<size=45>" + temp[1] + "</size>";
                }
                catch (Exception e)
                {
                    text.text = "图片命名不正确";
                }
                

                // Vector2 newSize = Common.ShowImageFun(new Vector2(texture2D.width, texture2D.height), new Vector2(912.4f, 527.16f));

                Vector2 newSize = new Vector2(790f, 527f);
                rawImage.rectTransform.sizeDelta = newSize;

                _curRawImages.Add(item);


                rawImage.GetComponent<Button>().onClick.AddListener((() =>
                {
                    ShowRawImage.texture = rawImage.texture;
                    ShowRawImage.rectTransform.sizeDelta = newSize*4;
                    ShowRawImageParent.rectTransform.DOKill();
                    ShowRawImageParent.rectTransform.DOScale(Vector3.one, 0.35f);
                }));
            }
        }


    }

}
