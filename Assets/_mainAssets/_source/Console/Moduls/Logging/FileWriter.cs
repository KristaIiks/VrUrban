using System.IO;
using System.Text;

namespace SmartConsole
{
	public class FileWriter
	{
		public readonly string FilePath;

		public FileWriter(string path)
		{
			FilePath = path;
			FileStream fs = new FileStream(FilePath, FileMode.OpenOrCreate, FileAccess.Write);
			fs.Close();
		}

		public FileWriter(string path, string fileName)
		{
			FilePath = string.Format(@"{0}/{1}.txt", path, fileName);
			FileStream fs = new FileStream(FilePath, FileMode.CreateNew, FileAccess.Write);
			fs.Close();
		}
		
		public void Print(string text)
		{
			using (StreamWriter writer = new StreamWriter(FilePath, true, Encoding.UTF8))
			{
				writer.WriteLine(text);
			}
		}
		
		public void RemoveText(int length)
		{
			if (length <= 0) { return; }
			
			using (FileStream fs = new FileStream(FilePath, FileMode.Open, FileAccess.Write))
			{
				fs.SetLength(fs.Length - (length + 2));
			}
		}
	}
}
