using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineMove : MonoBehaviour
{

    public RectTransform Point1;

    public RectTransform Point2;


    private RectTransform _rectTransform;


    // Start is called before the first frame update
    void Start()
    {
        _rectTransform = this.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Point1 != null && Point2 != null)
        {
            Vector3 dir = Vector3.Normalize(Point1.anchoredPosition - Point2.anchoredPosition);

            float angle = Vector3.Angle(Vector3.right, dir);

            Vector3 newDir = Vector3.Cross(Vector3.right, dir);

            if (newDir.z < 0) angle *= -1;

           // Debug.Log(angle);

            this.transform.localRotation = Quaternion.Euler(new Vector3(0f,0f,angle));

            _rectTransform.anchoredPosition = Point1.anchoredPosition;

            float width = Vector2.Distance(Point1.anchoredPosition, Point2.anchoredPosition);

            _rectTransform.sizeDelta = new Vector2(width,_rectTransform.sizeDelta.y);
        }
    }
}
