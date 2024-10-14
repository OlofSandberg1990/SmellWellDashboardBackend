import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-sheets',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './sheets.component.html',
  styleUrl: './sheets.component.css'
})
export class SheetsComponent implements OnInit {
  data: any;
  constructor(private http: HttpClient) { }

  ngOnInit(): void {
    this.http.get('https://localhost:7050/KWRANKING') // Replace this URL with your own
      .subscribe({
        next: (data) => {
          this.data = data; // Process your data here
        },
        error: (error) => {
          console.error('Error fetching data', error); // Handle errors here
        }
      });
  }
}
