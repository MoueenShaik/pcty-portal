import { Employee } from "../models/Employee";
import { EmployeeCosts } from "../models/EmployeeCosts";

export class EmployeeService {


    
    public async getAllEmployees(): Promise<Employee[]> {
        let headers = new Headers();

    // headers.append('Content-Type', 'application/json');
    // headers.append('Accept', 'application/json');
    headers.append('Authorization', 'Bearer eyJ1c2VySWQiOiIxMDAxIiwibmFtZSI6Ik1vdWVlbiBTaGFpayIsImVtYWlsQWRkcmVzcyI6Im1vdWVlbi5zaGFpa0BnbWFpbC5jb20ifQ==');
    // headers.append('Origin','http://localhost:3000');
    headers.append('Access-Control-Allow-Origin', '*')
    headers.append('Access-Control-Allow-Origin', 'http://localhost:3000');
headers.append('Access-Control-Allow-Credentials', 'true');
///headers.append('GET', 'POST', 'OPTIONS');
    const defaultOptions = {
      headers:headers,
      /// mode: 'no-cors',
      credentials: 'include',
      method: 'GET',
    };

    const apiUrl = 'https://localhost:7046/api/Employee';
    const response = await fetch(apiUrl,{ method : 'GET' , headers : headers, credentials:'include'})
    //   .then((response) => {  response.json()})
    //   .then((data) => console.log('This is your data', data));

       /// const response = await fetch('/api/users');
        return await response.json();
    }

    public async addUser(user: any) {
        const response = await fetch(`/api/user`, {
            method: 'POST',
            headers: {'Content-Type': 'application/json'},
            body: JSON.stringify({user})
          })
        return await response.json();
    }

    public async getEmployeeBenefitsCosts(id : number) : Promise<EmployeeCosts> {
        let headers = new Headers();

        // headers.append('Content-Type', 'application/json');
        // headers.append('Accept', 'application/json');
        headers.append('Authorization', 'Bearer eyJ1c2VySWQiOiIxMDAxIiwibmFtZSI6Ik1vdWVlbiBTaGFpayIsImVtYWlsQWRkcmVzcyI6Im1vdWVlbi5zaGFpa0BnbWFpbC5jb20ifQ==');
        // headers.append('Origin','http://localhost:3000');
        headers.append('Access-Control-Allow-Origin', '*')
        headers.append('Access-Control-Allow-Origin', 'http://localhost:3000');
    headers.append('Access-Control-Allow-Credentials', 'true');
    ///headers.append('GET', 'POST', 'OPTIONS');
        const defaultOptions = {
          headers:headers,
          /// mode: 'no-cors',
          credentials: 'include',
          method: 'GET',
        };
    
        const apiUrl =  'https://localhost:7046/api/Employee/GetEmployeeBenefitsCost?employeeId=' + id;
        const response = await fetch(apiUrl,{ method : 'GET' , headers : headers, credentials:'include'})
        //   .then((response) => {  response.json()})
        //   .then((data) => console.log('This is your data', data));
    
           /// const response = await fetch('/api/users');
            return await response.json();   
    }

}
export default EmployeeService