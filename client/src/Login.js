import React, { useState } from "react";
import { Divider } from "primereact/divider";
import { InputText } from "primereact/inputtext";
import { Button } from "primereact/button";
import axios from "./axiosConfig";
import { useNavigate } from "react-router-dom";
import { jwtDecode } from "jwt-decode";

axios.defaults.baseURL = "http://localhost:5068";

export default function LoginDemo() {
  const [password, setPassword] = useState("");
  const [userName, setUserName] = useState("");
  const navigate = useNavigate();

  const LoginUser = async (e) => {
    e.preventDefault();

    try {
      const res = await axios.get(
        `/customer/api/login?name=${userName}&password=${password}`
      );
      if (res?.status === 200) {
        const token = res.data;
        localStorage.setItem("token", token); // שמירת ה-token ב-localStorage

        const decoded = jwtDecode(token);
        localStorage.setItem("loggedInUser", decoded.UserID); // שמירת ה-ID של המשתמש ב-localStorage
        localStorage.setItem("role", decoded.role); // שמירת ה-role של המשתמש ב-localStorage

        navigate("./Home", { replace: false });
      } else if (res?.status === 401) {
        alert("Invalid username or password");
      } else {
        alert("An error occurred. Please try again.");
      }
    } catch (err) {
      alert("Invalid username or password");
    }
  };

  const Register = async (e) => {
    try {
      navigate("./register", { replace: false });
    } catch (err) {
      console.log(err);
    }
  };

  return (
    <div className="card">
      <div className="flex flex-column md:flex-row">
        <div className="w-full md:w-5 flex flex-column align-items-s justify-content-center gap-3 py-5">
          <div className="flex flex-wrap justify-content-center align-items-center gap-2">
            <label htmlFor="username" className="w-6rem">
              Username
            </label>
            <InputText
              id="username"
              type="text"
              onChange={(e) => setUserName(e.target.value)}
            />
          </div>
          <div className="flex flex-wrap justify-content-center align-items-center gap-2">
            <label htmlFor="password" className="w-6rem">
              Password
            </label>
            <InputText
              id="password"
              type="password"
              onChange={(e) => setPassword(e.target.value)}
            />
          </div>
          <Button
            label="Login"
            icon="pi pi-user"
            className="w-10rem mx-auto"
            onClick={LoginUser}
          ></Button>
        </div>
        <div className="w-full md:w-2">
          <Divider layout="vertical" className="hidden md:flex">
            <b>OR</b>
          </Divider>
          <Divider
            layout="horizontal"
            className="flex md:hidden"
            align="center"
          >
            <b>OR</b>
          </Divider>
        </div>
        <div className="w-full md:w-5 flex align-items-center justify-content-center py-5">
          <Button
            label="Sign Up"
            icon="pi pi-user-plus"
            className="p-button-success"
            onClick={Register}
          ></Button>
        </div>
      </div>
    </div>
  );
}
