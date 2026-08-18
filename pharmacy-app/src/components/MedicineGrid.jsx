import { useState, useEffect } from 'react';
import { getMedicines, searchMedicines } from '../services/api';
import SearchBar from './SearchBar';
import MedicineRow from './MedicineRow';
import AddMedicineForm from './AddMedicineForm';

function MedicineGrid() {
  const [medicines, setMedicines] = useState([]);
  const [filteredMedicines, setFilteredMedicines] = useState([]);
  const [showForm, setShowForm] = useState(false);
  const [editingMedicine, setEditingMedicine] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  const loadMedicines = async () => {
    setLoading(true);
    setError(null);
    try {
      const res = await getMedicines();
      setMedicines(res.data);
      setFilteredMedicines(res.data);
    } catch (err) {
      setError('Failed to load medicines.');
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    loadMedicines();
  }, []);

  const handleSearch = async (name) => {
    if (!name) {
      setFilteredMedicines(medicines);
      return;
    }
    try {
      const res = await searchMedicines(name);
      setFilteredMedicines(res.data);
    } catch (err) {
      setError('Failed to search medicines.');
    }
  };

  const handleAddClick = () => {
    setEditingMedicine(null);
    setShowForm(true);
  };

  const handleEdit = (id) => {
    const medicine = medicines.find((m) => m.id === id);
    setEditingMedicine(medicine);
    setShowForm(true);
  };

  const handleDelete = (id) => {
    setMedicines((prev) => prev.filter((m) => m.id !== id));
    setFilteredMedicines((prev) => prev.filter((m) => m.id !== id));
  };

  const handleFormCancel = () => {
    setShowForm(false);
    setEditingMedicine(null);
  };

  const handleFormSaved = () => {
    setShowForm(false);
    setEditingMedicine(null);
    loadMedicines();
  };

  return (
    <div className="medicine-grid">
      <div className="toolbar">
        <SearchBar onSearch={handleSearch} />
        <button onClick={handleAddClick}>Add Medicine</button>
      </div>

      {showForm && (
        <AddMedicineForm
          medicine={editingMedicine}
          onCancel={handleFormCancel}
          onSaved={handleFormSaved}
        />
      )}

      <div className="legend">
        <span className="legend-item">
          <span className="legend-swatch legend-swatch--red" /> Expiring within 30 days
        </span>
        <span className="legend-item">
          <span className="legend-swatch legend-swatch--yellow" /> Low stock (quantity under 10)
        </span>
        <span className="legend-item">
          <span className="legend-swatch legend-swatch--white" /> Normal
        </span>
      </div>

      {error && <div className="msg-error">{error}</div>}
      {loading ? (
        <p>Loading medicines...</p>
      ) : (
        <table>
          <thead>
            <tr>
              <th>Full Name</th>
              <th>Expiry Date</th>
              <th>Quantity</th>
              <th>Price</th>
              <th>Brand</th>
              <th>Actions</th>
            </tr>
          </thead>
          <tbody>
            {filteredMedicines.map((medicine) => (
              <MedicineRow
                key={medicine.id}
                medicine={medicine}
                onEdit={handleEdit}
                onDelete={handleDelete}
              />
            ))}
          </tbody>
        </table>
      )}
    </div>
  );
}

export default MedicineGrid;
