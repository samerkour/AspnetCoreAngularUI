import { Component, OnInit } from '@angular/core';
import { ApiserviceService } from 'src/app/apiservice.service';

@Component({
  selector: 'app-show-employee',
  templateUrl: './show-employee.component.html',
  styleUrls: ['./show-employee.component.css']
})
export class ShowEmployeeComponent implements OnInit {

  constructor(private service: ApiserviceService<any>) { }

  EmployeeList: any = [];
  ModalTitle = "";
  ActivateAddEditEmpComp: boolean = false;
  emp: any;

  ngOnInit(): void {
    this.refreshEmpList();
  }



  addClick() {
    this.emp = {
      id: "0",
      firstName: "",
      lastName: "",
      age:"0",
      birthDate:new Date(),
      PhoneNumber: "",
      email:"",
      accountNumber:""

    }
    this.ModalTitle = "Add Employee";
    this.ActivateAddEditEmpComp = true;
  }



  editClick(item: any) {
    this.emp = item;
    this.ModalTitle = "Edit Employee";
    this.ActivateAddEditEmpComp = true;
  }




  deleteClick(item: any) {
    if (confirm('Are you sure??')) {
      this.service.deleteEmployee(item.id).subscribe(data => {
        alert(data.message);
        this.refreshEmpList();
      })
    }
  }

  closeClick() {
    this.ActivateAddEditEmpComp = false;
    this.refreshEmpList();
  }

  refreshEmpList() 
  {
    this.service.getEmployeeList().subscribe(data => 
    {
      this.EmployeeList = data.result;
    });
  }
}
