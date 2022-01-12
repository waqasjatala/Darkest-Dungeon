using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Video;

#if !UNITY_WEBGL
public class MoviePlayer : MonoBehaviour
{
    [SerializeField]
    public UnityEngine.Video.VideoClip videoClip;

    private GameIntro gameIntro;
    private AudioSource audioSource;
    private IEnumerator videoCoroutine;
    private int skipFrames = 6;
    private float vol;

    private void Awake()
    {
        gameIntro = GetComponentInParent<GameIntro>();
        audioSource = GetComponent<AudioSource>();
        var videoPlayer = gameObject.AddComponent<UnityEngine.Video.VideoPlayer>();

        videoPlayer.playOnAwake = false;
        videoPlayer.clip = videoClip;
        videoPlayer.renderMode = UnityEngine.Video.VideoRenderMode.MaterialOverride;
        videoPlayer.targetMaterialRenderer = GetComponent<Renderer>();
        videoPlayer.targetMaterialProperty = "_MainTex";
        videoPlayer.audioOutputMode = UnityEngine.Video.VideoAudioOutputMode.AudioSource;
        videoPlayer.SetTargetAudioSource(0, audioSource);
    }

    private void Update()
    {
        if (Input.anyKey)
            FinishVideo();
    }
    VideoPlayer vp;

    public void Play()
    {
        vp = GetComponent<UnityEngine.Video.VideoPlayer>();
        if (vp == null)
        {
          //  gameIntro.FinishIntro(); 
            return;
        }

        gameObject.SetActive(true);
        vp.Play();
        audioSource.Play();
        vol = audioSource.volume;
        GetComponent<RawImage>().color = new Color(255, 255, 255, 0);
        audioSource.volume = 0;
        videoCoroutine = VideoEnd();
        StartCoroutine(videoCoroutine);
    }

    private void FinishVideo()
    {
        StopCoroutine(videoCoroutine);
        vp.Pause();
        audioSource.Stop();
        gameObject.SetActive(false);
        gameIntro.FinishIntro();
    }

    private IEnumerator VideoEnd()
    {
        while (vp.isPlaying)
        {
            if (skipFrames != 0)
            {
                skipFrames--;
                if (skipFrames == 0)
                {
                    GetComponent<RawImage>().color = new Color(255, 255, 255, 255);
                    audioSource.volume = vol;
                }
            }
            yield return 0;
        }

        FinishVideo();
    }
}
#endif