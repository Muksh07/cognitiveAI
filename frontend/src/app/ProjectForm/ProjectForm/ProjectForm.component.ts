import { Component, OnInit } from '@angular/core';
import { ProjectService } from '../../Service/project.service';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-ProjectForm',
  standalone: true,
  imports:[FormsModule,CommonModule],
  templateUrl: './ProjectForm.component.html',
  styleUrls: ['./ProjectForm.component.css']
})
export class ProjectFormComponent implements OnInit 
{

  projectName = '';
  location = '';
  framework = '.NET 8.0';
  applicationType = ''; 
  folderStructure: any = null;
  selectedFileContent: string = '';

  constructor(private projectService: ProjectService) { }

  ngOnInit() {
  }

  onSubmit() {
    const request = {
      projectName: this.projectName,
      location: this.location,
      framework: this.framework,
      applicationType: this.applicationType
    };

    this.projectService.generateProject(request).subscribe(response => {
      this.folderStructure = response.folderStructure;
    }, error => {
      console.error('Error generating project:', error);
    });
  }

  displayFileContent(content: string) {
    this.selectedFileContent = content;
  }

  // formatFolderStructure(structure: any, indent: string = ''): string {
  //   let result = `${indent}${structure.name}\n`;
  //   structure.files.forEach((file: any) => {
  //     result += `${indent}  ${file.name}\n`;
  //   });
  //   structure.folders.forEach((folder: any) => {
  //     result += this.formatFolderStructure(folder, indent + '  ');
  //   });
  //   return result;
  // }

  toggleFolder(folder: any) {
    folder.expanded = !folder.expanded;
  }

}
