using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ServiceLocator
{
	public static IResourceService Resource => ResourceService.Instance;

}
