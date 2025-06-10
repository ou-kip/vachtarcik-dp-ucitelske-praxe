import React, { useState, useEffect } from 'react';
import axios from 'axios';
import './CompanySelect.css'

interface CustomSelectProps {
    selectedCompany: string | null;
    handleChange: (value: string) => void;
}

const CompanySelect: React.FC<CustomSelectProps> = ({
    selectedCompany,
    handleChange,
}) => {
    const [isOpen, setIsOpen] = useState(false);
    const [hoveredIndex, setHoveredIndex] = useState<number | null>(null);
    const [loading, setLoading] = useState(false);
    const [companies, setCompanies] = useState<string[]>([]);

    const toggleDropdown = () => {
        setIsOpen((prev) => !prev);
    };

    const handleMouseEnter = (index: number) => {
        setHoveredIndex(index);
    };

    const handleMouseLeave = () => {
        setHoveredIndex(null);
    };

    const handleOptionClick = (value: string) => {
        handleChange(value);
        setIsOpen(false);
    };

    const fetchCompanies = async () => {
        setLoading(true);
        /*setError(null);*/

        try {
            axios.defaults.baseURL = 'https://praxeosu.cz:5005';
            const response = await axios.get('api/v1/internshipcompany/company/getcollection', { withCredentials: true })

            if (response.data && response.data.data && Array.isArray(response.data.data.companyNames)) {
                setCompanies(response.data.data.companyNames);
            }
            else {
                throw new Error('Neplatný formát dat.');
            }
        }
        catch (err) {
            console.error('Chyba při načítání dat:', err);
            /*setError('Nepodařilo se načíst seznam společností.');*/
        }
        finally {
            setLoading(false);
        }
    };

    // Načíst data při mountu komponenty
    useEffect(() => {
        fetchCompanies();
    }, []);

    return (
        <div style={{ position: 'relative', width: '100%' }}>
            <div
                onClick={toggleDropdown}
                style={{
                    padding: '10px',
                    borderRadius: '10px',
                    backgroundColor: '#f0f2f5',
                    cursor: 'pointer',
                }}
            >
                {selectedCompany || (loading ? 'Načítání...' : 'Vyberte firmu')}
            </div>
            {isOpen && (
                <div style={{ position: 'absolute', top: '100%', left: 0, width: '100%', border: '1px solid #ccc', backgroundColor: '#fff', zIndex: 1000}}>
                    {companies.length > 0 ? (
                        companies.map((company, index) => (
                            <div
                                key={index}
                                onClick={() => handleOptionClick(company)}
                                onMouseEnter={() => handleMouseEnter(index)}
                                onMouseLeave={handleMouseLeave}
                                style={{
                                    padding: '10px',
                                    backgroundColor:
                                        hoveredIndex === index ? 'rgb(240, 242, 245)' : '#fff',
                                    cursor: 'pointer',
                                }}
                            >
                                {company}
                            </div>
                        ))
                    ) : (
                        <div
                            style={{
                                padding: '10px',
                                color: '#999',
                            }}
                        >
                            Žádné firmy k zobrazení.
                        </div>
                    )}
                </div>
            )}
        </div>
    );
};

export default CompanySelect;