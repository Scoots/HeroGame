using UnityEngine;
using System.Collections;
using Holoville.HOTween;
using Client.Managers.Game;
using Common.Data;

public class CardProfileBackgroundWidget : MonoBehaviour
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

    void OnPress(bool isPressed)
    {
        Debug.Log("shroud clicked");
    }







}
