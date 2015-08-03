using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class PrefabHelper : MonoBehaviour {
	[SerializeField]
	private GameObject prefab;
	public GameObject Prefab { get { return prefab; } }

	void Awake()
	{
		Refresh();
	}

	public void Clean()
	{
		int childCount = transform.childCount;
		for (int i = childCount - 1; i >= 0; --i)
		{
			Object.DestroyImmediate(transform.GetChild(i).gameObject);
		}
	}

	[ContextMenu("Refresh")]
	public GameObject Refresh()
	{
#if UNITY_EDITOR
		Clean();
#endif

		if (prefab != null)
		{
			GameObject proxyObject = Object.Instantiate(prefab, transform.position, transform.rotation) as GameObject;
			proxyObject.name = name;
			proxyObject.SetActive(gameObject.activeSelf);

			CopyWidgetData(proxyObject);

#if UNITY_EDITOR
			if (!Application.isPlaying)
			{
				foreach (var item in proxyObject.GetComponentsInChildren<Transform>())
				{
					item.gameObject.hideFlags = HideFlags.NotEditable;
				}

				proxyObject.transform.parent = transform;
				proxyObject.transform.localScale = Vector3.one;
				proxyObject.transform.localPosition = Vector3.one;
			}
			else
#endif
			{
				proxyObject.transform.parent = transform.parent;
				proxyObject.transform.localScale = transform.localScale;
				proxyObject.transform.localPosition = transform.localPosition;

				Object.DestroyImmediate(gameObject);
			}

			return proxyObject;
		}

		return null;
	}

	private void CopyWidgetData(GameObject proxyObject)
	{
		UIWidget source = GetComponent<UIWidget>();
		if (source != null)
		{
			UIWidget dest = proxyObject.GetComponent<UIWidget>();

			if (dest != null)
			{
#if UNITY_EDITOR
				if (Application.isPlaying)
				{
					dest.alpha = source.alpha;
				}
#endif
				RecursivelySetDepth(dest, source.depth);

				dest.width = source.width;
				dest.height = source.height;
				dest.keepAspectRatio = source.keepAspectRatio;
				dest.aspectRatio = source.aspectRatio;

				var postionBeforePivot = dest.transform.localPosition;
				dest.pivot = source.pivot;
				dest.transform.localPosition = postionBeforePivot;

				if (dest.autoResizeBoxCollider)
				{
					dest.ResizeCollider();
				}

				CopyAnchor(source.leftAnchor, dest.leftAnchor);
				CopyAnchor(source.rightAnchor, dest.rightAnchor);
				CopyAnchor(source.topAnchor, dest.topAnchor);
				CopyAnchor(source.bottomAnchor, dest.bottomAnchor);
			}

			var collider = proxyObject.GetComponent<BoxCollider>();
			if (collider != null)
			{
				collider.size = new Vector3(source.width, source.height, 1);
			}
		}
	}

	private void CopyAnchor(UIRect.AnchorPoint source, UIRect.AnchorPoint dest)
	{
		dest.target = source.target;
		dest.relative = source.relative;
		dest.absolute = source.absolute;
	}

	private void RecursivelySetDepth(UIWidget widget, int depth)
	{
		widget.depth = depth;
		for (int i = 0; i < widget.transform.childCount; i++)
		{
			var childTransform = widget.transform.GetChild(i);
			UIWidget childWidget = childTransform.GetComponent<UIWidget>();
			if (childWidget != null)
			{
				RecursivelySetDepth(childWidget, childWidget.depth + depth);
			}
		}
	}
}

[System.AttributeUsage(System.AttributeTargets.Field)]
public class PrefabOverride : System.Attribute
{ }
