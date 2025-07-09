import { ComponentFixture, TestBed } from '@angular/core/testing';

import { NavbarBusinessUser } from './navbar-business-user';

describe('NavbarBusinessUser', () => {
  let component: NavbarBusinessUser;
  let fixture: ComponentFixture<NavbarBusinessUser>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [NavbarBusinessUser]
    })
    .compileComponents();

    fixture = TestBed.createComponent(NavbarBusinessUser);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
