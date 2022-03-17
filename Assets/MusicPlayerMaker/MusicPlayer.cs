
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using UnityEngine.UI;

public class MusicPlayer : UdonSharpBehaviour
{

    private bool isplay=false;
    public int id;
    public Slider yl;
    public AudioClip[] music;
    public AudioSource mc;
    private float katime;
    private float matime;
    private  float lyl;
    public GameObject sx;
    public GameObject sj;
    public GameObject loop;
    public Text Name;
    public GameObject vo;
    public GameObject vf;
    private bool mute;
    public GameObject playbt;
    public GameObject pausebt;
    public Text 开始time;
    public Text 总time;
    public Slider time条;


    public void Start()
    {
        mc.clip = music[id];
        matime = mc.clip.length;
        if (time条 != null)
            time条.maxValue = matime;
    }
    public void Update()
    {
/*mute*/{
            if (yl.value == 0 & vo.activeSelf)
            {
                vo.SetActive(false);
                vf.SetActive(true);
                lyl = 0;
                mute = true;
            }
            else if (yl.value > 0 & !mute)
            {
                vo.SetActive(true);
                vf.SetActive(false);
            }
        }       
/*time*/if (isplay)
        {
            katime = mc.time;
            if(time条!=null)
            time条.value = mc.time;
            if(playbt!=null&pausebt!=null)
            {
                playbt.SetActive(false);
                pausebt.SetActive(true);
            }
            if(开始time!=null&总time!=null)
            {
                开始time.text= string.Format(katime >= 600 ? "{0:00}:{1:00}" : "{0:0}:{1:00}", Mathf.Floor(katime / 60), Mathf.Floor(katime % 60));
                总time.text= string.Format(matime >= 600 ? "{0:00}:{1:00}" : "{0:0}:{1:00}", Mathf.Floor(matime / 60), Mathf.Floor(matime % 60));
            }
            
        }
        else if(!isplay)
        {
            if (playbt != null & pausebt != null)
            {
                playbt.SetActive(true);
                pausebt.SetActive(false);
            }
        }
/*Mode*/if (isplay & katime+0.1>=matime)
        {
            
            if (sx.activeSelf)
            {
                
                if (id < music.Length - 1)
                {
                    id = id + 1;
                    mc.clip = music[id];
                    Name.text = music[id].name;
                    mc.time = 0;
                    mc.Play();
                    matime = mc.clip.length;
                    if (time条 != null)
                        time条.maxValue = matime;
                    Debug.Log("下一首");
                }
                else if(id == music.Length - 1)
                {
                    id = 0;
                    mc.clip = music[id];
                    Name.text = music[id].name;
                    mc.time = 0;
                    mc.Play();
                    matime = mc.clip.length;
                    if (time条 != null)
                        time条.maxValue = matime;
                    Debug.Log("到第一首");
                }
            }
           else if(sj.activeSelf)
            {
                
                id = UnityEngine.Random.Range(0, music.Length);
                mc.clip = music[id];
                Name.text = music[id].name;
                mc.time = 0;
                mc.Play();
                matime = mc.clip.length;
                if (time条 != null)
                    time条.maxValue = matime;
                Debug.Log("随机");
            }
           else if(loop.activeSelf)
            {
                mc.Play();
                matime = mc.clip.length;
                if (time条 != null)
                    time条.maxValue = matime;
                Debug.Log("重复");
            }

            
        }
    }
    public void Time()
    {
        if (time条 != null)
        {
            if (time条.value != matime)
            {
                mc.time = time条.value;
                katime = mc.time;
            }
        }
       
    }
    public void play()
    {
        Stop();
        mc.clip = music[id];
        matime = mc.clip.length;
        if (time条 != null)
            time条.maxValue = matime;
        mc.Play();
        isplay=true;
        Name.text = music[id].name;
        
    }

    public void Stop()
    {
        mc.Stop();
        isplay = false;
        Name.text = "";
        if (time条 != null)
            time条.value = mc.time;
        if (开始time != null & 总time != null)
        {
            开始time.text = "00:00";
            总time.text = "00:00";
        }
    }
    public void voc()
    {
        mc.volume = yl.value;
        mute = false;
    }
    public void von()
    {
        vo.SetActive(false);
        vf.SetActive(true);
        lyl = yl.value;
        yl.value = 0;
        mc.volume = yl.value;
        mute = true;
    }
    public void vof()
    {
        vo.SetActive(true);
        vf.SetActive(false);
        if(lyl>0.1f)
        {
            yl.value = lyl;
        }
        else if(lyl<=0.1f)
        {
            yl.value = 0.1f;
        }
        mc.volume = yl.value;
        mute = false;
    }
    public void Sx()
    {
        sx.SetActive(false);
        sj.SetActive(true);
        loop.SetActive(false);
        mc.loop = false;
    }
    public void Sj()
    {
        sx.SetActive(false);
        sj.SetActive(false);
        loop.SetActive(true);
        mc.loop = true;
    }
    public void Loop()
    {
        sx.SetActive(true);
        sj.SetActive(false);
        loop.SetActive(false);
        mc.loop = false;
    }
    public void Last()
    {
       
        if(!sj.activeSelf)
        {
            if (id > 0)
            {
                id = id - 1;
                mc.clip = music[id];
                Name.text = music[id].name;
                matime = mc.clip.length;
                if (time条 != null)
                    time条.maxValue = matime;
                mc.time = 0;
                mc.Stop();
                mc.Play();
                isplay = true;
                Debug.Log("上一首");
            }
            else if (id == 0)
            {
                id = music.Length - 1;
                mc.clip = music[id];
                Name.text = music[id].name;
                matime = mc.clip.length;
                if (time条 != null)
                    time条.maxValue = matime;
                mc.time = 0;
                mc.Stop();
                mc.Play();
                isplay = true;
                Debug.Log("到最后一首");
            }
        }
        else if (sj.activeSelf)
        {

            id = UnityEngine.Random.Range(0, music.Length);
            mc.clip = music[id];
            Name.text = music[id].name;
            matime = mc.clip.length;
            if (time条 != null)
                time条.maxValue = matime;
            mc.time = 0;
            mc.Stop();
            mc.Play();
            isplay = true;
            Debug.Log("随机");
        }
    }
    public void Next()
    {
        
        if (!sj.activeSelf)
        {
            if (id < music.Length - 1)
            {
                id = id + 1;
                mc.clip = music[id];
                Name.text = music[id].name;
                matime = mc.clip.length;
                if (time条 != null)
                    time条.maxValue = matime;
                mc.time = 0;
                mc.Stop();
                mc.Play();
                isplay = true;
                Debug.Log("下一首");
            }
            else if (id == music.Length - 1)
            {
                id = 0;
                mc.clip = music[id];
                Name.text = music[id].name;
                matime = mc.clip.length;
                if (time条 != null)
                    time条.maxValue = matime;
                mc.time = 0;
                mc.Stop();
                mc.Play();
                isplay = true;
                Debug.Log("到第一首");
            }

        }
        else if (sj.activeSelf)
        {

            id = UnityEngine.Random.Range(0, music.Length);
            mc.clip = music[id];
            Name.text = music[id].name;
            matime = mc.clip.length;
            if (time条 != null)
                time条.maxValue = matime;
            mc.Stop();
            mc.Play();
            isplay = true;
            Debug.Log("随机");
        }
    }

    public void P()
    {
        mc.Play();
        isplay = true;
        playbt.SetActive(false);
        pausebt.SetActive(true);
    }
    public void Pau()
    {
        mc.Pause();
        isplay = false;
        playbt.SetActive(true);
        pausebt.SetActive(false);
    }

}
