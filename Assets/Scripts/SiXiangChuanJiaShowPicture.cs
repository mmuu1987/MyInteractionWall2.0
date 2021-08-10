using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;


/// <summary>
/// 中信协同委活动图片管理
/// </summary>
public class SiXiangChuanJiaShowPicture : MonoBehaviour
{

    public GameObject ImageIemPrefab;

    public RectTransform Parent;

    

    private YearsEvent _curYearsEvent;

    public Text TitleText;

    public Text Description;

    public RawImage ShowRawImage;

    public Image ShowRawImageParent;

    private Dictionary<string, string> _descDic = new Dictionary<string, string>();

    private List<RawImage> _curRawImages = new List<RawImage>();


    private RectTransform _rectTransform;

    public Button GoBackButton;

    public bool IsRestSize = true;
    // Start is called before the first frame update
    void Start()
    {
        //ShowRawImageParent.GetComponent<Button>().onClick.AddListener((() =>
        //{
        //    ShowRawImageParent.rectTransform.DOKill();
        //    ShowRawImageParent.rectTransform.DOScale(Vector3.zero, 0.35f);
        //}));

        //ShowRawImageParent.rectTransform.DOScale(Vector3.zero, 0f);
        _rectTransform = this.GetComponent<RectTransform>();

        GoBackButton.onClick.AddListener(CloseInfo);
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void ShowInfo(YearsEvent ye)
    {

        
        _descDic.Clear();

        if (ye != _curYearsEvent)
        {
            _curYearsEvent = ye;

            foreach (RawImage image in _curRawImages)
            {
                Destroy(image.transform.parent.gameObject);
            }
            _curRawImages.Clear();
            Debug.Log("重置数据");
            SetInfo();
        }


    }

    public void CloseInfo()
    {
        _rectTransform.DOScale(Vector3.zero, 0.35f);
        _curYearsEvent = null;
    }
    private void SetInfo()
    {
        _rectTransform.DOScale(Vector3.one, 0.35f);

        try
        {
            string str = _curYearsEvent.Describe;

            str = str.Trim();

            string[] temps = str.Split(new[] { "\r\n" }, StringSplitOptions.None);

            foreach (string s in temps)
            {
                string[] temps1 = s.Split(new[] { "：" }, StringSplitOptions.None);

                _descDic.Add(temps1[0], temps1[1]);
            }

            TitleText.text = _descDic["活动"];

            Description.text = _descDic["简述"]; 

        }
        catch (Exception e)
        {
            TitleText.text = "格式不正确";

            Description.text = "格式不正确，请检查txt文档格式是否正确";

            Debug.LogError(e.ToString());
        }


        int n = 0;
        foreach (Texture2D texture2D in _curYearsEvent.TexList)
        {
            n++;
            RawImage rawImage = Instantiate(ImageIemPrefab, Parent).transform.Find("Content").GetComponent<RawImage>();

            rawImage.texture = texture2D;

           // Vector2 newSize = Common.ShowImageFun(new Vector2(texture2D.width, texture2D.height), new Vector2(780f, 600f));

           if(IsRestSize)
            rawImage.rectTransform.sizeDelta = new Vector2(800f,512f);

            _curRawImages.Add(rawImage);

            rawImage.transform.Find("Text").GetComponent<Text>().text = "第"+n+"/"+_curYearsEvent.TexList.Count+"页";

            rawImage.rectTransform.parent.GetComponent<Button>().onClick.AddListener((() =>
            {
                //UIControl.Instance.ShowImageFun(texture2D,new Vector2(3600f,2224f));
            }));


        }
    }

 


}
