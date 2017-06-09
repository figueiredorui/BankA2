import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { TransactionsComponent } from './dashboard/transactions/transactions.component';
import { FilesComponent } from './dashboard/files/files.component';
import { TagListComponent } from './tags/tag-list/tag-list.component';

const routes: Routes = [
  { path: '', redirectTo: 'account/1/dashboard', pathMatch: 'full' },
  { path: 'tags', component: TagListComponent, },
  {
    path: 'account',
    //component: DashboardComponent,
    children: [
      {
        path: '',
        children: [
          { path: ':id/dashboard', component: TransactionsComponent, data : {name : 'dashboard'} },
          { path: ':id/files', component: FilesComponent, data : {name : 'files'} },
        ],
      }
    ]
  }

];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
