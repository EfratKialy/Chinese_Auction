import "./App.css";
import ShowDonor from "./DonatorList";
import LoginDemo from "./Login";
import { BrowserRouter, Route, Routes } from "react-router-dom";
import ShowAllGift from "./GiftList";
import Home from "./HomePage";
import RegisterIn from "./Register";
import PurchaseList from "./Cart";
import ShowCustomersAndGifts from "./Purchasing_management";

function App() {
  return (
    <div className="App">
      <BrowserRouter>
        <Routes>
          <Route path="/" element={<LoginDemo />} />
          <Route path="/gift" element={<ShowAllGift />} />
          <Route path="/cart" element={<PurchaseList />} />
          <Route path="/donor" element={<ShowDonor />} />
          <Route path="/home" element={<Home />} />
          <Route path="/register" element={<RegisterIn />} />
          <Route path="/register/login" element={<LoginDemo />} />
          <Route path="/register/login/Home" element={<Home />} />
          <Route
            path="/Purchasing_management"
            element={<ShowCustomersAndGifts />}
          />
        </Routes>
      </BrowserRouter>
    </div>
  );
}

export default App;
