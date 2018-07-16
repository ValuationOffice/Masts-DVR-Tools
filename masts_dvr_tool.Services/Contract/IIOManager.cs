using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVRTools.Services
{
    public interface IIOManager
    {
        /// <summary>
        /// Receive file path directory browser
        /// </summary>
        /// <returns>File path as string</returns>
        string OpenFileDialog();
        /// <summary>
        /// Receive a directory path from directory browser
        /// </summary>
        /// <returns>Folder path as strng</returns>
        string OpenFolder();
        /// <summary>
        /// Zips a directory
        /// </summary>
        /// <param name="inputLocation">The location of a Directory to Zip</param>
        /// <param name="outputLocation">The location where the Zip file should be put</param>
        void ZipDirectory(string inputLocation, string outputLocation, string password = "");
        /// <summary>
        /// Creates a new directory at the specified path
        /// </summary>
        /// <param name="path">Location within file system where directory will be created.</param>
        void CreateDirectory(string path);
        /// <summary>
        /// Deletes a directory from the file system.
        /// </summary>
        /// <param name="path">Path to the directory.</param>
        void DeleteDirectory(string path);
        /// <summary>
        /// Copy a file from one location to another
        /// </summary>
        /// <param name="path">Path of original file</param>
        /// <param name="outputPath">Location where file will be copied to</param>
        void CopyFileToDirectory(string path, string outputPath);
        /// <summary>
        /// Creates a file in the specified directory
        /// </summary>
        /// <param name="path">File name and location</param>
        void CreateFile(string path);
        /// <summary>
        /// Check if file Exists
        /// </summary>
        /// <param name="path">File name</param>
        /// <returns></returns>
        bool FileExists(string path);
        /// <summary>
        /// Open a file of type CSV with the default application
        /// </summary>
        /// <param name="path">CSV file path</param>
        void OpenCSVFile(string path);
    }
}
