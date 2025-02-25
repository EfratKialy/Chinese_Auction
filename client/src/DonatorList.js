import React, { useState, useEffect, useRef } from "react";
import { DataTable } from "primereact/datatable";
import { Column } from "primereact/column";
import { Button } from "primereact/button";
import { Toast } from "primereact/toast";
import Home from "./HomePage";
import "primereact/resources/themes/lara-light-indigo/theme.css";
import axios from './axiosConfig';
axios.defaults.baseURL = "http://localhost:5068";

export default function ShowDonor() {
  const [donors, setDonors] = useState([]);
  const [category, setCategory] = useState([]);
  const [donorGifts, setDonorGifts] = useState([]);
  const [expandedRows, setExpandedRows] = useState(null);
  const toast = useRef(null);

  useEffect(() => {
    getDonors();
    getCategory();
  }, []);

  useEffect(() => {
    donors.forEach((d) => {
      getDonorsGifts(d.id);
    });
  }, [donors]);

  const getDonors = async () => {
    try {
      const response = await axios.get(`/donor/api`);
      setDonors(response.data);
    } catch (err) {
      alert(err);
    }
  };

  const getCategory = async () => {
    try {
      const response = await axios.get(`/category/api`);
      setCategory(response.data);
    } catch (err) {
      alert(err);
    }
  };

  const getDonorsGifts = async (id) => {
    try {
      const response = await axios.get(`/donor/api/donor/api/byDonorId?Did=${id}`);
      setDonorGifts((prev) => {
        const updatedGifts = [...prev];
        const index = updatedGifts.findIndex((d) => d.id === id);
        if (index === -1) {
          updatedGifts.push({ id, gifts: response.data });
        } else {
          updatedGifts[index].gifts = response.data;
        }
        return updatedGifts;
      });
    } catch (err) {
      alert(err);
    }
  };

  const onRowExpand = (event) => {
    toast.current.show({
      severity: "info",
      summary: "Product Expanded",
      detail: event.data.name,
      life: 3000,
    });
  };

  const onRowCollapse = (event) => {
    toast.current.show({
      severity: "success",
      summary: "Product Collapsed",
      detail: event.data.name,
      life: 3000,
    });
  };

  const expandAll = () => {
    let _expandedRows = {};
    donors.forEach((p) => (_expandedRows[`${p.id}`] = true));
    setExpandedRows(_expandedRows);
  };

  const collapseAll = () => {
    setExpandedRows(null);
  };

  const formatCurrency = (value) => {
    return value.toLocaleString("en-US", {
      style: "currency",
      currency: "USD",
    });
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

  const allowExpansion = (rowData) => {
    return rowData.length > 0;
  };

  const getDonorName = (data) => {
    const donor = donors.find((d) => d.id === data.donorId);
    return donor ? donor.name : "Unknown";
  };

  const getCategoryName = (data) => {
    const cat = category.find((c) => c.id === data.categoryId);
    return cat ? cat.name : "Unknown";
  };

  const rowExpansionTemplate = (data) => {
    const donor = donorGifts.find((d) => d.id === data.id);
    if (!donor || !donor.gifts) {
      return <></>;
    }

    return (
      <div className="p-3">
        <h4>Orders for {data.name}</h4>
        <DataTable value={donor.gifts}>
          <Column header="Image" body={imageBodyTemplate} />
          <Column field="name" header="Name" sortable />
          <Column field="donor" header="Donor" sortable body={getDonorName} />
          <Column field="category" header="Category" sortable body={getCategoryName} />
          <Column field="price" header="Price" sortable />
          <Column field="numOfPurchases" header="NumOfPurchases" sortable />
        </DataTable>
      </div>
    );
  };

  const header = (
    <div className="flex flex-wrap justify-content-end gap-2">
      <Button icon="pi pi-plus" label="Expand All" onClick={expandAll} text />
      <Button icon="pi pi-minus" label="Collapse All" onClick={collapseAll} text />
    </div>
  );

  return (
    <div className="card">
      <Home activeIndex={2} />
      <Toast ref={toast} />
      <DataTable
        value={donors}
        expandedRows={expandedRows}
        onRowToggle={(e) => setExpandedRows(e.data)}
        onRowExpand={onRowExpand}
        onRowCollapse={onRowCollapse}
        rowExpansionTemplate={rowExpansionTemplate}
        dataKey="id"
        header={header}
        tableStyle={{ minWidth: "60rem" }}
      >
        <Column expander={allowExpansion} style={{ width: "5rem" }} />
        <Column field="name" header="Name" sortable />
        <Column field="email" header="Email" sortable />
      </DataTable>
    </div>
  );
}
