
import React from 'react';
import { Divider } from 'primereact/divider';
import { InputText } from 'primereact/inputtext';
import { Button } from 'primereact/button';
import { useState,useContext } from "react";
//import axios from "axios";
import axios from './axiosConfig';
import { useNavigate } from 'react-router-dom';

axios.defaults.baseURL = "http://localhost:5068";

export default function RegisterIn() {
    const [password, setPassword] = useState("");
    const [userName, setUserName] = useState("");
    const [email, setEmail] = useState("");
    const [phone, setPhone] = useState("");

    const navigate = useNavigate();

    // const { userId, setUserId } = useContext(userIdContext);
    // const { userName, setUserName } = useContext(userNameContext);

    const SignUp = async (e) => {
        // e.preventDefault();
        let u =
        {
            "name": userName,
            "email": email,
            "phone": phone,
            "password": password
          }
          if(!userName||!email||!phone||!password)
            {
                alert("All fields are required")
            }
        else{
        try {
          const res = await axios.post(`/customer/api`,u);
          if (res?.status === 200) {
            const user = {
                name: userName,
                id: res.data.id,
                email: res.data.email,
                phone: res.data.phone,
                gifts: res.data.gifts,
              };
              localStorage.setItem("loggedInUser", JSON.stringify(user.id)); // Store user object in localStorage
            navigate("./login", { replace: false });
          } else if (res?.status === 401) {
            alert("Invalid username or password");
          } else {
            alert("An error occurred. Please try again.");
        } }catch (err) {
          console.log(err);
        }
      };}
     
    return (
        <div className="card">
            <div className="flex flex-column md:flex-row">
                <div className="w-full md:w-5 flex flex-column align-items-s justify-content-center gap-3 py-5">
                    <div className="flex flex-wrap justify-content-center align-items-center gap-2">
                        <label htmlFor="username" className="w-6rem">
                            Username
                        </label>
                        <InputText id="username" type="text" onChange={(e) => setUserName(e.target.value)}/>
                    </div>
                    <div className="flex flex-wrap justify-content-center align-items-center gap-2">
                        <label htmlFor="password" className="w-6rem">
                            Password
                        </label>
                        <InputText id="password" type="password" onChange={(e) => setPassword(e.target.value)}/>
                    </div>
                    <div className="flex flex-wrap justify-content-center align-items-center gap-2">
                        <label htmlFor="phone" className="w-6rem">
                            phone
                        </label>
                        <InputText id="phone" type="phone" onChange={(e) => setPhone(e.target.value)}/>
                    </div>
                    <div className="flex flex-wrap justify-content-center align-items-center gap-2">
                        <label htmlFor="email" className="w-6rem">
                            email
                        </label>
                        <InputText id="email" type="email" onChange={(e) => setEmail(e.target.value)}/>
                    </div>
                    <div className="w-full md:w-30 flex align-items-center justify-content-center py-5">
                    <Button label="Sign Up" icon="pi pi-user-plus" className="p-button-success" onClick={SignUp}></Button>
                </div>                </div>
                
            </div>
        </div>
    )
}
