import { useState } from 'react';
import DropdownArrow from '../../../assets/arrow-down-square-fill.svg';

interface DropdownDbSelectProps<T> {
    placeholder: string;
    fetchData: () => Promise<T[]>;
    getLabel: (item: T) => string;
    onSelect: (item: T) => void;
}

export default function DropdownDbSelect<T>({
    placeholder,
    fetchData,
    getLabel,
    onSelect,
}: DropdownDbSelectProps<T>) {
    const [isOpen, setIsOpen] = useState(false);
    const [hoveredIndex, setHoveredIndex] = useState<number | null>(null);
    const [loading, setLoading] = useState(false);
    const [items, setItems] = useState<T[]>([]);
    const [searchTerm, setSearchTerm] = useState('');

    const toggleDropdown = async () => {
        if (!isOpen) {
            setLoading(true);
            try {
                const data = await fetchData();
                setItems(data);
            } catch (e) {
                console.error('Chyba při načítání dat:', e);
                setItems([]);
            } finally {
                setLoading(false);
            }
        }
        setIsOpen(prev => !prev);
    };

    const filtered = items.filter((item) =>
        getLabel(item).toLowerCase().includes(searchTerm.toLowerCase())
    );

    return (
        <div style={{ position: 'relative', width: '100%' }}>
            <div
                onClick={toggleDropdown}
                style={{
                    display: 'flex',
                    justifyContent: 'space-between',
                    alignItems: 'center',
                    padding: '12px',
                    borderRadius: '8px',
                    border: '1px solid #ccc',
                    backgroundColor: '#fff',
                    cursor: 'pointer',
                    fontWeight: 500,
                    boxShadow: isOpen ? '0 0 0 2px #007bff33' : 'none',
                    maxHeight: '44px'
                }}
            >
                <span style={{ color: '#999' }}>
                    {loading ? 'Načítání...' : placeholder}
                </span>
                <img src={DropdownArrow} style={{ width: 25, height: 25 }} />
            </div>

            {isOpen && (
                <div style={{
                    position: 'absolute',
                    top: '100%',
                    left: 0,
                    width: '100%',
                    border: '1px solid #ccc',
                    backgroundColor: '#fff',
                    zIndex: 1000,
                    borderRadius: '0 0 8px 8px',
                    maxHeight: 320,
                    overflowY: 'auto'
                }}>
                    <div style={{ padding: 8, borderBottom: '1px solid #eee' }}>
                        <input
                            type="text"
                            placeholder="Hledat..."
                            value={searchTerm}
                            onChange={(e) => setSearchTerm(e.target.value)}
                            style={{
                                width: '100%',
                                padding: 8,
                                borderRadius: 6,
                                border: 'none',
                                fontSize: 14,
                            }}
                        />
                    </div>
                    {filtered.map((item, i) => (
                        <div
                            key={i}
                            onClick={() => {
                                onSelect(item);
                                setIsOpen(false);
                            }}
                            onMouseEnter={() => setHoveredIndex(i)}
                            onMouseLeave={() => setHoveredIndex(null)}
                            style={{
                                padding: '10px 12px',
                                backgroundColor: hoveredIndex === i ? '#f5f7fa' : '#fff',
                                cursor: 'pointer',
                            }}
                        >
                            {getLabel(item)}
                        </div>
                    ))}
                    {filtered.length === 0 && (
                        <div style={{ padding: 10, color: '#999' }}>Žádné výsledky</div>
                    )}
                </div>
            )}
        </div>
    );
}