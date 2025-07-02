import { CommonModule, isPlatformBrowser } from '@angular/common';
import { Component, Inject, PLATFORM_ID, OnInit } from '@angular/core';
import { MatDialog, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { GridOptions, ColDef, ColGroupDef, CellClickedEvent, SideBarDef, GridReadyEvent } from 'ag-grid-community';
import { BusinessUserDto } from '../../models/businessUser/businessUserDto';
import { BusinessUserComponentService } from '../../services/component/business-user-component-service';
import { MatButtonModule } from '@angular/material/button';
import { AgGridAngular } from 'ag-grid-angular';

@Component({
  selector: 'app-business-user-component',
  standalone: true,
  imports: [CommonModule,AgGridAngular],
  templateUrl: './business-user-component.html',
  styleUrl: './business-user-component.css'
})
export class BusinessUserComponent implements OnInit {
  isBrowser = false;
  agGridLoaded = false;

  userId: string = '';
  userDeleteId: boolean = false;
  user: BusinessUserDto = {}as BusinessUserDto;

  constructor(
    private businessUserComponentService: BusinessUserComponentService,
    private dialog: MatDialog,
    @Inject(PLATFORM_ID) private platformId: Object
  ) {
    this.isBrowser = isPlatformBrowser(this.platformId);
  }

  async ngOnInit() {
    if (this.isBrowser) {
      await import('ag-grid-angular');
      this.agGridLoaded = true;
    }
  }

  protected gridOptions: GridOptions = {
    pagination: true,
    paginationPageSize: 50,
  };

  public columnDefs: (ColDef | ColGroupDef)[] = [
    { field: 'email', headerName: "Email", unSortIcon: true },
    { field: 'companyName', headerName: "Şirket Adı", unSortIcon: true },
    { field: 'companyAddress', headerName: "Şirket Adresi", unSortIcon: true },
    {
      field: 'Delete', headerName: "Sil", filter: false, valueGetter: () => 'Delete',
      cellRenderer: () => `<i class="fa-solid fa-trash-can" style="cursor:pointer;opacity:0.7; font-size:20px;"></i>`,
      onCellClicked: (event: CellClickedEvent) => this.onDeleteUserId(event.data.id),
    },
    {
      field: 'Update', headerName: "Güncelle", filter: false, valueGetter: () => 'Update',
      cellRenderer: () => `<i class="fa-solid fa-pen" style="cursor:pointer;opacity:0.7; font-size:20px;" data-bs-toggle="modal" data-bs-target="#updateModal"></i>`,
      onCellClicked: (event: CellClickedEvent) => this.getByUser(event.data.id),
    },
  ];

  public rowSelection = 'multiple';
  public defaultColDef: ColDef = {
    flex: 1,
    filter: true,
    sortable: true,
    resizable: true,
    floatingFilter: true,
    minWidth: 130,
  };

  public rowBuffer = 0;
  public rowModelType: 'clientSide' | 'infinite' | 'viewport' | 'serverSide' = 'infinite';
  public cacheBlockSize = 300;
  public cacheOverflowSize = 2;
  public maxConcurrentDatasourceRequests = 1;
  public infiniteInitialRowCount = 1000;
  public maxBlocksInCache = 10;
  public noRowsTemplate: any;
  public rowData!: any[];
  public sideBar: SideBarDef | string | string[] | boolean | null = {
    toolPanels: ['columns', 'filters'],
    defaultToolPanel: '',
  };

  onGridReady(params: GridReadyEvent) {
    params.api.sizeColumnsToFit();
    this.getAllUser();
  }

  async getAllUser() {
    const response = await this.businessUserComponentService.getAllUser();
    this.rowData = response;
  }

  async getByUser(id: string) {
    this.user = await this.businessUserComponentService.getById(id);
  }

  onDeleteUserId(id: string) {
    this.userId = id;
    this.userDeleteId = true;
    this.deleteUser(id);
  }

  deleteUser(id: string) {
    if (this.userDeleteId) {
      this.openDialog().afterClosed().subscribe(result => {
        if (!result) return;
        this.businessUserComponentService.deleteUser(id, () => this.getAllUser());
      });
    }
  }

  openDialog() {
    return this.dialog.open(UserDeleteTemplate, {
      width: '550px',
      panelClass: 'matdialog-delete',
    });
  }
}

@Component({
  selector: 'user-delete-template',
  standalone: true,
  template: `
    <h5 mat-dialog-title>Silmek İstediğinize Emin Misiniz</h5>
    <div mat-dialog-content></div>
    <div mat-dialog-actions class="mat-mdc-dialog-actions">
      <button class="button-4" mat-button [mat-dialog-close]="false">
        <i class="fa-solid fa-circle-xmark"></i> İptal
      </button>
      <button class="button-24" mat-button [mat-dialog-close]="true" cdkFocusInitial>
        <i class="fa-solid fa-trash-can"></i> Sil
      </button>
    </div>
  `,
  imports: [MatDialogModule, MatButtonModule, CommonModule]
})
export class UserDeleteTemplate {
  constructor(public dialogRef: MatDialogRef<UserDeleteTemplate>) {}
}
