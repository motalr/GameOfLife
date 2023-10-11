import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import { Coordinate } from './models/coordinates_model';
import { GridConfig } from './models/grid_config';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'Client';
  readonly url = "http://localhost:5014/api/Game";
  totalRows = 0;
  totalColumns = 0;
  rowRange: number[] = [];
  colRange: number[] = [];
  aliveCells: Coordinate[] = [];
  isGameOngoing = false;

  constructor(private http: HttpClient) {
    this.getGameConfig();
  }

  ngOnInit() {
    console.log("init");
  }

  getGameConfig() {
    console.log("getGameConfig");

    this.http.get<GridConfig>(this.url + '/getConfig').subscribe(data => {
      console.log(data);
      this.totalRows = data.totalRows;
      this.totalColumns = data.totalColumns;

      this.updateGrid();
    });
  }

  isCellAlive(row: number, col: number) {
    return this.aliveCells.some((a) => a.row == row && a.col == col);
  }

  resetTable() {
    this.aliveCells = [];
    this.updateTable();

    this.http.post(this.url + '/stop', this.aliveCells).subscribe(() => {

    });
  }

  updateTable() {
    console.log("updateTable");
    window.location.reload();
  }

  stopGame() {
    console.log("stopGame");
    this.isGameOngoing = false;
    this.aliveCells = [];
  }

  async startGame() {
    console.log("startGame");
    this.isGameOngoing = true;
    await this.initializeCells();

    await this.getNextGeneration().catch(() => {
      console.log("error");
    });
  }

  initializeCells() {
    this.http.post(this.url + '/init', this.aliveCells).subscribe(() => {

    });
  }

  async getNextGeneration() {
    console.log("getNextGeneration");

    if (!this.isGameOngoing) {
      console.log("terminate");
      return;
    }

    await this.http.get<Coordinate[]>(this.url + '/getNextGeneration').subscribe((data) => {
      console.log("data");
      console.log(data);

      if (data.length > 0) {
        console.log("update data")
        this.aliveCells = data;
        this.updateGrid();

        //reiterate
        this.getNextGeneration();
      }
    });
  }

  updateGrid() {
    //Note: setup table
    for (var i = 1; i <= this.totalRows; i++) {
      this.rowRange.push(i);
    }

    for (var i = 1; i <= this.totalColumns; i++) {
      this.colRange.push(i);
    }
  }

  cellSelected(row: number, col: number) {
    var coordinate = new Coordinate();
    coordinate.row = row;
    coordinate.col = col;

    this.aliveCells.push(coordinate);
  }
}
