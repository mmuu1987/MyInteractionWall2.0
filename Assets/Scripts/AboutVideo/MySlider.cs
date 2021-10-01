using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


/// <summary>
/// 滑动位置，预览该位置的视频定帧
/// </summary>
public class MySlider : Slider
{

    public Action<float,bool> OnChangeValue;

    private bool _isDrag = false;

    private Coroutine _coroutine;

    private WaitForSeconds _waitForSeconds;

    protected override void Awake()
    {
        base.Awake();
        _waitForSeconds=new WaitForSeconds(0.2f);
    }
    
    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerUp(eventData);

        _coroutine = StartCoroutine(Wait());
        _isDrag = true;
    }

    public override void OnPointerUp(PointerEventData eventData)
    {

        base.OnPointerUp(eventData);
        _isDrag = false;
        if (_coroutine != null) StopCoroutine(_coroutine);
        _coroutine = null;
        if (OnChangeValue != null)
            OnChangeValue(this.value, _isDrag);
    }
   
    public void SetValue(float newValue)
    {
        if (_isDrag) return;
        this.value = newValue;
    }

    private IEnumerator Wait()
    {
        while (true)
        {
            yield return _waitForSeconds;
            if (OnChangeValue != null)
                OnChangeValue(this.value, _isDrag);

        }
    }
}