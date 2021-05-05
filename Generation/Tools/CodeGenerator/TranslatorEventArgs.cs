using System;

namespace CodeGenerator
{
	public class TranslatorEventArgs : EventArgs
	{
		public string Message { get; set; }

		public decimal Progress { get; set; }

		public TranslatorEventArgs()
		{
		}

		public TranslatorEventArgs(string message)
		{
			Message = message;
		}

		public TranslatorEventArgs(decimal progress)
		{
			Progress = progress;
		}
	}
}
