using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace GradeBook
{

	public class Data
	{
		public ScoolData[] dataLst;
	}
	public class ScoolData
	{

		[JsonProperty("schoolList")]
		public SchoolList SchoolList { get; set; }


	}

	public class SchoolList
	{

		[JsonProperty("schoolid")]
		public string SchoolID { get; set; }

		[JsonProperty("schoolName")]
		public string SchoolName { get; set; }

	}
}
