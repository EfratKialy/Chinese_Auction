import React from 'react';
import { TabMenu } from 'primereact/tabmenu';
import { useNavigate } from 'react-router-dom';

export default function Home({ activeIndex, onTabChange }) {
  const [localActiveIndex, setLocalActiveIndex] = React.useState(activeIndex || 0); // Use the passed activeIndex or default to 0
  const navigate = useNavigate();
  const role = localStorage.getItem('role');

  const items = [
    { label: 'HomePage', icon: 'pi pi-home', url: '/Home' },
    { label: 'Gifts', icon: 'pi pi-gift', url: '/gift' },
    { label: 'Cart', icon: 'pi pi-shopping-cart', url: '/cart' },
    { label: 'Sign Out', icon: 'pi pi-sign-out', command: () => { GoOut() }}
  ];

  if (role !== 'Customer') {
    items.splice(2, 0, { label: 'Donors', icon: 'pi pi-users', url: '/donor' });
    items.splice(4, 0, { label: 'Purchasing_management', icon: 'pi pi-file', url: '/Purchasing_management' });
  }

  const onLocalTabChange = (e) => {
    setLocalActiveIndex(e.index); // Update local active index on tab change
    if (onTabChange) {
      onTabChange(e.index); // Call the provided onTabChange function if available
    }
    navigate(items[e.index].url); // Navigate to the selected tab's URL
  };

  const GoOut = () => {
    localStorage.removeItem('loggedInUser');
    localStorage.removeItem('token'); // הסרת ה-token אם קיים
    localStorage.removeItem('role'); // הסרת ה-token אם קיים
    navigate('/', { replace: true }); // משתמשים ב-replace כדי לוודא שדף הבית ייטען מחדש
    navigate(0)
  };

  return (
    <div className="card d-flex flex-row justify-content-between align-items-center">
      <TabMenu model={items} activeIndex={localActiveIndex} onTabChange={onLocalTabChange} />
    </div>
  );
}
