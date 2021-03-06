﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grace.ExampleApp.ReadmeGenerator
{

	public interface IReadmeFileOutput
	{
		
	}

	public class ReadmeFileOutput : IReadmeFileOutput
	{
		private string outputPath;
		private ClassEntry classEntry;
		private DirectoryEntry directoryEntry;

		public ReadmeFileOutput(ClassEntry classEntry, DirectoryEntry directoryEntry, string outputPath)
		{
			this.outputPath = outputPath;
			this.classEntry = classEntry;
			this.directoryEntry = directoryEntry;
		}
	}
}
