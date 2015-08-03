using UnityEngine;
using System.Collections;
using Holoville.HOTween;
using Client.Managers.Game;
using Common.Data;

public class ShroudWidget : MonoBehaviour 
{
	private UIWidget _widget;

    // TODO: Unfuck this entire script. (Kevin)
    // currently just directly applying the function via a static method...

	public void Awake()
	{
		_widget = GetComponent<UIWidget>();
	}

    public void OnEnable()
    {
        //OrbCardProfile.onShroudEvent += HideShroud; 
    }

    public void OnDisable()
    {
        //OrbCardProfile.onShroudEvent -= HideShroud; 
    }

    public void ShowShroud()
    {
        gameObject.SetActive(true);
        //HOTween.To(_widget, 0.5f, new TweenParms().Prop("alpha", 0.5f).Ease(EaseType.EaseOutExpo));
    }

    public void HideShroud()
    {
        //HOTween.To(_widget, 0.3f, new TweenParms().Prop("alpha", 0.5f).Ease(EaseType.EaseOutExpo).OnComplete(HideComplete)); 
    }
    
    public void HideComplete()
    {
        gameObject.SetActive(false); 
    }

    public void OnPress()
    {
        Debug.Log("shroud clicked");
        //OrbCardProfile.ShroudTriggered(); 
    }

}
