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

    
    public RawImage ShowRawImage;

    public Image ShowRawImageParent;

   

    



    private Dictionary<string, string> _descDic = new Dictionary<string, string>();

    private List<RawImage> _curRawImages = new List<RawImage>();

    private int _curIndex = 0;
    // Start is called before the first frame update
    void Start()
    {
        ShowRawImageParent.GetComponent<Button>().onClick.AddListener((() =>
        {
            ShowRawImageParent.rectTransform.DOKill();
            ShowRawImageParent.rectTransform.DOScale(Vector3.zero, 0.35f);
        }));

        ShowRawImageParent.rectTransform.DOScale(Vector3.zero, 0f);

        //PreviouseBtn.onClick.AddListener((Previous));

        //NextBtn.onClick.AddListener((Next));
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void Next()
    {
        _curIndex++;

       
        ShowInfo();
    }


    private void Previous()
    {
        _curIndex--;

        ShowInfo();
    }

    private void ShowInfo()
    {
        _descDic.Clear();

        

    }

    private void SetInfo(List<YearsEvent> yearsEvents)
    {
      
      
    }

    public void Init(List<YearsEvent> yearsEvents)
    {


        foreach (YearsEvent yearsEvent in yearsEvents)
        {
            foreach (Texture2D texture2D in yearsEvent.TexList)
            {
                RawImage rawImage = Instantiate(ImageIemPrefab, Parent).transform.Find("Content").GetComponent<RawImage>();

                rawImage.texture = texture2D;

                Vector2 newSize = Common.ShowImageFun(new Vector2(texture2D.width, texture2D.height), new Vector2(912.4f, 527.16f));

                rawImage.rectTransform.sizeDelta = newSize;

                _curRawImages.Add(rawImage);

                rawImage.transform.parent.GetComponent<Button>().onClick.AddListener((() =>
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
