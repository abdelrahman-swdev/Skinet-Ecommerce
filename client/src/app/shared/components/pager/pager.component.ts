import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';

@Component({
  selector: 'app-pager',
  templateUrl: './pager.component.html',
  styleUrls: ['./pager.component.scss']
})
export class PagerComponent {
  @Input() totalCount:number;
  @Input() pageSize:number;
  @Input() pageNumber: number;
  @Output() pageChanged = new EventEmitter<number>();

  constructor() { }

  onPagerChanged(event:any)
  {
    this.pageChanged.emit(event.page);
  }
}
