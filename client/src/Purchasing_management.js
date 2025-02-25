
import React, { useState, useEffect, useRef } from "react";
import { DataTable } from "primereact/datatable";
import { Column } from "primereact/column";
import { Button } from "primereact/button";
import { Toast } from "primereact/toast";
import Home from "./HomePage";
import "primereact/resources/themes/lara-light-indigo/theme.css";
//import axios from "axios";
import axios from './axiosConfig';
axios.defaults.baseURL = "http://localhost:5068";

export default function ShowGifts() {
  const [gifts, setGifts] = useState([]);
  const [purchases, setPurchases] = useState([]);
  const [customers, setCustomers] = useState([]);
  const [expandedRows, setExpandedRows] = useState(null);
  const toast = useRef(null);

  const getGifts = async () => {
    try {
      let tmp = await axios.get(`/gift/api`).then((res) => {
        return res.data;
      });
      setGifts(tmp);
    } catch (err) {
      alert(err);
    }
  };

  const getPurchasesByGift = async (giftId) => {
    try {
      let tmp = await axios.get(`/purchase/api/purchase/api/GetPurchaseByGift?id=${giftId}`).then((res) => {
        return res.data;
      });
      return tmp;
    } catch (err) {
      alert(err);
    }
  };

  const getCustomers = async () => {
    try {
      let tmp = await axios.get(`/customer/api`).then((res) => {
        return res.data;
      });
      setCustomers(tmp);
    } catch (err) {
      alert(err);
    }
  };

  const downloadGiftsWithWinners = async () => {
    try {
      const response = await axios.get('/purchase/api/download-gifts-with-winners', {
        responseType: 'blob', // חשוב כדי לטפל בקבצים בינאריים
      });

      // יצירת URL אובייקט לנתונים הבינאריים
      const url = window.URL.createObjectURL(new Blob([response.data]));

      // יצירת אלמנט <a> להורדת הקובץ
      const link = document.createElement('a');
      link.href = url;
      link.setAttribute('download', 'gifts_with_winners.csv');

      // הוספת האלמנט לדף ולחיצה עליו כדי להתחיל את ההורדה
      document.body.appendChild(link);
      link.click();

      // הסרת האלמנט מהדף לאחר ההורדה
      link.parentNode.removeChild(link);
    } catch (error) {
      console.error("Error downloading file:", error);
    }
  };

  useEffect(() => {
    getGifts();
    getCustomers();
  }, []);

  useEffect(() => {
    gifts.forEach(async (gift) => {
      let giftPurchases = await getPurchasesByGift(gift.id);
      setPurchases(prevPurchases => [...prevPurchases, { giftId: gift.id, purchases: giftPurchases }]);
    });
  }, [gifts]);

  const onRowExpand = (event) => {
    toast.current.show({
      severity: "info",
      summary: "Gift Expanded",
      detail: event.data.name,
      life: 3000,
    });
  };

  const onRowCollapse = (event) => {
    toast.current.show({
      severity: "success",
      summary: "Gift Collapsed",
      detail: event.data.name,
      life: 3000,
    });
  };

  const expandAll = () => {
    let _expandedRows = {};
    gifts.forEach((gift) => (_expandedRows[`${gift.id}`] = true));
    setExpandedRows(_expandedRows);
  };

  const collapseAll = () => {
    setExpandedRows(null);
  };

  const getCustomerName = (customerId) => {
    const customer = customers.find(c => c.id === customerId);
    return customer ? customer.name : "Unknown";
  };

  const getCustomerPhone = (customerId) => {
    const customer = customers.find(c => c.id === customerId);
    return customer ? customer.phone : "Unknown";
  };

  const getCustomerEmail = (customerId) => {
    const customer = customers.find(c => c.id === customerId);
    return customer ? customer.email : "Unknown";
  };

  const imageBodyTemplate = (rowData) => {
    return (
      <img
        src={`http://localhost:5068${rowData.image}`}
        alt={rowData.image}
        width="64px"
        className="shadow-4"
      />
    );
  };

  const rowExpansionTemplate = (data) => {
    const giftPurchases = purchases.find(p => p.giftId === data.id);
    const filteredPurchases = giftPurchases ? giftPurchases.purchases.filter(p => p.status === true) : [];
    return (
      <div className="p-3">
        <h4>Purchases for {data.name}</h4>
        <DataTable value={filteredPurchases}>
          <Column field="customerId" header="Customer" body={(rowData) => getCustomerName(rowData.customerId)} />
          <Column field="phone" header="Phone" body={(rowData) => getCustomerPhone(rowData.customerId)} />
          <Column field="email" header="Email" body={(rowData) => getCustomerEmail(rowData.customerId)} />
          <Column field="status" header="Status" />
          <Column field="paymentMethod" header="Payment Method" />
        </DataTable>
      </div>
    );
  };

  const header = (
    <div className="flex flex-wrap justify-content-end gap-2">
      <Button icon="pi pi-plus" label="Expand All" onClick={expandAll} text />
      <Button icon="pi pi-minus" label="Collapse All" onClick={collapseAll} text />
      <Button icon="pi pi-download" label="Download Gifts with Winners" onClick={downloadGiftsWithWinners} />
    </div>
  );

  const numberOfPurchasesBodyTemplate = (rowData) => {
    const giftPurchases = purchases.find(p => p.giftId === rowData.id);
    const filteredPurchases = giftPurchases ? giftPurchases.purchases.filter(p => p.status === true) : [];
    return filteredPurchases.length;
  };

  return (
    <div className="card">
      <Home activeIndex={4} />
      <Toast ref={toast} />
      <DataTable
        value={gifts}
        expandedRows={expandedRows}
        onRowToggle={(e) => setExpandedRows(e.data)}
        onRowExpand={onRowExpand}
        onRowCollapse={onRowCollapse}
        rowExpansionTemplate={rowExpansionTemplate}
        dataKey="id"
        header={header}
        tableStyle={{ minWidth: "60rem" }}
        sortMode="multiple"
      >
        <Column expander style={{ width: "5rem" }} />
        <Column field="name" header="Name" sortable />
        <Column body={imageBodyTemplate} header="Image" />
        <Column field="price" header="Price" sortable />
        <Column
          header="Number of Purchases"
          body={numberOfPurchasesBodyTemplate}
          sortable
          //sortField="numberOfPurchases"
        />
        <Column field="description" header="Description" sortable />
      </DataTable>
    </div>
  );
}
