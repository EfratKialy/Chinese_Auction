import React, { useRef, useState } from 'react';
import { ConfirmPopup } from 'primereact/confirmpopup';
import { Button } from 'primereact/button';
import { Toast } from 'primereact/toast';
import './App.css'; // Ensure this CSS file is imported

export default function ConfirmOrder({ onConfirm, total }) {
    const [visible, setVisible] = useState(false);
    const toast = useRef(null);
    const buttonEl = useRef(null);

    const accept = () => {
        toast.current.show({ severity: 'info', summary: 'תשלום בוצע בהצלחה', detail: '!!אולי את תהיי הזוכה שלנו', life: 3000 });
        onConfirm();
    };

    const reject = () => {
        toast.current.show({ severity: 'warn', summary: 'תשלום לא בוצע', detail: 'ההזמנה ממתינה לשתלום', life: 3000 });
    };

    return (
        <>
            <Toast ref={toast} />
            <ConfirmPopup
                target={buttonEl.current}
                visible={visible}
                onHide={() => setVisible(false)}
                message={`האם ברצונך לבצע תשלום בסך ${total} שקלים?`}
                icon="pi pi-exclamation-triangle"
                accept={accept}
                reject={reject}
            />
            <div className="confirm-order-container">
                {total > 0 && <Button ref={buttonEl} onClick={() => setVisible(true)} icon="pi pi-credit-card" label="תשלום" />}
            </div>
        </>
    );
}
