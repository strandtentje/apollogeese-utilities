using System;
using System.IO;

namespace Borrehsoft.Utensils.IO
{
	public class FileSource
	{
		public string FilePath;
		DateTime LastDate = DateTime.MaxValue; // this will absolutely break something
		string CachedText;

		public FileSource (string filePath)
		{
			this.FilePath = filePath;
		}

		public string GetText() {
			DateTime CurrentDate = File.GetLastWriteTime (this.FilePath);
			if (!CurrentDate.Equals (LastDate)) {
				this.LastDate = CurrentDate;
				this.CachedText = File.ReadAllText (this.FilePath);
			}
			return this.CachedText;
		}
	}
}

