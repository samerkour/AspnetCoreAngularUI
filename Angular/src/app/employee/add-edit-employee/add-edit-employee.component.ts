import { Component, Input, OnInit } from '@angular/core';
import { ApiserviceService } from 'src/app/apiservice.service';

@Component({
  selector: 'app-add-edit-employee',
  templateUrl: './add-edit-employee.component.html',
  styleUrls: ['./add-edit-employee.component.css']
})
export class AddEditEmployeeComponent implements OnInit {

  EmployeeList: any = [];
  constructor(private service: ApiserviceService<any>) { }
  @Input() emp: any;


  Id = "0";
  FirstName = "";
  LastName = "";
  Age = "";
  BirthDate = "";
  PhoneNumber = "";
  Email = "";
  AccountNumber = "";


  ngOnInit(): void 
  {
    this.loadEmployeeList();
   
  }

  loadEmployeeList() {


    this.Id = this.emp.id;
    this.FirstName = this.emp.firstName;
    this.LastName = this.emp.lastName;
    this.Age = this.emp.age;
    this.BirthDate = this.emp.birthDate;
    this.PhoneNumber = this.emp.phoneNumber;
    this.Email=this.emp.email;
    this.AccountNumber = this.emp.accountNumber;

    // this.service.getEmployeeList().subscribe(data => {
    //   this.EmployeeList = data.result;

    //   this.Id = this.emp.id;
    //   this.FirstName = this.emp.firstName;
    //   this.LastName = this.emp.lastName;
    //   this.Age = this.emp.age;
    //   this.BirthDate = this.emp.birthDate;
    //   this.PhoneNumber = this.emp.phoneNumber;
    //   this.Email=this.emp.email;
    //   this.AccountNumber = this.emp.accountNumber;

    // });
  }

  addEmployee() {
    var val = {
      //Id: this.Id,
      firstName: this.FirstName,
      lastName: this.LastName,
      age: this.Age,
      birthDate: this.BirthDate,
      phoneNumber: this.PhoneNumber,
      email: this.Email,
      accountNumber: this.AccountNumber
    };

    this.service.addEmployee(val).subscribe(res => 
    {
      alert(res.message);
    });
  }

  updateEmployee() {
    var val = {
      id: this.Id,
      firstName: this.FirstName,
      lastName: this.LastName,
      age: this.Age,
      birthDate: this.BirthDate,
      phoneNumber: this.PhoneNumber,
      email: this.Email,
      accountNumber: this.AccountNumber,
    };

    this.service.updateEmployee(val).subscribe(res => {
      alert(res.message);
    });
  }



}
