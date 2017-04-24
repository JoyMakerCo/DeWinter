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

		public bool LoadFile (string File)
		{
			_filename = File;
			TextAsset file = Resources.Load<TextAsset>(_filename);
			if (file != null)
				JsonConvert.PopulateObject(file.text, this);
			return file != null;
		}

		[OnDeserialized]
		private void OnDeserialized(StreamingContext context)
		{
			OnLoadComplete();
		}

		protected virtual void OnLoadComplete() {}
	}
}