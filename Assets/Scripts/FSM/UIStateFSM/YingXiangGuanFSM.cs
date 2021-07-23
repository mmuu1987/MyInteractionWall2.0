using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class YingXiangGuanFSM : UIStateFSM
{

    private RenderTexture _renderTexture;

    private List<VideoItem> _videoItems = new List<VideoItem>();

    private GameObject _videoPrefab;

    private Transform _videoParent;

    private VideoPlayManager _videoPlayer;


    public YingXiangGuanFSM(Transform go) : base(go)
    {
        _renderTexture = new RenderTexture(1920,1080,0);

        

        _videoPlayer = UIControl.Instance.VideoPlayManager;

        _videoPrefab = UIControl.Instance.VideoPrefab;

        _videoParent = UIControl.Instance.VideoParent.transform;

        SetVideoItem();

        go.transform.Find("BackLeft").GetComponent<Button>().onClick.AddListener((() =>
        {
            Target.ChangeState(UIState.Close);

        }));

        go.transform.Find("BackRight").GetComponent<Button>().onClick.AddListener((() =>
        {
            Target.ChangeState(UIState.Close);

        }));

        go.transform.Find("CloseBtn").GetComponent<Button>().onClick.AddListener((() =>
        {
            Target.ChangeState(UIState.Close);

        }));
    }

    public override void Enter()
    {
        base.Enter();
       
    }


    private void SetVideoItem()
    {
        foreach (string s in PictureHandle.Instance.YingSheGUanList)
        {
            VideoItem vi = Object.Instantiate(_videoPrefab, _videoParent).GetComponent<VideoItem>();
             _videoItems.Add(vi);
             vi.GetScaleTexture(s);
             vi.PlayButton.onClick.AddListener((() =>
             {
                 _videoPlayer.Play(vi.VideoPath);
             }));
        }
    }

    public override void Exit()
    {
        base.Exit();
       
    }
}
