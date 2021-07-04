using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 匹配长宽到logo图，使之有外发光效果
/// </summary>
public class AdjustItem : MonoBehaviour
{

    private RectTransform parent;


    private RectTransform _rectTransform;
    /// <summary>
    /// 发光图片跟logo图的比值，适用于长宽
    /// </summary>
    private float scale;
    // Start is called before the first frame update
    void Start()
    {
        parent = this.transform.parent.GetComponent<RectTransform>();
        _rectTransform = this.GetComponent<RectTransform>();
        scale = 512f / 694f;
    }

    // Update is called once per frame
    void Update()
    {
        Adjust();
    }

    private void Adjust()
    {
        Vector2 size = parent.sizeDelta;

        Vector2 newSize = new Vector2(size.x/scale,size.y/scale);

        this._rectTransform.sizeDelta = newSize;


    }
}
