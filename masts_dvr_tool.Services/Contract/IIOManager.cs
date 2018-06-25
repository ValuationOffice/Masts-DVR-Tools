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
        void ZipDirectory(string inputLocation, string outputLocation);
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
    }
}
