
using UdonSharp;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;



public class playbutton : UdonSharpBehaviour
{
   public MusicPlayer  muc;
    public int id;
    string t;
    


    public void Start()
    {
        var button = gameObject.GetComponent<Button>();
        //button.onClick.AddListener(Interact);
        //button.onClick.SetPersistentListenerState(0, UnityEventCallState.RuntimeOnly);
        //var udon = UnityEvent.GetValidMethodInfo(this, "Interact", new System.Type[] { });
        //UnityEventTools.AddVoidPersistentListener(button.onClick, System.Delegate.CreateDelegate(typeof(UnityAction),this,udon,true) as UnityAction);
        muc = GameObject.Find("MusicPlayer").GetComponent<MusicPlayer>();
        t = transform.Find("Text1").gameObject.GetComponent<Text>().text;
        id = int.Parse(t);
    }
    public override void Interact()
    {
        muc.SendCustomEvent("Stop");
        muc.SetProgramVariable("id", id);
        muc.SendCustomEvent("play");
    }
}
