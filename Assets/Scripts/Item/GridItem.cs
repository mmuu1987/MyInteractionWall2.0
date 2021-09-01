using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridItem : MonoBehaviour
{

    public GameObject YearEventGameObject;


    public Image RedVerticalImage;

    public Image CircleImage;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public RectTransform SetInfo(bool isUp)
    {

        int fontCount = 50;//字体个数

        float tempHeight = -1f;

        //一个格子是否装了两个
        bool isTwo = false;
        //每个grid设置两个年份，如果第一个年份太大就不设置第二个年份

        if (PictureHandle.Instance.YearsEvents.Count >= 2)
        {
            YearsEvent ye1 = PictureHandle.Instance.YearsEvents[0];
            YearsEvent ye2 = PictureHandle.Instance.YearsEvents[1];



            if (ye1.PicturesPath.Count > 0 || ye1.Describe.Length >= fontCount)//第一个年份文字就很多，或者有图片
            {
                YearInfoItem item = Instantiate(YearEventGameObject, this.transform).GetComponent<YearInfoItem>();
                PictureHandle.Instance.YearsEvents.Remove(ye1);

                
                 tempHeight=item.SetInfo(ye1, isUp);
                
            }
            else if (ye1.Describe.Length <= fontCount && ye2.Describe.Length <= fontCount && ye1.PicturesPath.Count <= 0 && ye2.PicturesPath.Count <= 0)//第一，第二个年份都只有文字描述
            {
                YearInfoItem item1 = Instantiate(YearEventGameObject, this.transform).GetComponent<YearInfoItem>();
                YearInfoItem item2 = Instantiate(YearEventGameObject, this.transform).GetComponent<YearInfoItem>();
                PictureHandle.Instance.YearsEvents.Remove(ye1);
                PictureHandle.Instance.YearsEvents.Remove(ye2);

                item1.SetInfo(ye1);
                item1.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, -200f);
                item2.SetInfo(ye2);
                item2.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, -680f);

                isTwo = true;
            }
            else if (ye1.Describe.Length <= fontCount && ye2.Describe.Length >= fontCount || ye2.PicturesPath.Count>0)//第一个年份文字不长，但是第二个文字描述长或者第二个有图片
            {
                PictureHandle.Instance.YearsEvents.Remove(ye1);
                YearInfoItem item = Instantiate(YearEventGameObject, this.transform).GetComponent<YearInfoItem>();
                tempHeight= item.SetInfo(ye1,isUp);
               
            }
            else
            {
                Debug.LogError("没有合适的年份");
                
            }
        }
        else if ( PictureHandle.Instance.YearsEvents.Count == 1)
        {
            YearsEvent ye1 = PictureHandle.Instance.YearsEvents[0];
            PictureHandle.Instance.YearsEvents.Remove(ye1);
            YearInfoItem item = Instantiate(YearEventGameObject, this.transform).GetComponent<YearInfoItem>();
            item.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, -200f);
            tempHeight = item.SetInfo(ye1, isUp);
        }
        else
        {
            return null;
        }

       








        if (!isUp)
        {
            
            RedVerticalImage.rectTransform.anchorMin = new Vector2(0f,1f);
            RedVerticalImage.rectTransform.anchorMax = new Vector2(0f, 1f);
            RedVerticalImage.rectTransform.pivot = new Vector2(0f,1f);
            RedVerticalImage.rectTransform.anchoredPosition = new Vector2(0f, 0f);
           
            if(!isTwo)
                RedVerticalImage.rectTransform.sizeDelta = new Vector2(10f, tempHeight+100f);

            CircleImage.rectTransform.anchoredPosition = new Vector2(0f, 1164f);
        }
        else
        {
            if(tempHeight>0)
             RedVerticalImage.rectTransform.sizeDelta = new Vector2(10f, tempHeight);
        }
        
        return this.GetComponent<RectTransform>();
    }
}
