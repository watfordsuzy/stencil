using System;
using System.Collections.Generic;

namespace CodeGenerator
{
	[Serializable]
	public class Options
	{
		public string OutputFolder { get; set; }

		public string DataFile { get; set; }

		public List<string> SelectedFiles { get; set; }

		public List<string> UnSelectedFiles { get; set; }

		public Options()
		{
			SelectedFiles = new List<string>();
			UnSelectedFiles = new List<string>();
		}
	}
}
