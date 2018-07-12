using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Linq;
using System.Diagnostics;
using Ionic.Zip;

namespace DVRTools.Services
{
    public class IOManager : IIOManager
    {
        IFileNameManager fileNameService;

        public IOManager(IFileNameManager fileNameService)
        {
            this.fileNameService = fileNameService;
        }

        public string OpenFileDialog()
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "PDFs| *.pdf|Word Documents |*.docx; *.doc;|Excel Document|*.xls; *.xls|All files (*.*)|*.*"
            };

            bool? result = openFileDialog.ShowDialog();

            string filepath = String.Empty;

            if (result == true)
                filepath = openFileDialog.FileName;

            return filepath;
        }

        public string OpenFolder()
        {
            string folderPath;

            using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                //Investigate alternative way to achieve this
                System.Windows.Forms.DialogResult result = dialog.ShowDialog();
                folderPath = dialog.SelectedPath;
            }
            return folderPath;
        }

        public void ZipDirectory(string inputLocation, string outputLocation)
        {
            string fileName = fileNameService.GenerateRandomFileName();

            try
            {
                using (ZipFile zip = new ZipFile())
                {
                    zip.AddDirectory(inputLocation);
                    zip.Save($@"{outputLocation}\{fileName}.zip");
                }
            }

            catch (DirectoryNotFoundException ex)
            {
                using (EventLog eventLog = new EventLog("Application"))
                {
                    eventLog.Source = "Application";
                    eventLog.WriteEntry($@"Could not find directory {outputLocation}\{fileName}.zip /n Stack trace {ex}", EventLogEntryType.FailureAudit, 101, 1);
                }

            }

            catch (IOException ex)
            {
                using (EventLog eventLog = new EventLog("Application"))
                {
                    eventLog.Source = "Application";
                    eventLog.WriteEntry($@"An IO Exception occured when trying to write file: {outputLocation}\{fileName}.zip /n Stack trace {ex}", EventLogEntryType.FailureAudit, 101, 1);
                }
            }

            catch (AccessViolationException ex)
            {
                using (EventLog eventLog = new EventLog("Application"))
                {
                    eventLog.Source = "Application";
                    eventLog.WriteEntry($@"The location {outputLocation}\{fileName}.zip is protected. The current user does not have the access rights to write to the location. /n Stack trace {ex}", EventLogEntryType.FailureAudit, 101, 1);
                }
            }


            OpenZipFile($@"{outputLocation}\{fileName}.zip");

        }

        private void OpenZipFile(string path)
        {
            try
            {
                if (path.EndsWith(".zip"))
                    System.Diagnostics.Process.Start(path);
                else
                    throw new Exception("File is not of type .zip");
            }
            catch (Exception ex)
            {
                using (EventLog eventLog = new EventLog("Application"))
                {
                    eventLog.Source = "Application";
                    eventLog.WriteEntry($@"Cannot Open Zip File {path} /n Stack trace {ex}", EventLogEntryType.FailureAudit, 101, 1);
                }
            }
        }

        public void CreateDirectory(string path)
        {
            try
            {
                Directory.CreateDirectory(path);
            }

            catch (UnauthorizedAccessException ex)
            {
                using (EventLog eventLog = new EventLog("Application"))
                {
                    eventLog.Source = "Application";
                    eventLog.WriteEntry($@"Tried creating Directory within {path}. Access was denied. /n Stack trace {ex}", EventLogEntryType.FailureAudit, 101, 1);
                }
            }

            catch (PathTooLongException ex)
            {
                using (EventLog eventLog = new EventLog("Application"))
                {
                    eventLog.Source = "Application";
                    eventLog.WriteEntry($@"Tried creating Directory within {path}. The filepath was too long. /n Stack trace {ex}", EventLogEntryType.FailureAudit, 101, 1);
                }

            }

            catch (DirectoryNotFoundException ex)
            {
                using (EventLog eventLog = new EventLog("Application"))
                {
                    eventLog.Source = "Application";
                    eventLog.WriteEntry($@"Tried creating Directory within {path}. The directory could not be found. /n Stack trace {ex}", EventLogEntryType.FailureAudit, 101, 1);
                }
            }

            catch(Exception ex)
            {
                using (EventLog eventLog = new EventLog("Application"))
                {
                    eventLog.Source = "Application";
                    eventLog.WriteEntry($@"Tried creating Directory within {path}. The following exception occured:. /n Stack trace {ex}", EventLogEntryType.FailureAudit, 101, 1);
                }
            }

        }

        public void DeleteDirectory(string path)
        {

            List<string> files = Directory.EnumerateFiles(path).ToList();

            foreach (var file in files)
            {

                try
                {
                    File.Delete(file);
                }

                catch (UnauthorizedAccessException ex)
                {
                    using (EventLog eventLog = new EventLog("Application"))
                    {
                        eventLog.Source = "Application";
                        eventLog.WriteEntry($@"Tried deleting {file}. The curent user does not have access rights to delete the file. /n Stack trace {ex}", EventLogEntryType.FailureAudit, 101, 1);
                    }
                }

               
            }

            Directory.Delete(path);
        }

        public void CopyFileToDirectory(string path, string outputPath)
        {
            int pos = path.LastIndexOf(@"\") + 1;
            string tempPath = path.Substring(pos, path.Length - pos);
            
            try
            {
                File.Copy(path, $@"{outputPath}/{tempPath}");
            }

            catch (UnauthorizedAccessException ex)
            {
                using (EventLog eventLog = new EventLog("Application"))
                {
                    eventLog.Source = "Application";
                    eventLog.WriteEntry($@"Tried copying {tempPath} to {outputPath}. The curent user does not have access rights to access this folder or file. /n Stack trace {ex}", EventLogEntryType.FailureAudit, 101, 1);
                }
            }

        }
    }
}
