using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
	public class LocalizationSvc : IAppService
	{
#if (UNITY_EDITOR)
		private const string GOOGLE_KEY = "https://docs.google.com/spreadsheets/d/{key}/gviz/tq";

#else
#endif
	}
}
