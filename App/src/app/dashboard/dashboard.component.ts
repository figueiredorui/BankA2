import { Component, OnInit, Input } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Observable } from 'rxjs/Observable';

@Component({
  moduleId: module.id,
  selector: 'dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent implements OnInit {

  public accountID: number;

  constructor(
    private route: ActivatedRoute,
  ) { }

  public ngOnInit() {

    this.route.params.subscribe(params => {
      this.accountID = params['id'] || 0;
    });
  }

  
}
