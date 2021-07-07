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
                Destroy(image.gameObject);
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
        }
        catch (Exception e)
        {

            Debug.LogError(e.ToString());
        }

        TitleText.text = _descDic["活动"];
        Description.text = _descDic["简述"];

        foreach (Texture2D texture2D in _curYearsEvent.TexList)
        {
            RawImage rawImage = Instantiate(ImageIemPrefab, Parent).GetComponent<RawImage>();

            rawImage.texture = texture2D;

            _curRawImages.Add(rawImage);

            rawImage.GetComponent<Button>().onClick.AddListener((() =>
            {
                ShowRawImage.texture = rawImage.texture;
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
