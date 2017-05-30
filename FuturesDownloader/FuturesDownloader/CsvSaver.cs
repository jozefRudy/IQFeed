using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FuturesDownloader
{
    class CsvSaver
    {
        string filename { get; set; }
        string dir { get; set; }
        List<BarItem> bars = new List<BarItem>();

        public CsvSaver(string path, List<BarItem> bars)
        {            
            this.bars = bars;
            this.ConstructNames(path);
        }

        void ConstructNames(string path)
        {
            string[] separated_path = path.Split(new char[] { Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar });
            this.filename = CleanName(separated_path.Last(), Path.GetInvalidFileNameChars());
            this.dir = CleanName(separated_path.First(), Path.GetInvalidPathChars());
        }

        static string CleanName(string filename, char[] invalid)
        {            
            return invalid.Aggregate(filename, (current, c) => current.Replace(c.ToString(), string.Empty));
        }

        public void Save()
        {
            if (bars.Count() == 0)
                return;

            Directory.CreateDirectory(this.dir);

            using (StreamWriter writer = new StreamWriter(Path.Combine(this.dir, this.filename)))
            {
                writer.WriteLine("Date,Open,High,Low,Close,Volume");
                foreach (var bar in this.bars)
                {                    
                    writer.WriteLine($"{bar.date.ToString("dd/MM/yyyy HH:mm:ss")},{bar.open},{bar.high},{bar.low},{bar.close},{bar.volume}");
                }                
            }
        }
    }
}
