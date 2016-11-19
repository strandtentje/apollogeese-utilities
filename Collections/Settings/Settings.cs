using System;
using System.Collections.Generic;
using System.IO;
using BorrehSoft.Utensils.Log;
using BorrehSoft.Utensils.Collections.Maps;

namespace BorrehSoft.Utensils.Collections.Settings
{
	/// <summary>
	/// Settings data structure; stores objects by key. May be
	/// parsed from a file using the <see cref="BorrehSoft.Utensils.Settings.SettingsParser"/>
	/// </summary>
	public class Settings : Map<object>
	{
		public Settings() {}

		public Settings(Map<object> origin) : base(origin){}

		new public Settings Clone() 
		{
			return new Settings (base.Clone());
		}

		public static Settings FromMerge (params Settings[] bases)
		{
			var combinationMap = new CombinedMap<object> (bases);
			var combinedDict = combinationMap.Dictionary;
			var newSettings = new Settings (new Map<object> (combinedDict));

			return newSettings;
		}

		public static Settings FromTuples(IEnumerable<Tuple<string, object>> tuples) {
			var settings = new Settings ();
			foreach (var tuple in tuples) {
				settings [tuple.Item1] = tuple.Item2;
			}
			return settings;
		}

		public FileInfo SourceFile { get; set; }


		public bool GetBool(string id) {
			if (Has (id)) {
				object boolObj = Get (id);
				if (boolObj is bool) {
					return (bool)boolObj;
				}
			}

			throw new MissingSettingException ("", id, "int");
		}

		public bool GetBool (string id, bool otherwise)
		{
			if (base.Has(id))
				return (bool)base[id];

			return otherwise;
		}		

		public Settings GetSubsettings (string name)
		{
			Settings subsettings = this[name] as Settings;

			// if (subsettings == null) Secretary.Report(8, "No subsettings at ", name);

			return subsettings ?? new Settings();
		}		

		public int GetInt(string id) {
			if (Has (id)) {
				object intObj = Get (id);
				if (intObj is int) {
					return (int)intObj;
				}
			}

			throw new MissingSettingException ("", id, "int");
		}

		public decimal GetDecimal(string id, decimal defaultDec) {
			if (Has (id)) {
				object decObj = Get (id);
				if (decObj is decimal) {
					return (decimal)decObj;
				}
			}

			return defaultDec;
		}

        public float GetFloat(string id)
        {
            if (Has(id))
            {
                object floatObj = Get(id);
                if (floatObj is float)
                {
                    return (float)floatObj;
                } 
            }

            throw new MissingSettingException("", id, "float");
        }

		public float GetFloat(string id, float defaultFlt) {
			if (this.Has (id)) {
				if (this [id] is float) {
					return (float)this [id];
				}
			}

			return defaultFlt;
		}

		public List<string> GetStringList(string id, params string[] defaults) {
			if (base.Has (id) && (base[id] is IEnumerable<object>)) {
				IEnumerable<object> list = (IEnumerable<object>)base [id];
				List<string> stringList = new List<string> ();

				foreach (object item in list) 
					stringList.Add ((string)item);

				return stringList;
			} else {
				return new List<string> (defaults);
			}
		}

		public int GetInt(string id, int otherwise)
		{
			int result;
			object resultObject = base [id];

			if (resultObject != null) {
				if (resultObject is int) {
					return (int)resultObject;
				} else if (resultObject is string) {
					if (int.TryParse ((string)resultObject, out result)) {
						return result;
					}
				}
			}

			return otherwise;
		}

		public object Tag {
			get;
			set;
		}

		public bool IsLoaded {
			get;
			set;
		}

		protected override void Delete (string key)
		{
			base.Delete (key);
			IsLoaded = false;
		}

		protected override void Add (string key, object value)
		{
			base.Add (key, value);
			IsLoaded = false;
		}

    }
}

