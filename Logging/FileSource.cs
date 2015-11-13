using System;
using System.IO;

namespace BorrehSoft.Utensils.IO
{
	public class FileSource
	{
		DateTime CurrentFiledate {
			get {
				return File.GetLastWriteTime (this.FilePath);
			}
		}

		public bool IsOutdated {
			get {
				return !CurrentFiledate.Equals (LastDate);
			}
		}

		public string FilePath;
		DateTime LastDate = DateTime.MaxValue; // this will absolutely break something
		string CachedText;

		public FileSource (string filePath)
		{
			this.FilePath = filePath;
		}

		public string GetText() {
			if (IsOutdated) {
				this.LastDate = CurrentFiledate;
				this.CachedText = File.ReadAllText (this.FilePath);
			}
			return this.CachedText;
		}
	}
}

