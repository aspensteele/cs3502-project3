// FileSystemManager.cs

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq; // For .Select and .ToArray

namespace FileManagementSystem
{
    // Define a simple enum for different file system entry types
    public enum FileSystemEntryType
    {
        File,
        Directory
    }

    // A lightweight struct or class to represent a file/directory entry
    // This avoids tying the UI directly to System.IO.FileSystemInfo
    public class FileSystemEntry
    {
        public string Name { get; set; }
        public string FullPath { get; set; }
        public FileSystemEntryType Type { get; set; }
        public FileAttributes Attributes { get; set; } // Useful for checking hidden, read-only etc.

        // Constructor for convenience
        public FileSystemEntry(string name, string fullPath, FileSystemEntryType type, FileAttributes attributes)
        {
            Name = name;
            FullPath = fullPath;
            Type = type;
            Attributes = attributes;
        }

        // Helper to check if the entry is hidden
        public bool IsHidden => (Attributes & FileAttributes.Hidden) != 0;
    }


    public class FileSystemManager
    {
        /// <summary>
        /// Retrieves child directories and files for a given path.
        /// Handles UnauthorizedAccessException by skipping inaccessible directories.
        /// </summary>
        /// <param name="path">The full path of the directory to list.</param>
        /// <returns>A list of FileSystemEntry objects representing the children.</returns>
        public IEnumerable<FileSystemEntry> GetDirectoryContents(string path)
        {
            var entries = new List<FileSystemEntry>();

            if (!Directory.Exists(path))
            {
                // You might want to throw an exception here or return an empty list with a warning
                Console.WriteLine($"Warning: Directory not found: {path}");
                return entries;
            }

            // Get subdirectories
            try
            {
                foreach (string dirPath in Directory.GetDirectories(path))
                {
                    var info = new DirectoryInfo(dirPath);
                    entries.Add(new FileSystemEntry(
                        info.Name,
                        info.FullName,
                        FileSystemEntryType.Directory,
                        info.Attributes
                    ));
                }
            }
            catch (UnauthorizedAccessException)
            {
                // Silently skip if we can't access, or log it if more detailed error reporting is needed
                Console.WriteLine($"Access denied to directory: {path}");
            }
            catch (Exception ex)
            {
                // Catch other potential errors like PathTooLongException
                Console.Error.WriteLine($"Error listing directories in {path}: {ex.Message}");
            }

            // Get files
            try
            {
                foreach (string filePath in Directory.GetFiles(path))
                {
                    var info = new FileInfo(filePath);
                    entries.Add(new FileSystemEntry(
                        info.Name,
                        info.FullName,
                        FileSystemEntryType.File,
                        info.Attributes
                    ));
                }
            }
            catch (UnauthorizedAccessException)
            {
                Console.WriteLine($"Access denied to files in directory: {path}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error listing files in {path}: {ex.Message}");
            }

            // Sort by type (directories first), then by name
            return entries.OrderBy(e => e.Type).ThenBy(e => e.Name);
        }

        /// <summary>
        /// Creates a new empty file at the specified path.
        /// </summary>
        /// <param name="fullPath">The full path for the new file.</param>
        /// <returns>True if the file was created successfully, false otherwise.</returns>
        public bool CreateFile(string fullPath)
        {
            if (File.Exists(fullPath))
            {
                // This scenario should ideally be checked by the UI before calling this method
                // or handled with a more specific exception/return code.
                throw new IOException("A file with that name already exists.");
            }

            try
            {
                using (File.Create(fullPath)) { } // Create and properly close the file handle
                return true;
            }
            catch (Exception ex)
            {
                // Log the exception for debugging purposes
                Console.Error.WriteLine($"Error creating file {fullPath}: {ex.Message}");
                // Re-throw or return false depending on desired error handling strategy
                throw new IOException($"Failed to create file: {ex.Message}", ex);
            }
        }

        // --- CRUD operations for future implementation ---

        /// <summary>
        /// Creates a new directory.
        /// </summary>
        public bool CreateDirectory(string fullPath)
        {
            if (Directory.Exists(fullPath))
            {
                throw new IOException("A directory with that name already exists.");
            }
            try
            {
                Directory.CreateDirectory(fullPath);
                return true;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error creating directory {fullPath}: {ex.Message}");
                throw new IOException($"Failed to create directory: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Reads the content of a text file.
        /// </summary>
        public string ReadTextFile(string fullPath)
        {
            if (!File.Exists(fullPath))
            {
                throw new FileNotFoundException("File not found.", fullPath);
            }
            try
            {
                return File.ReadAllText(fullPath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error reading file {fullPath}: {ex.Message}");
                throw new IOException($"Failed to read file: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Writes content to a text file.
        /// </summary>
        public bool WriteTextFile(string fullPath, string content)
        {
            try
            {
                File.WriteAllText(fullPath, content);
                return true;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error writing to file {fullPath}: {ex.Message}");
                throw new IOException($"Failed to write to file: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Deletes a file or an empty directory.
        /// </summary>
        public bool DeleteFileSystemEntry(string fullPath, FileSystemEntryType type)
        {
            try
            {
                if (type == FileSystemEntryType.File)
                {
                    if (File.Exists(fullPath))
                    {
                        File.Delete(fullPath);
                        return true;
                    }
                    throw new FileNotFoundException("File not found for deletion.", fullPath);
                }
                else // Directory
                {
                    if (Directory.Exists(fullPath))
                    {
                        if (Directory.EnumerateFileSystemEntries(fullPath).Any())
                        {
                            throw new IOException("Cannot delete non-empty directory. Use DeleteDirectoryRecursive for that.");
                        }
                        Directory.Delete(fullPath);
                        return true;
                    }
                    throw new DirectoryNotFoundException("Directory not found for deletion.");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error deleting {type} {fullPath}: {ex.Message}");
                throw new IOException($"Failed to delete {type}: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Deletes a directory and all its contents recursively.
        /// </summary>
        public bool DeleteDirectoryRecursive(string fullPath)
        {
            if (!Directory.Exists(fullPath))
            {
                throw new DirectoryNotFoundException("Directory not found for recursive deletion.");
            }
            try
            {
                Directory.Delete(fullPath, true); // true for recursive deletion
                return true;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error deleting directory recursively {fullPath}: {ex.Message}");
                throw new IOException($"Failed to delete directory recursively: {ex.Message}", ex);
            }
        }


        /// <summary>
        /// Renames a file or directory.
        /// </summary>
        public bool RenameFileSystemEntry(string oldFullPath, string newFullPath, FileSystemEntryType type)
        {
            if (oldFullPath == newFullPath) return true; // No change needed

            // Check if the target path already exists
            if (File.Exists(newFullPath) || Directory.Exists(newFullPath))
            {
                throw new IOException("A file or directory with the new name already exists.");
            }

            try
            {
                if (type == FileSystemEntryType.File)
                {
                    if (!File.Exists(oldFullPath))
                        throw new FileNotFoundException("Original file not found for rename.", oldFullPath);
                    File.Move(oldFullPath, newFullPath);
                }
                else // Directory
                {
                    if (!Directory.Exists(oldFullPath))
                        throw new DirectoryNotFoundException("Original directory not found for rename.");
                    Directory.Move(oldFullPath, newFullPath);
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error renaming {type} from {oldFullPath} to {newFullPath}: {ex.Message}");
                throw new IOException($"Failed to rename {type}: {ex.Message}", ex);
            }
        }

        // Helper to check if a path is a directory
        public bool IsDirectory(string path)
        {
            // Check if it exists and is a directory
            return Directory.Exists(path) && (File.GetAttributes(path) & FileAttributes.Directory) == FileAttributes.Directory;
        }
    }
}