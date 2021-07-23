using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class DaShiJiItem : MonoBehaviour
{

    public YearsEvent CurYearsEvent;

    public RawImage ContentImage;

    public RawImage LogoImage;

    public Texture2D LogoChuanJia;

    public Texture2D LogoZhongXin;

   // public RawImage BackImage;

   

    public RectTransform RectTransform { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        RectTransform = this.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void  Init(YearsEvent yearsEvent,Texture2D texture2D)
    {
        ContentImage.texture = texture2D;

        float v = UnityEngine.Random.Range(0, 2);

        LogoImage.texture = v >= 1 ? LogoZhongXin : LogoChuanJia;
        LogoImage.SetNativeSize();
        //BackImage.texture = texture2D;
        CurYearsEvent = yearsEvent;
    }

    private float _angle = 0f;
    private float _scale = 1f;

   

    private Tween _tween;
    public void  Rotation()
    {

        if (_tween != null) return;
           
        _angle += 180f;
        _scale *= -1f;
           
        float delay = Random.Range(0.1f, 1f);
        _tween= ContentImage.rectTransform.DOLocalRotateQuaternion(Quaternion.Euler(new Vector3(0f, _angle, 0f)), 0.55f).SetDelay(delay);
            //ContentImage.rectTransform.DOScaleX(_scale,0.55f);

            LogoImage.rectTransform.DOLocalRotateQuaternion(Quaternion.Euler(new Vector3(0f, _angle + 180, 0f)), 0.55f).SetDelay(delay).OnComplete((
                () =>
                {
                    _angle += 180f;
                    _scale *= -1f;

                    float randTime = Random.Range(1f, 2f);
                    ContentImage.rectTransform.DOLocalRotateQuaternion(Quaternion.Euler(new Vector3(0f, _angle, 0f)), 0.55f).SetDelay(randTime);
                    //ContentImage.rectTransform.DOScaleX(_scale,0.55f);

                    LogoImage.rectTransform.DOLocalRotateQuaternion(Quaternion.Euler(new Vector3(0f, _angle + 180, 0f)), 0.55f).SetDelay(randTime).OnComplete((
                        () =>
                        {
                           
                            _tween = null;


                        }));

                    if (Math.Abs(_angle - 180f) < Mathf.Epsilon)
                    {
                        DoFade(ContentImage.rectTransform, 0f, randTime);
                        DoFade(LogoImage.rectTransform, 1f, randTime);
                    }
                    else
                    {
                        DoFade(ContentImage.rectTransform, 1f, randTime);
                        DoFade(LogoImage.rectTransform, 0f, randTime);
                    }

                    if (_angle >= 360f) _angle = 0f;

                }));
            // LogoImage.rectTransform.DOScaleX(_scale*-1f, 0.55f);

            if (Math.Abs(_angle - 180f) < Mathf.Epsilon)
            {
                DoFade(ContentImage.rectTransform, 0f,delay);
                DoFade(LogoImage.rectTransform, 1f, delay);

                //float v = Random.Range(0, 2);

                //LogoImage.texture = v > 1 ? LogoZhongXin : LogoChuanJia;
                //LogoImage.SetNativeSize();
            }
            else
            {
                DoFade(ContentImage.rectTransform, 1f, delay);
                DoFade(LogoImage.rectTransform, 0f, delay);
            }

         

    }

    private void DoFade(RectTransform rectTransform,float target,float delay)
    {
        MaskableGraphic[] maskableGraphics = rectTransform.GetComponentsInChildren<MaskableGraphic>();

        foreach (MaskableGraphic graphic in maskableGraphics)
        {
            graphic.DOFade(target, 0.55f).SetDelay(delay);
        }
    }
    public bool Move(float delta,float width)
    {
        if (RectTransform == null) return false;
        //-412.5  -1287.5 -2162.5
        Vector2 pos = new Vector2(RectTransform.anchoredPosition.x+delta, RectTransform.anchoredPosition.y);

        Vector2 size = RectTransform.sizeDelta;
        if (delta < 0)
        {
            if (pos.x + size.x / 2 < 0)
            {
               // pos = new Vector2(pos.x + width , pos.y);
                return true;
            }
        }
        else
        {
            if (pos.x - size.x / 2  > width - size.x)
            {
                return true;
            }
        }



        RectTransform.anchoredPosition = pos;

        return false;
    }

    //public void OnGUI()
    //{
    //    if (GUI.Button(new Rect(0f, 0f, 500f, 500f), "test"))
    //    {
    //        Rotation();
    //    }
    //}
}
