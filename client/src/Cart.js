import React, { useState, useEffect, useRef } from "react";
import { DataView, DataViewLayoutOptions } from "primereact/dataview";
import { Tag } from "primereact/tag";
import { Button } from "primereact/button";
import Home from "./HomePage";
import axios from "./axiosConfig";
import ConfirmOrder from "./MakePayment";

axios.defaults.baseURL = "http://localhost:5068";

export default function PurchaseList() {
  const [purchases, setPurchases] = useState([]);
  const [layout, setLayout] = useState("grid");
  const [role, setRole] = useState("");
  const [categories, setCategories] = useState([]);
  const [finalPayment, setFinalPayment] = useState([]);


  const nameC = useRef("");
  const userId = localStorage.getItem("loggedInUser"); // Assuming you store the user ID in localStorage

  const getPurchases = async () => {
    try {
      let response = await axios.get(
        `/purchase/api/purchase/api/GetPurchaseByCustomer?id=${userId}`
      );
      const purchaseData = response.data;

      // Get gift IDs from purchases
      const giftIds = purchaseData.map((p) => p.giftId);

      // Fetch gift details
      const giftResponse = await axios.get(`/gift/api`);

      const giftData = giftResponse.data;

      // Combine purchases with their corresponding gifts
      const purchasesWithGifts = purchaseData.map((purchase) => {
        const gift = giftData.find((g) => g.id === purchase.giftId);
        return { ...purchase, gift };
      });

      setPurchases(purchasesWithGifts);
      calFinalPayment();
    } catch (error) {
      console.error("Error fetching purchases or gifts:", error);
    }
  };
  const readRoleFromLocalStorage = () => {
    const role = localStorage.getItem("role");
    setRole(role);
  };

  const calFinalPayment = () => {
    const totalPayment = purchases
      .filter(purchase => !purchase.status) // סינון רק לרכישות שהסטטוס שלהן הוא false
      .reduce((acc, purchase) => acc + purchase.gift.price, 0);
  
    setFinalPayment(totalPayment);
  };

  const getCategories = async () => {
    try {
      let tmp = await axios.get(`/category/api`).then((res) => res.data);
      setCategories([
        { label: "All Categories", value: null },
        ...tmp.map((c) => ({ label: c.name, value: c.id })),
      ]);
    } catch (err) {
      alert(err);
    }
  };

  useEffect(() => {
    getPurchases();
    readRoleFromLocalStorage();
    getCategories();
  }, []);

  useEffect(()=>{
    calFinalPayment()
  },[purchases])

  const updateAllPurchasesStatus = async () => {
    try {
      const purchasesToUpdate = purchases.filter(purchase => !purchase.status);
  
      if (purchasesToUpdate.length === 0) {
        return;
      }
  
      const updatedPurchases = await Promise.all(
        purchasesToUpdate.map(async (purchase) => {
          // Update the purchase status
          const updatedPurchaseResponse = await axios.put(`/purchase/api?purchaseId=${purchase.id}`, {
            ...purchase,
            status: true,
          });
  
          const updatedPurchase = updatedPurchaseResponse.data;
  
          // Fetch the current gift details
          const giftResponse = await axios.get(`/gift/api/giftId?giftId=${purchase.giftId}`);
          const gift = giftResponse.data;
  
          // Update the number of purchases for the gift
          const updatedGiftResponse = await axios.put(`/gift/api/giftId?giftId=${gift.id}`, {
            ...gift,
            numOfPurchases: gift.numOfPurchases + 1,
          });
  
          const updatedGift = updatedGiftResponse.data;
  
          return updatedPurchase;
        })
      );
  
      setPurchases(prevPurchases => {
        const updatedIds = updatedPurchases.map(p => p.id);
        return prevPurchases.map(p => updatedIds.includes(p.id) ? { ...p, status: true } : p);
      });
  
      getPurchases(); // Optionally refresh purchases from server
    } catch (error) {
      console.error("Error updating purchase statuses:", error);
    }
  };
  
  

  const FindCategoryName = (id) => {
    if (categories.length > 0) {
      const c = categories.find((g) => g.value === id);
      nameC.current = c?.label;
    }
  };

  const deleteFromCart = async (purchaseId) => {
    try {
      await axios.delete(`/purchase/api?id=${purchaseId}`);
      setPurchases(purchases.filter((purchase) => purchase.id !== purchaseId));
    } catch (error) {
      console.error("Error deleting purchase:", error);
    }
  };

  const listItem = (purchase) => {
    if (!purchase || !purchase.gift) {
      return null;
    }

    return (
      <div className="col-12" key={purchase.id}>
        <div className="flex flex-column xl:flex-row xl:align-items-start p-4 gap-4 border-top-1 surface-border">
          <img
            className="w-9 sm:w-16rem xl:w-10rem shadow-2 block xl:block mx-auto border-round"
            src={`http://localhost:5068${purchase.gift.image}`}
            alt={purchase.gift.name}
          />
          <div className="flex flex-column sm:flex-row justify-content-between align-items-center xl:align-items-start flex-1 gap-4">
            <div className="flex flex-column align-items-center sm:align-items-start gap-3">
              <div className="text-2xl font-bold text-900">
                {purchase.gift.name}
              </div>
              <div className="flex align-items-center gap-3">
                <span className="flex align-items-center gap-2">
                  <i className="pi pi-tag"></i>
                  {FindCategoryName(purchase.gift.categoryId)}
                  <span className="font-semibold">{nameC.current}</span>
                </span>
                <Tag
                  value={purchase.status ? "שולם" : "לא שולם"}
                  severity={purchase.status ? "success" : "warning"}
                ></Tag>
              </div>
            </div>
            <div className="flex sm:flex-column align-items-center sm:align-items-end gap-3 sm:gap-2">
              <span className="text-2xl font-semibold">
                ${purchase.gift.price}
              </span>
              {!purchase.status && (
                <Button
                  // label="Delete"
                  icon="pi pi-trash"
                  className="p-button-danger"
                  onClick={() => deleteFromCart(purchase.id)}
                />
              )}
            </div>
          </div>
        </div>
      </div>
    );
  };

  const gridItem = (purchase) => {
    if (!purchase || !purchase.gift) {
      return null;
    }

    return (
      <div className="col-12 sm:col-6 lg:col-12 xl:col-4 p-2" key={purchase.id}>
        <div className="p-4 border-1 surface-border surface-card border-round">
          <div className="flex flex-wrap align-items-center justify-content-between gap-2">
            <div className="flex align-items-center gap-2">
              <i className="pi pi-tag"></i>
              {FindCategoryName(purchase.gift.categoryId)}
              <span className="font-semibold">{nameC.current}</span>
            </div>
            <Tag
              value={purchase.status ? "שולם" : "לא שולם"}
              severity={purchase.status ? "success" : "warning"}
            ></Tag>
          </div>
          <div className="flex flex-column align-items-center gap-3 py-5">
            <img
              className="w-9 shadow-2 border-round"
              src={`http://localhost:5068${purchase.gift.image}`}
              alt={purchase.gift.name}
            />
            <div className="text-2xl font-bold">{purchase.gift.name}</div>
          </div>
          <div className="flex align-items-center justify-content-between">
            <span className="text-2xl font-semibold">
              ${purchase.gift.price}
            </span>
            {!purchase.status && (
              <Button
                // label="Delete"
                icon="pi pi-trash"
                className="p-button-danger"
                onClick={() => deleteFromCart(purchase.id)}
              />
            )}
          </div>
        </div>
      </div>
    );
  };

  const itemTemplate = (purchase, layout) => {
    if (!purchase) {
      return;
    }

    if (layout === "list") return listItem(purchase);
    else if (layout === "grid") return gridItem(purchase);
  };

  // const header = () => {
  //   return (
  //     <div className="flex justify-content-end">
  //       <ConfirmOrder />
  //       <DataViewLayoutOptions
  //         layout={layout}
  //         onChange={(e) => setLayout(e.value)}
  //       />
  //     </div>
  //   );
  // };

  const header = () => {
    return (
      <div className="header-container">
        {<ConfirmOrder onConfirm={updateAllPurchasesStatus} total={finalPayment}/>}
        <div className="flex justify-content-end">
          <DataViewLayoutOptions
            layout={layout}
            onChange={(e) => setLayout(e.value)}
          />
        </div>
      </div>
    );
  };
  return (
    <div className="card">
      <Home activeIndex={role == "Manager" ? 3 : 2} />
      {/* <ConfirmOrder /> */}
      <DataView
        value={purchases}
        itemTemplate={(purchase) => itemTemplate(purchase, layout)}
        layout={layout}
        header={header()}
      />
    </div>
  );
}
