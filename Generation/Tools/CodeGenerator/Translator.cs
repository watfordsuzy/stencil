using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace CodeGenerator
{
	public class Translator
	{
		public string DataFile { get; set; }

		public List<Template> Templates { get; set; }

		public string OutputFolder { get; set; }

		public event EventHandler<TranslatorEventArgs> Error;

		public event EventHandler<TranslatorEventArgs> Notice;

		public event EventHandler<TranslatorEventArgs> Progress;

		public Translator()
		{
			Templates = new List<Template>();
		}

		protected virtual void OnError(string errorMessage)
		{
			if (this.Error != null)
			{
				this.Error(this, new TranslatorEventArgs(errorMessage));
			}
		}

		protected virtual void OnNotice(string processMessage)
		{
			if (this.Notice != null)
			{
				this.Notice(this, new TranslatorEventArgs(processMessage));
			}
		}

		protected virtual void OnProgessNotification(decimal percentageComplete)
		{
			if (this.Progress != null)
			{
				this.Progress(this, new TranslatorEventArgs(percentageComplete));
			}
		}

		public void GenFiles()
		{
			OnNotice("Parsing Templates");
			string transformedDoc = Transform();
			OnNotice("Writing Files");
			ParseDocument(transformedDoc);
			OnNotice("Complete");
		}

		protected void ParseDocument(string transformedDoc)
		{
			try
			{
				int num = 0;
				Regex regex = new Regex("'''\\[STARTFILE:(BASE:|)(?<FileName>.*?)\\](?<Data>(.|\\W)*?)'''\\[ENDFILE\\]");
				MatchCollection matchCollection = regex.Matches(transformedDoc);
				OnProgessNotification(num);
				foreach (Match item in matchCollection)
				{
					FileInfo fileInfo = new FileInfo(OutputFolder + "\\" + item.Groups["FileName"].ToString());
					string directoryName = fileInfo.DirectoryName;
					try
					{
						if (!Directory.Exists(directoryName))
						{
							Directory.CreateDirectory(directoryName);
						}
						StreamWriter streamWriter = new StreamWriter(OutputFolder + "\\" + item.Groups["FileName"].ToString());
						streamWriter.Write(HttpUtility.HtmlDecode(item.Groups["Data"].ToString()));
						streamWriter.Close();
						num++;
						OnProgessNotification(decimal.Multiply(decimal.Divide(num, new decimal(matchCollection.Count)), 100m));
					}
					catch (Exception ex)
					{
						OnError(ex.Message + "\rProcessing will continue to the next file.");
					}
				}
				OnProgessNotification(0m);
			}
			catch (Exception ex)
			{
				OnError("Error Loading Document\r\nError: " + ex.Message);
			}
		}

		protected string Transform()
		{
			try
			{
				XslCompiledTransform xslTransform = new XslCompiledTransform();
				StringBuilder stringBuilder = new StringBuilder();
				using StringWriter w = new StringWriter(stringBuilder);
				using XmlTextWriter output = new XmlTextWriter(w);
				XmlResolver resolver = new XmlUrlResolver();
				XPathDocument input = new XPathDocument(DataFile);
				List<Template> list = new List<Template>();
				foreach (Template template in Templates)
				{
					if (template.IsSelected)
					{
						list.Add(template);
					}
				}
				int num = 0;
				OnProgessNotification(num);
				foreach (Template item in list)
				{
					try
					{
						xslTransform.Load(item.Location);
						xslTransform.Transform(input, null, output, resolver);
					}
					catch (Exception ex)
					{
						OnError("Error Transforming Template " + item.Location + ".\rProcessing will continue.\r\nError: " + ex.Message);
					}
					num++;
					OnProgessNotification(decimal.Multiply(decimal.Divide(num, new decimal(list.Count)), 100m));
				}
				return stringBuilder.ToString();
			}
			catch (Exception ex)
			{
				OnError("Error Transforming Templates.\r\nProcessing will stop.\r\nError: " + ex.Message);
				return string.Empty;
			}
		}
	}
}
