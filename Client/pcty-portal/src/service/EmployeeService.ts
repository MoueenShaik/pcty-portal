import { Employee } from "../models/Employee";
import { EmployeeCosts } from "../models/EmployeeCosts";
import {Buffer} from 'buffer';

export class EmployeeService {

    public getHeaders(): Headers {
        let headers = new Headers();

        headers.append('Authorization', 'Bearer ' + this.getToken());
        headers.append('Access-Control-Allow-Origin', '*')
        headers.append('Access-Control-Allow-Origin', 'http://localhost:3000');
        headers.append('Access-Control-Allow-Credentials', 'true');      

        return headers;
    }

    private getToken() : string
    {
        var inputDetails = {"userId":"1001","name":"Moueen Shaik","emailAddress":"moueen.shaik@gmail.com"};
        const encodedString = Buffer.from(JSON.stringify(inputDetails)).toString('base64');
        return encodedString;
    }

    public async getAllEmployees(): Promise<Employee[]> {
        
        const apiUrl = process.env.REACT_APP_API_EMPLOYEE_ENDPOINT;
        const response = await fetch(apiUrl, { method: 'GET', headers: this.getHeaders(), credentials: 'include' })
        return await response.json();
    }

    public async getEmployeeBenefitsCosts(id: number): Promise<EmployeeCosts> {

        const apiUrl = process.env.REACT_APP_API_EMPLOYEE_ENDPOINT + 
        '/GetEmployeeBenefitsCost?employeeId=' + id;
        const response = await fetch(apiUrl, { method: 'GET', headers: this.getHeaders(), credentials: 'include' })
        return await response.json();
    }

}
export default EmployeeService