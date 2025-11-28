# File Management System - CS 3502 Project 3

## Build Instructions

### Requirements
- Windows 10/11
- .NET Framework 4.7.2 or higher (or .NET 6.0+)
- Visual Studio 2019 or newer

### Steps to Build and Run
1. Open `FileManagementSystem.sln` in Visual Studio
2. Restore NuGet packages (automatic)
3. Build the solution (F6 or Build → Build Solution)
4. Run the application (F5 or Debug → Start Debugging)

### Dependencies
- System.Windows.Forms (built-in)
- System.IO (built-in)
- System.Drawing (built-in)

## Usage
- Click **Open** to browse to a different root folder
- Click **Create** to create new files or directories
- Select items in the TreeView to view/edit content
- Use **Copy/Cut/Paste** for file operations
- Click **Save** after editing text files

## Features
- CRUD operations (Create, Read, Update, Delete)
- Directory navigation with lazy loading
- Image preview (.jpg, .png, .gif, .bmp, .ico)
- Text file editing
- Copy/Cut/Paste functionality
- Error handling with user-friendly messages