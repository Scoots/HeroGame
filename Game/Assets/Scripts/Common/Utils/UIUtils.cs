using UnityEngine;
using System.Collections;

public class UIUtils  
{
	public static CompType GetChildOfTypeWithName<CompType>(GameObject go, string compName) where CompType : Component
	{
		// if we have a game object
		if (null != go)
		{
			CompType[] subComps = go.GetComponentsInChildren<CompType>(true);
			return System.Array.Find<CompType>(subComps, new System.Predicate<CompType>(x => x.name == compName));
		}

		return null;
	}
}
