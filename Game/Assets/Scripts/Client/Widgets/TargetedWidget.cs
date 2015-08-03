using UnityEngine;
using System.Collections;
using Holoville.HOTween;
using Client.Managers.Game;
using Common.Data;

public class TargetedWidget : MonoBehaviour 
{
	public Color targetColor = Color.red; 
	private UIWidget _targetSprite; 
	private UIWidget _widget;

	//public GameObject target; 
	public Camera worldCamera; 
	public Camera uiCamera;

	public void Awake()
	{
		_widget = GetComponent<UIWidget>();
	}

	private bool _active = true; 
	public bool Active 
	{
		get { return _active;}
		set { _active = value;}
	}

	private GameObject _target; 
	public GameObject Target 
	{
		get { return _target;}
		set { 
			_target = value;
			worldCamera = NGUITools.FindCameraForLayer(_target.layer);
		}
	}

	private Vector3 _pos; 


	public void Init(UIWidget tSprite )
	{

	}







}
