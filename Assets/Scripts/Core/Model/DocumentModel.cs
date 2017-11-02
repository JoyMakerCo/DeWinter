using System;
using UnityEngine;
using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace Core
{
	public class DocumentModel : IModel
	{
		private TextAsset _file;

		public DocumentModel (string filename)
		{
			if (filename != null) LoadFile(filename);
		}

		public bool LoadFile (string filename)
		{
			_file = Resources.Load<TextAsset>(filename);
			if (_file != null)
				JsonConvert.PopulateObject(_file.text, this);
			return _file != null;
		}

		[OnDeserialized]
		private void OnDeserialized(StreamingContext context)
		{
			if (_file != null)
				Resources.UnloadAsset(_file);
			_file = null;
			OnLoadComplete();
		}

		protected virtual void OnLoadComplete() {}
	}
}
