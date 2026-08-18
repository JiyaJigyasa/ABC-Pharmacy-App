import { deleteMedicine } from '../services/api';

const THIRTY_DAYS_MS = 30 * 24 * 60 * 60 * 1000;

function MedicineRow({ medicine, onEdit, onDelete }) {
  const isExpiringSoon =
    new Date(medicine.expiryDate) < new Date(Date.now() + THIRTY_DAYS_MS);
  const isLowStock = medicine.quantity < 10;

  const rowStyle = isExpiringSoon
    ? { backgroundColor: '#f8d7da' }
    : isLowStock
    ? { backgroundColor: '#fff3cd' }
    : undefined;

  const handleDelete = async () => {
    const confirmed = window.confirm(
      `Delete "${medicine.fullName}"? This cannot be undone.`
    );
    if (!confirmed) return;

    try {
      await deleteMedicine(medicine.id);
      onDelete(medicine.id);
    } catch (err) {
      alert('Failed to delete medicine. Please try again.');
    }
  };

  return (
    <tr style={rowStyle}>
      <td>{medicine.fullName}</td>
      <td>{new Date(medicine.expiryDate).toLocaleDateString()}</td>
      <td>{medicine.quantity}</td>
      <td>{medicine.price.toFixed(2)}</td>
      <td>{medicine.brand}</td>
      <td>
        <button onClick={() => onEdit(medicine.id)}>Edit</button>
        <button onClick={handleDelete}>Delete</button>
      </td>
    </tr>
  );
}

export default MedicineRow;
