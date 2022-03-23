using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;
using UnityEditor.Events;
using UnityEngine.UI;
using UnityEngine.Events;
using UdonSharp;
using VRC.Udon;
using System;
using UdonSharpEditor;

namespace MusicPlayerMaker {
    public class MusicPlayerMakerWindow : EditorWindow
    {
        [MenuItem("Window/MusicPlayerMaker")]
        static void Open()
        {
            GetWindow<MusicPlayerMakerWindow>("MusicPlayerMaker");
        }

        [SerializeField]
        public UdonBehaviour udon;
        [SerializeField]
        Texture2D Image;
        [SerializeField]
        GameObject PlayerObject;
        [SerializeField]
        AudioSource AudioSource;
        [SerializeField]
        AudioClip[] AudioClips;
        [SerializeField]
        Vector2 ButtonSize = new Vector2(170, 24);
        [SerializeField]
        float ButtonMargin = 5;
        [SerializeField]
        float ButtonA =0.4f;
        public MusicPlayer muc;
       public  AudioClip[] audio;
        public AudioClip[] lsau;

        int zong;
        int lg;
        bool cz=false;
        public AudioClip[] jc;
        int jilu;
        GameObject po;

        public UnityAction Interact;

        void OnGUI()
        {
            var oldPlayerObject = PlayerObject;
            var oldimage = Image;
            var oldbta = ButtonA;
            //muc = GameObject.Find("MusicPlayer").GetComponent<MusicPlayer>();
            
            //udon.publicVariables.TryGetVariableValue("", out music01);

            
            var so = new SerializedObject(this);
            so.Update();

            if (GUILayout.Button("添加音频"))
                MakePlayer();
            if (GUILayout.Button("重新排序和按钮大小修改(删减音频后要确保此按钮已被点击或自动执行）"))
                Px();
            EditorGUILayout.PropertyField(so.FindProperty("PlayerObject"), new GUIContent("音乐播放器"));
            EditorGUILayout.PropertyField(so.FindProperty("AudioSource"), new GUIContent("AudioSource"));
            EditorGUILayout.PropertyField(so.FindProperty("Image"), new GUIContent("背景图片"));
            EditorGUILayout.PropertyField(so.FindProperty("ButtonA"), new GUIContent("歌曲按钮背景不透明度0～1"));
            EditorGUILayout.PropertyField(so.FindProperty("ButtonSize"), new GUIContent("按钮大小"));
            EditorGUILayout.PropertyField(so.FindProperty("ButtonMargin"), new GUIContent("按钮行距"));
            EditorGUILayout.PropertyField(so.FindProperty("AudioClips"), new GUIContent("音频组添加（如果添加或删减后点播放测试发现播放器主脚本里的music组被还原到上一次了（如果有些歌曲点不了可以去看看这里的问题），请先点排序后手动对music组进行展开再合上即可，我也不知道为什么）"), true);

            so.ApplyModifiedProperties();
            if(PlayerObject==null)
            {
                return;
            }
            var canvas = PlayerObject.FindChild("Canvas");
            var image = canvas.GetOrAddComponent<Image>();
            var scr = PlayerObject.FindChild("Canvas").FindChild("Scroll View");
            int js;
            Transform[] zg;
            var content = PlayerObject.FindChild("Canvas").FindChild("Scroll View").FindChild("Viewport").FindChild("Content");
            zg = content.GetComponentsInChildren<Transform>();
            js = (zg.Length - 1) / 3;
            if (js != 0 && PlayerObject != oldPlayerObject && PlayerObject != null)
            {
                var bl = PlayerObject.FindChildOrCreate("变量存储");
                var textOba = bl.FindChildOrCreate("按钮不透明度");
                ButtonA = textOba.GetOrAddComponent<Text>().text==""? 0.4f : float.Parse(textOba.GetOrAddComponent<Text>().text);
                var textbtx = bl.FindChildOrCreate("按钮大小x");
                var textbty = bl.FindChildOrCreate("按钮大小y");
                ButtonSize = textbtx.GetOrAddComponent<Text>().text == "" && textbty.GetOrAddComponent<Text>().text == "" ? new Vector2(170, 24) : new Vector2(float.Parse(textbtx.GetOrAddComponent<Text>().text), float.Parse(textbty.GetOrAddComponent<Text>().text));
                var textbm = bl.FindChildOrCreate("按钮行距");
                ButtonMargin = textbm.GetOrAddComponent<Text>().text == "" ? 5 : float.Parse(textbm.GetOrAddComponent<Text>().text);
            }
            if (ButtonA>1)
            {
                ButtonA = 1f;
            }
            if(ButtonA!=oldbta && PlayerObject != null&& PlayerObject == oldPlayerObject)
            {
                Px();
            }
            if (Image != null && Image != oldimage && PlayerObject != null)
            { 
                upimage();
            }
            else if(Image == null&&oldimage!=null)
            {
                image.enabled = false;
                scr.GetComponent<Image>().enabled = true;
            }
            if (PlayerObject != null && PlayerObject != oldPlayerObject)
            {
                
                if(image.sprite != null)
                {
                  if (Image == null && canvas.GetComponent<Image>().sprite != null)
                      Image = canvas.GetComponent<Image>().sprite.texture;
                }
                else
                {
                    image.enabled = false;                   
                    scr.GetComponent<Image>().enabled = true;
                }
                udon = PlayerObject.GetComponent<UdonBehaviour>();
                udon.publicVariables.TryGetVariableValue("music", out jc);
                var obj = PlayerObject.FindChild("Audio Source");
                if (obj != null)
                    AudioSource = obj.GetComponent<AudioSource>();
                upimage();
                if (jc.Length!=0)
                {
                    
                    if(js>jc.Length)
                    {
                        Debug.LogError("发现主脚本MusicPlayer里的music音频组与已经添加在场景上的按钮不符，如果您不想删除的话，请对照左边歌曲名称层级顺序进行重新手动排序处理！");
                        cz = true;
                        jilu =js;
                        return;
                    }
                    lsau = jc;
                    Px();
                    Debug.Log("已读取" + lg+"个对象");
                }
                
            }
        }

        void upimage()
        {
            if (Image != null)
            {
                var scr = PlayerObject.FindChild("Canvas").FindChild("Scroll View");
                var canvas = PlayerObject.FindChild("Canvas");
                var image = canvas.GetComponent<Image>();
                scr.GetComponent<Image>().enabled = false;
                image.enabled = true;
                image.sprite = Sprite.Create(Image, new Rect(0, 0, Image.width, Image.height),new Vector2(0.5f,0.5f));                
                image.color = new Color(image.color.r, image.color.g, image.color.b, 0.4f);
                image.type = (Image.Type)3;
                image.fillMethod = (Image.FillMethod)1;
                image.fillOrigin = 0;
                image.fillAmount = 0.9f;
               
            }
        }
        void Px()
        {

            if (PlayerObject == null)
                return;
            upimage();
            var content = PlayerObject.FindChild("Canvas").FindChild("Scroll View").FindChild("Viewport").FindChild("Content");
            Transform[] zg;
            zg = content.GetComponentsInChildren<Transform>();
            int js;
            js = (zg.Length - 1) / 3;
            if (cz)
            {

                udon.publicVariables.TryGetVariableValue("music", out jc);
                if(js==jc.Length)
                {
                    lsau = jc;
                }
                else if(js!=jc.Length)
                {
                    Debug.LogError("请先去把主脚本MusicPlayer里的music音频组手动重新排序！");
                    return;
                }
                
            }
            cz = false;
            
            var lll = 0;
            foreach (Transform child in zg)
            {
                if (child.gameObject.name == "Text1")
                {
                    child.gameObject.GetComponent<Text>().text = lll.ToString();
                    lll++;
                    child.gameObject.GetComponent<Text>().enabled = false;
                }
            }
            lll = 0;
            
            
            int lf= AudioClips != null?AudioClips.Length:0;
            if(lf!=0)
            {
                for (var i = 0; i < AudioClips.Length; i++)
                {
                    var audioClip = AudioClips[i];
                    var cc = content.transform.Find(audioClip.name);
                    if (cc != null)
                    {

                        lf = lf - 1;
                    }
                }
                
            }
            udon = PlayerObject.GetComponent<UdonBehaviour>();
            udon.publicVariables.TryGetVariableValue("music", out jc);
            if (lsau.Length==0 & jc.Length==0 & js!=0)
            {
                Debug.LogError("您的音频组意外丢失，可能是因为在清除了主脚本MusicPlayer里音频组参数的情况下，同时关闭了本菜单，导致音频组数组丢失，你可以在播放器主脚本的music音频组变量手动按照左边歌曲名称层级顺序放置您的音频，这是目前唯一补救方法！");
                cz = true;
                jilu = js;
                return;
            }
           
            if (lsau.Length != 0)
            {
                for (var i = 0; i < lsau.Length - 1; i++)
                {
                    if (lsau[i] == null)
                    {
                        Debug.LogError("您的播放器主脚本MusicPlayer已导入的音频组内含有空对象，可能是添加音频的时候出错了，需要你去主脚本音频组检查按照左边歌曲名称层级顺序进行手动排序处理！");
                        cz = true;
                        jilu = js;
                        return;
                    }
                }
            }


            lg = lf + js;
            audio = new AudioClip[js];
            var contentRecttransform = content.GetComponent<RectTransform>();
            contentRecttransform.sizeDelta = new Vector2(contentRecttransform.sizeDelta.x, ButtonMargin + js * (ButtonSize.y + ButtonMargin));
            var offset = contentRecttransform.sizeDelta.y / 2 - ButtonMargin - ButtonSize.y / 2;
            for (var i = 0; i < js; ++i)
            {
                GameObject  buttonObject=null;
                
                foreach (Transform child in zg)
                {
                    if (child.gameObject.name == "Text1")
                    {
                       if( child.gameObject.GetComponent<Text>().text == i.ToString())
                        {
                            buttonObject = child.transform.parent.gameObject;
                        }
                    }
                }
                for(var j=0;j<lsau.Length;++j)
                {
                
                    var an = lsau[j];
                    if(an.name== buttonObject.name)
                    {
                        audio[i] = lsau[j];
                        break;
                    }
                }

                var image = buttonObject.GetComponent<Image>();
                    image.color = new Color(image.color.r, image.color.g, image.color.b, ButtonA);
                var rectTransform = buttonObject.GetComponent<RectTransform>();
                rectTransform.sizeDelta = ButtonSize;
                rectTransform.localPosition = Vector3.zero;
                rectTransform.localScale = Vector3.one;
                rectTransform.localRotation = new Quaternion(0, 0, 0, 0);
                rectTransform.anchoredPosition = new Vector2(0, offset - (i) * (ButtonSize.y + ButtonMargin));

                
                var textObject = buttonObject.FindChildOrCreate("Text");
                var text = textObject.GetOrAddComponent<Text>();
                var textRectTransform = textObject.GetComponent<RectTransform>();
                textRectTransform.sizeDelta = 5 * ButtonSize;
                textRectTransform.localPosition = Vector3.zero;
                textRectTransform.localScale = new Vector3((float)0.2, (float)0.2, 1);
                textRectTransform.localRotation = new Quaternion(0, 0, 0, 0);
                textRectTransform.anchoredPosition = Vector2.zero;
                text.fontSize = (int)(70 * (ButtonSize.y / 24));



            }
            var bl = PlayerObject.FindChildOrCreate("变量存储");
            var textOba = bl.FindChildOrCreate("按钮不透明度");
            textOba.GetOrAddComponent<Text>().text = ButtonA.ToString();
            textOba.GetOrAddComponent<Text>().enabled = false;
            var textbtx = bl.FindChildOrCreate("按钮大小x");
            textbtx.GetOrAddComponent<Text>().text = ButtonSize.x.ToString();
            textbtx.GetOrAddComponent<Text>().enabled = false;
            var textbty = bl.FindChildOrCreate("按钮大小y");
            textbty.GetOrAddComponent<Text>().text = ButtonSize.y.ToString();
            textbty.GetOrAddComponent<Text>().enabled = false;
            var textbm = bl.FindChildOrCreate("按钮行距");
            textbm.GetOrAddComponent<Text>().text = ButtonMargin.ToString();
            textbm.GetOrAddComponent<Text>().enabled = false;
            lsau = new AudioClip[lg];
            Array.Copy(audio, lsau, audio.Length);
            Selection.activeObject = udon;
            udon.publicVariables.TrySetVariableValue("music", audio);
            udon.SetProgramVariable("music", audio);
            Selection.activeObject = udon;
            Debug.Log("排序完成");

        }


        void MakePlayer()
        {
            if (PlayerObject == null)
                return;
            {
 //var stopInfo = UnityEvent.GetValidMethodInfo(AudioSource, "Stop", new System.Type[] { });
            //var playInfo = UnityEvent.GetValidMethodInfo(AudioSource, "Play", new System.Type[] { });
            //var clipInfo = typeof(AudioSource).GetProperty("clip").GetSetMethod();
            //var volumeInfo = typeof(AudioSource).GetProperty("volume").GetSetMethod();

            //var stopDelegate = System.Delegate.CreateDelegate(typeof(UnityAction), AudioSource, stopInfo, true) as UnityAction;
            //var playDelegate = System.Delegate.CreateDelegate(typeof(UnityAction), AudioSource, playInfo, true) as UnityAction;
            //var clipDelegate = System.Delegate.CreateDelegate(typeof(UnityAction<AudioClip>), AudioSource, clipInfo, true) as UnityAction<AudioClip>;
            //var volumeDelegate = System.Delegate.CreateDelegate(typeof(UnityAction<float>), AudioSource, volumeInfo, true) as UnityAction<float>;

            //var volumeSlider = PlayerObject.FindChild("Canvas").FindChild("VolumeSlider").GetComponent<Slider>();
            //Enumerable.Range(0, volumeSlider.onValueChanged.GetPersistentEventCount()).Reverse().ToList().ForEach(i => UnityEventTools.RemovePersistentListener(volumeSlider.onValueChanged, i));
            //UnityEventTools.AddPersistentListener(volumeSlider.onValueChanged, volumeDelegate);

            //var stopButton = PlayerObject.FindChild("Canvas").FindChild("StopButton").GetComponent<Button>();
            //Enumerable.Range(0, stopButton.onClick.GetPersistentEventCount()).Reverse().ToList().ForEach(i => UnityEventTools.RemovePersistentListener(stopButton.onClick, i));
            //UnityEventTools.AddVoidPersistentListener(stopButton.onClick, stopDelegate);
            }
            var content = PlayerObject.FindChild("Canvas").FindChild("Scroll View").FindChild("Viewport").FindChild("Content");
            Transform[] zg;
            int js, ss;
            zg = content.GetComponentsInChildren<Transform>();
            js = (zg.Length - 1) / 3;
            if (cz)
            {
                Px();
                udon.publicVariables.TryGetVariableValue("music", out jc);
                if (js != jc.Length)
                {
                    return;
                }

            }
            cz = false;
           
            ss = 0;
            
            int lf = AudioClips != null ? AudioClips.Length : 0;
            bool nu = true;
            if (lf != 0)
            {
                for (var i = 0; i < AudioClips.Length; i++)
                {
                    var audioClip = AudioClips[i];
                    var cc = content.transform.Find(audioClip.name);
                    if (cc != null)
                    {
                        ss = ss + 1;
                        lf = lf - 1;
                    }
                }
                nu = false;
            }
                
            lg = lf + js;
            var contentRecttransform = content.GetComponent<RectTransform>();
            contentRecttransform.sizeDelta = new Vector2(contentRecttransform.sizeDelta.x, ButtonMargin + lg * (ButtonSize.y + ButtonMargin));
            var offset = contentRecttransform.sizeDelta.y / 2 - ButtonMargin - ButtonSize.y / 2;
            foreach (Transform child in zg)
            {
                if (child.gameObject.name != "Text" & child.gameObject.name != "Text1")
                {
                    var ps = child.gameObject.GetComponent<RectTransform>();
                    var ls = ps.anchoredPosition.y;
                    ps.anchoredPosition = new Vector2(0, ls + lf / 2f * (ButtonMargin + (ButtonSize.y + ButtonMargin)) - ((lf * ButtonMargin) / 2));
                }
            }
            udon.publicVariables.TryGetVariableValue("music", out jc);
            audio = new AudioClip[lg];
            if (js>= jc.Length)
            {
                Array.Copy(lsau, audio, lsau.Length);
                
            }
            else if (js < jc.Length)
            {
                Px();
                Debug.LogWarning("检测到之前删减过音频,已为您重新排序");
                MakePlayer();
                return;
            }

            int sss=0;
            //content.DestroyAllChildren();
            if (nu) 
            {
                Debug.LogError("请不要添加空对象");
                return;
            }
            for (var i = 0; i < AudioClips.Length; ++i)
            {
                
                var audioClip = AudioClips[i];
                if(audioClip==null)
                {
                    sss++;
                    continue;
                }
                
                var cc = content.transform.Find(audioClip.name);
                if (cc != null)
                {
                    sss++;
                    continue;

                }
                audio[i + js - sss] = AudioClips[i];
                var buttonObject = content.FindChildOrCreate(audioClip.name);
                
                var image = buttonObject.GetOrAddComponent<Image>();                
                    image.color=new Color(image.color.r, image.color.g, image.color.b,ButtonA);
                var button = buttonObject.GetOrAddComponent<Button>();
                var uuudon = buttonObject.AddUdonSharpComponent<playbutton>();
                var uudon = buttonObject.GetComponent<UdonBehaviour>();
                Interact = uudon.Interact;
                var rectTransform = buttonObject.GetComponent<RectTransform>();
                UnityEventTools.AddPersistentListener(button.onClick, Interact);
                button.onClick.SetPersistentListenerState(0, UnityEventCallState.RuntimeOnly);
                rectTransform.sizeDelta = ButtonSize;
                rectTransform.localPosition = Vector3.zero;
                rectTransform.localScale = Vector3.one;
                rectTransform.localRotation = new Quaternion(0, 0, 0, 0);
                rectTransform.anchoredPosition = new Vector2(0, offset - (i + js - sss) * (ButtonSize.y + ButtonMargin));
                button.targetGraphic = image;
                {
//UnityEventTools.AddVoidPersistentListener(button.onClick, stopDelegate);
                //UnityEventTools.AddObjectPersistentListener(button.onClick, clipDelegate, audioClip);
                //UnityEventTools.AddVoidPersistentListener(button.onClick, playDelegate);
                //button.onClick.SetPersistentListenerState(0, UnityEventCallState.RuntimeOnly);
                //button.onClick.SetPersistentListenerState(1, UnityEventCallState.RuntimeOnly);
                //button.onClick.SetPersistentListenerState(2, UnityEventCallState.RuntimeOnly);
                }
                

                var textObject = buttonObject.FindChildOrCreate("Text");
                var text = textObject.GetOrAddComponent<Text>();
                var textRectTransform = textObject.GetComponent<RectTransform>();
                textRectTransform.sizeDelta = 5*ButtonSize;
                textRectTransform.localPosition = Vector3.zero;
                textRectTransform.localScale = new Vector3((float)0.2, (float)0.2,1);
                textRectTransform.localRotation = new Quaternion(0, 0, 0, 0);
                textRectTransform.anchoredPosition = Vector2.zero;
                text.text = audioClip.name;
                text.fontSize = (int)(70 *(ButtonSize.y/24));
                text.color = Color.black;
                text.alignment = TextAnchor.MiddleCenter;
                var texts = buttonObject.FindChildOrCreate("Text1");
                var texts1 = texts.GetOrAddComponent<Text>();
                texts1.text = (i + js - sss).ToString();
                texts1.enabled = false;


            }
            //muc.setaudio(AudioClips);
            lsau = new AudioClip[audio.Length];
            lsau = audio;

            var sb = AudioClips.Length - ss;
            Debug.Log("已添加"+sb+"个对象，共" + lsau.Length+"个对象");
            Selection.activeObject = udon;
            udon.publicVariables.TrySetVariableValue("music", audio);
            udon.SetProgramVariable("music", audio);
            Selection.activeObject = udon;
            AudioClips = null;
            Px();
        }


    }

    public static class Util {
        public delegate GameObject CreateChild();

        public static GameObject FindChildOrCreate(this GameObject gameObject, string name, CreateChild createChild = null) {
            var child = gameObject.FindChild(name);
            if (child != null)
                return child;
            child = createChild == null ? new GameObject(name) : createChild();
            child.name = name;
            child.transform.parent = gameObject.transform;
            return child;
        }

        public static GameObject FindChild(this GameObject gameObject, string name) {
            var childTransform = gameObject.transform.Find(name);
            return childTransform == null ? null : childTransform.gameObject;
        }

        public static void DestroyAllChildren(this GameObject gameObject) {
            Enumerable.Range(0, gameObject.transform.childCount).Select(i => gameObject.transform.GetChild(i).gameObject).ToList().ForEach(child => UnityEngine.Object.DestroyImmediate(child));
        }
    }
}

