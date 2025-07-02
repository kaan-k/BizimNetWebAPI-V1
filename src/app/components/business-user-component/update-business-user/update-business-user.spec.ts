import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UpdateBusinessUser } from './update-business-user';

describe('UpdateBusinessUser', () => {
  let component: UpdateBusinessUser;
  let fixture: ComponentFixture<UpdateBusinessUser>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [UpdateBusinessUser]
    })
    .compileComponents();

    fixture = TestBed.createComponent(UpdateBusinessUser);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
