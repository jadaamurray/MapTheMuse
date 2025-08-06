import { useEffect, useState } from 'react';
import axios from '../api/axios';

export default function DestinationsPage() {
  const [destinations, setDestinations] = useState([]);

  useEffect(() => {
    axios.get('/destinations')
      .then(res => setDestinations(res.data))
      .catch(err => console.error(err));
  }, []);

  return (
    <div className="p-4">
      <h1 className="text-2xl font-bold mb-4">Destinations</h1>
      <ul className="space-y-4">
        {destinations.map(dest => (
          <li key={dest.id} className="border p-4 rounded">
            <h2 className="text-xl font-semibold">{dest.name}</h2>
            <p>{dest.shortDescription}</p>
          </li>
        ))}
      </ul>
    </div>
  );
}
