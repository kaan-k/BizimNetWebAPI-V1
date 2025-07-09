import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddBusinessUser } from './add-business-user';

describe('AddBusinessUser', () => {
  let component: AddBusinessUser;
  let fixture: ComponentFixture<AddBusinessUser>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AddBusinessUser]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AddBusinessUser);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
