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
public class XieTongWeiHuoDong : MonoBehaviour
{

    public GameObject ImageIemPrefab;

    public RectTransform Parent;

    private List<YearsEvent> _yearsEvents;

    private YearsEvent _curYearsEvent;

    public Text TitleText;

    public Text Description;

    public RawImage ShowRawImage;

    public Image ShowRawImageParent;

    public Button PreviouseBtn;

    public Button NextBtn;



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

        PreviouseBtn.onClick.AddListener((Previous));

        NextBtn.onClick.AddListener((Next));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Next()
    {
        _curIndex++;

        if (_yearsEvents.Count <= _curIndex)
        {
            _curIndex = 0;
        }
        ShowInfo();
    }


    private void Previous()
    {
        _curIndex--;

        if (_curIndex <= 0)
        {
            _curIndex = _yearsEvents.Count-1;
        }

        ShowInfo();
    }

    private void ShowInfo()
    {
        _descDic.Clear();

        YearsEvent ye = _yearsEvents[_curIndex];

        if ( ye != _curYearsEvent)
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

    private void SetInfo()
    {

        try
        {
            string str = _curYearsEvent.Describe;

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

       

        foreach (Texture2D texture2D in _curYearsEvent.TexList)
        {
            RawImage rawImage = Instantiate(ImageIemPrefab, Parent).transform.Find("Content").GetComponent<RawImage>();

            rawImage.texture = texture2D;

            Vector2 newSize =Common.ShowImageFun(new Vector2(texture2D.width, texture2D.height), new Vector2(780f, 600f));

            rawImage.rectTransform.sizeDelta = newSize;

            _curRawImages.Add(rawImage);

            rawImage.transform.Find("Text").gameObject.SetActive(false);

            rawImage.transform.parent.GetComponent<Button>().onClick.AddListener((() =>
            {
                ShowRawImage.texture = rawImage.texture;

                Vector2 size = Common.ShowImageFun(new Vector2(rawImage.texture.width, rawImage.texture.height),
                    new Vector2(1920, 1080));
               // ShowRawImage.rectTransform.sizeDelta = size;
                ShowRawImageParent.rectTransform.DOKill();
                ShowRawImageParent.rectTransform.DOScale(Vector3.one, 0.35f);
            }));
        }
    }

    public void Init()
    {
        _yearsEvents = PictureHandle.Instance.XieTongHuoDongList;
        _curIndex = 0;
        _curYearsEvent = _yearsEvents[_curIndex];

        SetInfo();


    }


}
