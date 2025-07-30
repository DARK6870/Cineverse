import { ComponentFixture, TestBed } from '@angular/core/testing';
import { TestGraphqlComponent } from './test-graphql';
import { GraphqlService } from '../services/graphql';
import { of } from 'rxjs';

import 'jasmine';

describe('TestGraphqlComponent', () => {
  let component: TestGraphqlComponent;
  let fixture: ComponentFixture<TestGraphqlComponent>;
  let mockGraphqlService: jasmine.SpyObj<GraphqlService>;

  beforeEach(async () => {
    // Create a spy object with the testQuery method
    mockGraphqlService = jasmine.createSpyObj('GraphqlService', ['testQuery']);
    mockGraphqlService.testQuery.and.returnValue(of({
      data: {
        __schema: {
          types: []
        }
      }
    }));

    await TestBed.configureTestingModule({
      imports: [TestGraphqlComponent],
      providers: [
        { provide: GraphqlService, useValue: mockGraphqlService }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(TestGraphqlComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should call testQuery when button is clicked', () => {
    const button = fixture.nativeElement.querySelector('button');
    button.click();
    expect(mockGraphqlService.testQuery).toHaveBeenCalled();
  });
});
