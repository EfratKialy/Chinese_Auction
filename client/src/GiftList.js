import React, { useState, useEffect, useRef } from "react";
import { Button } from "primereact/button";
import { DataView } from "primereact/dataview";
import { Dropdown } from "primereact/dropdown";
import Home from "./HomePage";
import { classNames } from "primereact/utils";
import "primereact/resources/themes/lara-light-indigo/theme.css";
import "primereact/resources/primereact.min.css";
import "primeicons/primeicons.css";
import { Dialog } from "primereact/dialog";
import axios from "./axiosConfig";
import { InputText } from "primereact/inputtext";
import { IconField } from "primereact/iconfield";

axios.defaults.baseURL = "http://localhost:5068";

export default function ShowAllGift() {
  const [gifts, setGifts] = useState([]);
  const [sortKey, setSortKey] = useState("");
  const [sortOrder, setSortOrder] = useState(0);
  const [sortField, setSortField] = useState("");
  const [visible, setVisible] = useState(false);
  const [newGiftName, setNewGiftName] = useState("");
  const [newGiftPrice, setNewGiftPrice] = useState("");

  const [selectedDonor, setSelectedDonor] = useState(null);
  const [selectedCategory, setSelectedCategory] = useState(null);
  const [newGiftImage, setNewGiftImage] = useState(null);
  const [donors, setDonors] = useState([]);
  const [categories, setCategories] = useState([]);
  const [selectedGift, setSelectedGift] = useState(null);
  const [editing, setEditing] = useState(false);
  const [globalFilterValue, setGlobalFilterValue] = useState("");
  const [role, setRole] = useState("");
  const [customers, setCustomers] = useState([]);

  const nameC = useRef("");
  const nameCustomer = useRef("");

  const sortOptions = [
    { label: "Price High to Low", value: "!price" },
    { label: "Price Low to High", value: "price" },
  ];

  const getGifts = async () => {
    try {
      let tmp = await axios.get(`/gift/api`).then((res) => res.data);
      setGifts(tmp);
    } catch (err) {
      alert(err);
    }
  };

  const getDonors = async () => {
    try {
      let tmp = await axios.get(`/donor/api`).then((res) => res.data);
      setDonors(tmp.map((d) => ({ label: d.name, value: d.id })));
    } catch (err) {
      alert(err);
    }
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

  const readRoleFromLocalStorage = () => {
    const role = localStorage.getItem("role");
    setRole(role);
  };

  useEffect(() => {
    getGifts();
    getDonors();
    getCategories();
    readRoleFromLocalStorage();
    getCustomers();
  }, []);

  const onGlobalFilterChange = (e) => {
    setGlobalFilterValue(e.target.value);
  };

  const addToCart = async (giftId) => {
    const loggedInUser = JSON.parse(localStorage.getItem("loggedInUser"));
    if (!loggedInUser) {
      console.error("User is not logged in.");
      return;
    }

    const purchaseData = {
      customerId: loggedInUser,
      giftId: giftId,
      status: false,
      paymentMethod: "Credit Card",
    };

    try {
      const response = await axios.post("/purchase/api", purchaseData);
    } catch (error) {
      console.error("Error adding purchase:", error);
    }
  };

  const MakeTheRffle = async (id) => {
    try {
      const response = await axios.get(
        `/purchase/api/purchase/api/MakeRaffleByGift?id=${id}`
      );
      getGifts();
    } catch (error) {
      console.error("Error in lottery:", error);
    }
  };

  const handleImageChange = (e) => {
    setNewGiftImage(e.target.files[0]);
  };

  const clearForm = () => {
    setNewGiftName("");
    setNewGiftPrice("");
    setSelectedDonor(null);
    setSelectedCategory(null);
  };

  const saveNewGift = async () => {
    const formData = new FormData();
    formData.append("donorId", selectedDonor);
    formData.append("categoryId", selectedCategory);
    formData.append("name", newGiftName);
    formData.append("price", newGiftPrice);
    formData.append("numOfPurchases", 0);
    formData.append("image", "ghjk");
    formData.append("image2", newGiftImage);

    try {
      const token = localStorage.getItem("token"); // or wherever you store the token
      await axios.post("/gift/api/upload", formData, {
        headers: {
          "Content-Type": "multipart/form-data",
          Authorization: `Bearer ${token}`, // Add the token here
        },
      });
      clearForm();
      getGifts();
      setVisible(false);
    } catch (error) {
      console.error("Error saving new gift:", error);
    }
  };

  const editGift = async () => {
    const editAGift = {
      donorId: selectedDonor,
      categoryId: selectedCategory,
      name: newGiftName,
      price: newGiftPrice,
      numOfPurchases: selectedGift.numOfPurchases,
      image: selectedGift.image,
    };
    try {
      await axios.put(`/gift/api/giftId?giftId=${selectedGift.id}`, editAGift);
      getGifts();
      setVisible(false);
      clearForm();
    } catch (error) {
      console.error("Error editing gift:", error);
    }
  };

  const header = () => {
    return (
      <div>
        <Dropdown
          options={sortOptions}
          value={sortKey}
          optionLabel="label"
          placeholder="Sort By Price"
          onChange={onSortChange}
          className="w-full sm:w-14rem"
          style={{ marginRight: 10 }}
        />
        <div>
          <IconField iconPosition="left">
            <InputText
              value={globalFilterValue}
              onChange={onGlobalFilterChange}
              placeholder="חפש לפי שם מתנה"
            />
          </IconField>
        </div>
      </div>
    );
  };

  const FindCategoryName = (id) => {
    if (categories.length > 0) {
      const c = categories.find((g) => g.value === id);
      nameC.current = c?.label;
    }
  };

  const FindCustomerName = (id) => {
    if (customers.length > 0) {
      const c = customers.find((g) => g.id === id);
      nameCustomer.current = c?.name;
    }
  };

  const deleteFromCart = async (giftId) => {
    try {
      await axios.delete(`/gift/api?id=${giftId}`);
      setGifts(gifts.filter((gift) => gift.id !== giftId));
    } catch (error) {
      console.error("Error deleting gift:", error);
    }
  };

  const itemTemplate = (gift, index) => {
    return (
      <div className="col-12" key={gift.id}>
        <div
          className={classNames(
            "flex flex-column xl:flex-row xl:align-items-start p-4 gap-4",
            { "border-top-1 surface-border": index !== 0 }
          )}
        >
          <img
            className="w-9 sm:w-16rem xl:w-10rem shadow-2 block xl:block mx-auto border-round"
            src={`http://localhost:5068${gift.image}`}
            alt={gift.image}
          />
          <div className="flex flex-column sm:flex-row justify-content-between align-items-center xl:align-items-start flex-1 gap-4">
            <div className="flex flex-column align-items-center sm:align-items-start gap-3">
              <div className="text-2xl font-bold text-900">{gift.name}</div>
              <div className="flex align-items-center gap-3">
                <span className="flex align-items-center gap-2">
                  <i className="pi pi-tag"></i>
                  {FindCategoryName(gift.categoryId)}
                  <span className="font-semibold">{nameC.current}</span>
                </span>
                {gift.winnerId > 0 && (
                  <span
                    className="flex align-items-center gap-2"
                    style={{ color: "red", fontStyle: "italic" }}
                  >
                    {FindCustomerName(gift.winnerId)}
                    <span className="font-semibold">
                      {nameCustomer.current}: ההגרלה נסגרה. הזוכה בהגרלה היא
                    </span>
                  </span>
                )}
              </div>
            </div>

            <div className="flex sm:flex-column align-items-center sm:align-items-end gap-3 sm:gap-2">
              <span className="text-2xl font-semibold">${gift.price}</span>
              {gift.winnerId === 0 && (
                <Button
                  icon="pi pi-shopping-cart"
                  className="p-button-rounded"
                  onClick={() => addToCart(gift.id)}
                />
              )}
              {role === "Manager" && (
                <Button
                  icon="pi pi-pencil"
                  onClick={() => {
                    setSelectedGift(gift);
                    setNewGiftName(gift.name);
                    setNewGiftPrice(gift.price);
                    setSelectedDonor(gift.donorId);
                    setSelectedCategory(gift.categoryId);
                    setEditing(true);
                    setVisible(true);
                  }}
                />
              )}
              {role === "Manager" && (
                <Button
                  icon="pi pi-trash"
                  onClick={() => deleteFromCart(gift.id)}
                />
              )}
              {role === "Manager" && gift.winnerId === 0 && (
                <Button
                  icon="pi pi-sparkles"
                  label="הגרלה"
                  onClick={() => MakeTheRffle(gift.id)}
                />
              )}
            </div>
          </div>
        </div>
      </div>
    );
  };

  const listTemplate = (items) => {
    if (!items || items.length === 0) return null;

    let filteredItems = items;
    if (selectedCategory !== null) {
      filteredItems = items.filter(
        (item) => item.categoryId === selectedCategory
      );
    }

    if (globalFilterValue.trim()) {
      filteredItems = filteredItems.filter((item) =>
        item.name.toLowerCase().includes(globalFilterValue.toLowerCase())
      );
    }

    let list = filteredItems.map((gift, index) => itemTemplate(gift, index));

    return <div className="grid grid-nogutter">{list}</div>;
  };

  const clearFilters = () => {
    setSelectedCategory(null);
    setGlobalFilterValue("");
  };

  const onSortChange = (event) => {
    const value = event.value;

    if (value.indexOf("!") === 0) {
      setSortOrder(-1);
      setSortField(value.substring(1, value.length));
      setSortKey(value);
    } else {
      setSortOrder(1);
      setSortField(value);
      setSortKey(value);
    }
  };

  const footerContent = (
    <div>
      <Button
        label="Cancel"
        icon="pi pi-times"
        onClick={() => {
          setVisible(false);
          setEditing(false);
          clearForm();
        }}
        className="p-button-text"
      />
      <Button
        label={editing ? "Save Changes" : "Save"}
        icon="pi pi-check"
        onClick={editing ? editGift : saveNewGift}
        autoFocus
      />
    </div>
  );

  return (
    <div className="card">
      <Home activeIndex={1} />
      {role === "Manager" && (
        <Button
          label="הוספת מתנה"
          icon="pi pi-external-link"
          onClick={() => {
            setVisible(true);
            setEditing(false);
          }}
        />
      )}
      <Dropdown
        options={categories}
        value={selectedCategory}
        onChange={(e) => setSelectedCategory(e.value)}
        optionLabel="label"
        placeholder="Filter by Category"
        className="w-full sm:w-14rem"
        style={{ marginRight: 10 }}
      />

      <Button
        label="Clear Filters"
        icon="pi pi-filter-slash"
        onClick={clearFilters}
        className="p-button-text"
      />
      <DataView
        value={gifts}
        listTemplate={listTemplate}
        header={header()}
        sortField={sortField}
        sortOrder={sortOrder}
      />

      <Dialog
        header={editing ? "Edit Gift" : "Add New Gift"}
        visible={visible}
        style={{ width: "50vw" }}
        onHide={() => setVisible(false)}
        footer={footerContent}
      >
        <div className="p-fluid">
          <div className="p-field">
            <label htmlFor="newGiftName">Gift Name</label>
            <input
              id="newGiftName"
              type="text"
              value={newGiftName}
              onChange={(e) => setNewGiftName(e.target.value)}
              className="p-inputtext"
            />
          </div>
          <div className="p-field">
            <label htmlFor="newGiftPrice">Price</label>
            <input
              id="newGiftPrice"
              type="number"
              value={newGiftPrice}
              onChange={(e) => setNewGiftPrice(e.target.value)}
              className="p-inputtext"
            />
          </div>
          <div className="p-field">
            <label htmlFor="contributor">Select Contributor</label>
            <Dropdown
              id="contributor"
              value={selectedDonor}
              options={donors}
              onChange={(e) => setSelectedDonor(e.value)}
              optionLabel="label"
              placeholder="Select Contributor"
            />
          </div>
          <div className="p-field">
            <label htmlFor="category">Select Category</label>
            <Dropdown
              id="category"
              value={selectedCategory}
              options={categories.filter((c) => c.value)}
              onChange={(e) => setSelectedCategory(e.value)}
              optionLabel="label"
              placeholder="Select Category"
            />
          </div>
          {!editing ? (
            <div className="p-field">
              <label htmlFor="image">Upload Image</label>
              <input
                type="file"
                id="image"
                accept="image/*"
                onChange={handleImageChange}
              />
            </div>
          ) : (
            <></>
          )}
        </div>
      </Dialog>
    </div>
  );
}
