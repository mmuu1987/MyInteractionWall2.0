using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class YearInfoItem : MonoBehaviour
{

    public Text YearText;

    public Text DescriptionText;

    public RawImage RawImage;

    private YearsEvent _yearsEvent;

    public ScrollRect ScrollRect;


    public Button ImageButton;
    private float _tempHeight = -1;
    // Start is called before the first frame update
    void Start()
    {
        ImageButton.onClick.AddListener((() =>
        {
            //Debug.Log("button");
            //UIControl.Instance.ShowImageFun(RawImage.texture, new Vector2(3600f, 2224f));
        }));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    /// <summary>
    /// 设置年代信息，true为设置成功，false为失败，失败原因可能是介绍字数太多
    /// </summary>
    /// <returns></returns>
    public float SetInfo(YearsEvent info)
    {
        _yearsEvent = info;

        YearText.text = info.Years;

        DescriptionText.text = info.Describe;

        _tempHeight = -1;

        _tempHeight = DescriptionText.preferredHeight + YearText.preferredHeight + 300f;

        Vector2 size = this.GetComponent<RectTransform>().sizeDelta;
      

        if (info.PicturesPath.Count > 0)
        {
            LoadImage();
        }
        else
        {


            this.GetComponent<RectTransform>().sizeDelta = new Vector2(size.x, _tempHeight);
            this.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, _tempHeight-1164f);

            RawImage.gameObject.SetActive(false);
        }

        return _tempHeight;

    }
    /// <summary>
    /// 匹配图片，不至于拉长，拉高图片
    /// </summary>
    private void ShowImage()
    {

        

        Vector2 temp = new Vector2(RawImage.texture.width,RawImage.texture.height);

       
        //图片的容器的宽高
        Vector2 size = new Vector2(RawImage.rectTransform.sizeDelta.x, RawImage.rectTransform.sizeDelta.y);
       
        


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

        RawImage.rectTransform.sizeDelta = new Vector2(temp.x, temp.y);
    }
    private void LoadImage()
    {

        string path = _yearsEvent.PicturesPath[0];

        byte[] bytes = File.ReadAllBytes(path);

        Texture2D tex = new Texture2D(4, 4, TextureFormat.DXT5, false);

        tex.LoadImage(bytes);

        tex.Apply();

        RawImage.texture = tex;
       
        RawImage.gameObject.SetActive(true);

        ShowImage();

        Vector2 desPos = DescriptionText.rectTransform.anchoredPosition;

        RawImage.rectTransform.anchoredPosition = new Vector2(desPos.x, desPos.y - DescriptionText.preferredHeight - 50);

        _tempHeight += (RawImage.rectTransform.sizeDelta.y)-200f;

        ScrollRect.gameObject.SetActive(true);

        RectTransform parent = ScrollRect.transform.Find("Viewport/Content").GetComponent<RectTransform>();

        ScrollRect.transform.Find("Viewport").GetComponent<Image>().raycastTarget = false;
        YearText.rectTransform.parent = parent;
        YearText.raycastTarget = false;
        DescriptionText.rectTransform.parent = parent;
        DescriptionText.raycastTarget = false;
        RawImage.raycastTarget = false;
        RawImage.rectTransform.parent = parent;

        parent.sizeDelta = new Vector2(parent.sizeDelta.x,_tempHeight);



    }
}
