using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectController : ControllerBase
    {
        [HttpPost]
        [Route("generate")]
        public IActionResult GenerateProject([FromBody] ProjectRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.ProjectName) || string.IsNullOrWhiteSpace(request.Location) || string.IsNullOrWhiteSpace(request.Framework) || string.IsNullOrWhiteSpace(request.ApplicationType))
            {
                return BadRequest("Invalid input");
            }

            string projectPath = Path.Combine(request.Location, request.ProjectName);
            if (!Directory.Exists(projectPath))
            {
                Directory.CreateDirectory(projectPath);
            }

            var processInfo = new ProcessStartInfo("dotnet", $"new {request.ApplicationType} -o {projectPath}")
            {
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (var process = Process.Start(processInfo))
            {
                process.WaitForExit();
                var output = process.StandardOutput.ReadToEnd();
                var error = process.StandardError.ReadToEnd();
                if (process.ExitCode != 0)
                {
                    return BadRequest($"Failed to create project: {error}");
                }
            }

            var folderStructure = GetFolderStructure(projectPath);
            return Ok(new { path = projectPath, folderStructure });
        }

        // private object GetFolderStructure(string path)
        // {
        //     var directoryInfo = new DirectoryInfo(path);
        //     return GetDirectoryStructure(directoryInfo);
        // }

        // private object GetDirectoryStructure(DirectoryInfo directoryInfo)
        // {
        //     return new
        //     {
        //         Name = directoryInfo.Name,
        //         Files = directoryInfo.GetFiles().Select(file => file.Name).ToList(),
        //         Folders = directoryInfo.GetDirectories().Select(GetDirectoryStructure).ToList()
        //     };
        // }
        private object GetFolderStructure(string path)
        {
            var directoryInfo = new DirectoryInfo(path);
            return GetDirectoryStructure(directoryInfo);
        }


        private object GetDirectoryStructure(DirectoryInfo directoryInfo)
        {
            return new
            {
                name = directoryInfo.Name,
                files = directoryInfo.GetFiles().Select(file => new { name = file.Name, content = System.IO.File.ReadAllText(file.FullName) }).ToList(),
                folders = directoryInfo.GetDirectories().Select(GetDirectoryStructure).ToList()
            };
        }

    }
}
