using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TouchDirWithFileDate.Commands.Settings;

namespace TouchDirWithFileDate.Commands;

public class TouchDirectory
{

    private readonly ConsoleSettings settings;

    public TouchDirectory(ConsoleSettings settings)
    {
        this.settings = settings;
    }

    public void Execute()
    {
        TouchDirWithFileDate(settings.Directory, settings.Recursive);
    }

    public void TouchDirWithFileDate(string rootDir, bool recursive)
    {
        if (Directory.Exists(rootDir))
        {
            DirectoryInfo di = new DirectoryInfo(rootDir);
            TouchDir(di, recursive, rootDir);

            if (Environment.ExitCode > 0)
                Console.WriteLine("Erors occured");
        }
        else
        {
            Console.WriteLine($"Directory {rootDir} doesn't exist!");
        }
    }


    private void TouchDir(DirectoryInfo dir, bool recursive, string rootDir)
    {
        try
        {

            //recurse call ABOVE, will turn into depth first instead of bredth first
            //begin at innermost dir and work outwards, so outer dirs will get correct updated dates from inner subdirs

            if (recursive)
            {
                foreach (DirectoryInfo subDir in dir.GetDirectories())
                {
                    TouchDir(subDir, recursive, rootDir);
                }
            }


            //Update the CreationTime, LastWriteTime and LastAccessTime
            DateTime medianDate = GetMedianFileAndDirectoryDate(dir);

            //full dir name except root dir
            string dirName = dir.FullName.Replace(rootDir, "");
            if (dirName.Trim() == string.Empty)
                dirName = $@"\ (ROOT DIR {rootDir})";

            if (dir.LastWriteTime != medianDate)
            {
                Console.WriteLine($"Touching with datetime {medianDate:yyyy-MM-dd HH:mm:ss}: {dirName}");
                //don't set creation date, or last access date
                dir.LastWriteTime = medianDate;
            }
            else
            {
                Console.WriteLine($"Already correct date   {medianDate:yyyy-MM-dd HH:mm:ss}: {dirName}");
            }


        }
        catch (IOException e)
        {
            if (e.Message.Contains("used by another process"))
            {
                Console.WriteLine("ERROR: {0}", e.Message);
                Console.WriteLine("If you have this directory open in Windows, close it and try again");
                Environment.ExitCode = 1;
            }
            else
            {
                Console.WriteLine("ERROR: {0}", e.Message);
                Environment.ExitCode = 2;
            }
        }
        catch (UnauthorizedAccessException e)
        {
            Console.WriteLine("ERROR: {0}", e.Message);
            Console.WriteLine("Your current user doesn't have access, skipping");
            Environment.ExitCode = 1;
        }
        catch (Exception e)
        {
            Console.WriteLine("ERROR: {0}", e.Message);
            Environment.ExitCode = 2;
        }

    }

    private DateTime GetMedianFileAndDirectoryDate(DirectoryInfo dir)
    {

        FileInfo[] allFiles = dir.GetFiles();
        DirectoryInfo[] allDirs = dir.GetDirectories();

        if (allFiles.Any() || allDirs.Any())
        {

            //get all modified date ticks on all files and dirs
            //remove minutes and seconds
            List<decimal> allTicks = new List<decimal>(allFiles.Length + allDirs.Length);
            allTicks.AddRange(allFiles.Select(file => (decimal)new DateTime(file.LastWriteTime.Year, file.LastWriteTime.Month, file.LastWriteTime.Day, file.LastWriteTime.Hour, 0, 0).Ticks).ToList());
            allTicks.AddRange(allDirs.Select(d => (decimal)new DateTime(d.LastWriteTime.Year, d.LastWriteTime.Month, d.LastWriteTime.Day, d.LastWriteTime.Hour, 0, 0).Ticks).ToList());
            //using the median is more reasonable since average might give a date that NO file actually has
            long medianTick = (long)allTicks.Median();
            return new DateTime(medianTick);
        }
        else
        {
            //no files exist, return dir date
            return dir.LastWriteTime;
        }
    }





}