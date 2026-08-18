import { useState } from 'react';
import { addMedicine, updateMedicine } from '../services/api';

const emptyForm = {
  fullName: '',
  notes: '',
  expiryDate: '',
  quantity: '',
  price: '',
  brand: '',
};

function AddMedicineForm({ medicine, onCancel, onSaved }) {
  const isEditMode = Boolean(medicine);
  const [form, setForm] = useState(
    medicine
      ? {
          fullName: medicine.fullName ?? '',
          notes: medicine.notes ?? '',
          expiryDate: medicine.expiryDate ? medicine.expiryDate.slice(0, 10) : '',
          quantity: medicine.quantity ?? '',
          price: medicine.price ?? '',
          brand: medicine.brand ?? '',
        }
      : emptyForm
  );
  const [message, setMessage] = useState(null);
  const [submitting, setSubmitting] = useState(false);

  const handleChange = (e) => {
    const { name, value } = e.target;
    setForm((prev) => ({ ...prev, [name]: value }));
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setSubmitting(true);
    setMessage(null);

    const payload = {
      fullName: form.fullName,
      notes: form.notes,
      expiryDate: form.expiryDate,
      quantity: Number(form.quantity),
      price: Number(form.price),
      brand: form.brand,
    };

    try {
      if (isEditMode) {
        await updateMedicine(medicine.id, payload);
        setMessage({ type: 'success', text: 'Medicine updated successfully.' });
      } else {
        await addMedicine(payload);
        setMessage({ type: 'success', text: 'Medicine added successfully.' });
        setForm(emptyForm);
      }
      onSaved?.();
    } catch (err) {
      const text =
        err.response?.data?.message || 'Something went wrong. Please try again.';
      setMessage({ type: 'error', text });
    } finally {
      setSubmitting(false);
    }
  };

  return (
    <form onSubmit={handleSubmit} className="medicine-form">
      <h2>{isEditMode ? 'Edit Medicine' : 'Add Medicine'}</h2>

      {message && (
        <div className={message.type === 'success' ? 'msg-success' : 'msg-error'}>
          {message.text}
        </div>
      )}

      <label>
        Full Name
        <input
          type="text"
          name="fullName"
          value={form.fullName}
          onChange={handleChange}
          required
        />
      </label>

      <label>
        Notes
        <textarea name="notes" value={form.notes} onChange={handleChange} />
      </label>

      <label>
        Expiry Date
        <input
          type="date"
          name="expiryDate"
          value={form.expiryDate}
          onChange={handleChange}
          required
        />
      </label>

      <label>
        Quantity
        <input
          type="number"
          name="quantity"
          min="0"
          value={form.quantity}
          onChange={handleChange}
          required
        />
      </label>

      <label>
        Price
        <input
          type="number"
          name="price"
          min="0"
          step="0.01"
          value={form.price}
          onChange={handleChange}
          required
        />
      </label>

      <label>
        Brand
        <input type="text" name="brand" value={form.brand} onChange={handleChange} />
      </label>

      <div className="form-actions">
        <button type="submit" disabled={submitting}>
          {submitting ? 'Saving...' : isEditMode ? 'Update' : 'Add'}
        </button>
        <button type="button" onClick={onCancel}>
          Cancel
        </button>
      </div>
    </form>
  );
}

export default AddMedicineForm;
