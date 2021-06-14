using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DaShiJianFSM : UIStateFSM
{

    private GameObject _gridGameObject;

    private Transform _gridTransform;
    public DaShiJianFSM(Transform go,GameObject prefab,Transform parentGrid) : base(go)
    {
        _gridGameObject = prefab;
        _gridTransform = parentGrid;
    }

    public override void Enter()
    {
        

        float xTempUp = 560f;
        float xtempDown = 1000f;
        float endXUp = -1f;//上轴实例化出来后所摆放的右下角x位置
        float endXDown = -1f;//下周实例化出来后所摆放右下角x位置

        Vector2 itemSize =Vector2.zero;
        for (int i = 0; i < 100; i++)
        {

            GridItem gi = Object.Instantiate(_gridGameObject, _gridTransform).GetComponent<GridItem>();



            RectTransform rt = null;
           

            if (i == 0 || i % 2 == 0)
            {
                rt = gi.SetInfo(true);

                if (rt == null)
                {
                    float maxVal = endXUp > endXDown ? endXUp : endXDown;

                    Vector2 size = _gridTransform.GetComponent<RectTransform>().sizeDelta;
                    _gridTransform.GetComponent<RectTransform>().sizeDelta = new Vector2(maxVal + itemSize.x, size.y);
                    Object.Destroy(gi.gameObject);
                    break;
                }

                itemSize = rt.sizeDelta;
                if (endXUp<0f)
                  endXUp = xTempUp + itemSize.x * (i / 2f);
                else
                {
                    endXUp += itemSize.x;
                }
                rt.anchoredPosition = new Vector2(endXUp, 0f);
               
            }
            else  if ( i % 2 != 0)
            {
                rt = gi.SetInfo(false);

                if (rt == null)
                {
                    float maxVal = endXUp > endXDown ? endXUp : endXDown;

                    Vector2 size = _gridTransform.GetComponent<RectTransform>().sizeDelta;
                    _gridTransform.GetComponent<RectTransform>().sizeDelta = new Vector2(maxVal + itemSize.x, size.y);
                    Object.Destroy(gi.gameObject);
                    break;
                }
                itemSize = rt.sizeDelta;
                if (endXDown<0f)
                  endXDown = xtempDown + itemSize.x * (i - 1) / 2;
                else
                {
                    endXDown += itemSize.x;
                }
                rt.anchoredPosition = new Vector2(endXDown, -1164f);
            }
           

        }

       
    }
}
