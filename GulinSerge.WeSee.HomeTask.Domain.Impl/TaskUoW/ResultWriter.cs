using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;

namespace GulinSerge.WeSee.HomeTask.Domain.Impl.TaskUoW
{
	public class ResultWriter
	{
		private readonly List<FileTag> _tags = new List<FileTag>();
		private const string _ext = ".wrp";

		public void WriteResult(WorkerResult result)
		{
			string fileName = Guid.NewGuid() + _ext;
			_tags.Add(new FileTag(result, fileName));
			Serialize(fileName, result);
		}

		public IEnumerable<ulong> GetResult()
		{
			foreach (ulong value in _tags
				.OrderBy(x => x.Result.MainTask.From)
				.SelectMany(fileTag => DeSerialize(fileTag.FileName)))
			{
				yield return value;
			}

			DeleteFiles();
		}

		private static void DeleteFiles()
		{
			foreach (string file in Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*" + _ext))
			{
				File.Delete(file);
			}
		}

		private static void Serialize(string filename, WorkerResult result)
		{
			Stream stream = File.Open(filename, FileMode.Create);
			BinaryFormatter bFormatter = new BinaryFormatter();
			bFormatter.Serialize(stream, result.Primes);
			stream.Close();
		}

		private static IEnumerable<ulong> DeSerialize(string filename)
		{
			Stream stream = File.Open(filename, FileMode.Open);
			BinaryFormatter bFormatter = new BinaryFormatter();
			IEnumerable<ulong> result = (IEnumerable<ulong>)bFormatter.Deserialize(stream);
			stream.Close();
			return result;
		}

		
	}
}