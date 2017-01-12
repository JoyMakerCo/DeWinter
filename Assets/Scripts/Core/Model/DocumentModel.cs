using System;
using UnityEngine;
using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace Core
{
	public class DocumentModel : IModel
	{
		protected string _filename;

		public DocumentModel (string File=null)
		{
			if (File != null)
				LoadFile(File);
		}

		public void LoadFile (string File)
		{
			_filename = File;
			TextAsset file = Resources.Load<TextAsset>(_filename);
			JsonConvert.PopulateObject(file.text, this);
		}

		[OnDeserialized]
		internal virtual void OnLoadComplete() {}
	}
}