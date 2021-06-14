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
    // Start is called before the first frame update
    void Start()
    {
        
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
        float tempHeight = -1;
        if (info.PicturesPath.Count > 0)
        {
            LoadImage();
        }
        else
        { 
            tempHeight = DescriptionText.preferredHeight + YearText.preferredHeight+300f;

            Vector2 size = this.GetComponent<RectTransform>().sizeDelta;
            this.GetComponent<RectTransform>().sizeDelta = new Vector2(size.x, tempHeight);


            this.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, tempHeight-1164f);

            RawImage.gameObject.SetActive(false);
        }

        return tempHeight;

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

        Vector2 desPos = DescriptionText.rectTransform.anchoredPosition;

        RawImage.rectTransform.anchoredPosition = new Vector2(desPos.x, desPos.y - DescriptionText.preferredHeight - 50);

    }
}
