import React, { useState, useEffect } from 'react';
import './InternshipDetailForm.css'
import { InternshipDetail } from '../../../Exports/InternshipDetail';
import axios from 'axios';

interface InternshipDetailFormProps {
    internshipId: string | null;
}

interface ApiResponse {
    statusCode: number;
    message: string | null;
    data: {
        internshipDto: InternshipDetail;
        statusCode: number;
        message: string | null;
    };
    errors: string[];
    hasErrors: boolean;
}

const InternshipDetailForm: React.FC<InternshipDetailFormProps> = ({ internshipId }) => {
    const [detail, setDetail] = useState<InternshipDetail>();

    axios.defaults.baseURL = 'https://praxeosu.cz:5005';

    useEffect(() => {
        const fetchData = async () => {
            try {
                const response = await axios.get<ApiResponse>(`/api/v1/internship/get?InternshipId=${internshipId}`, { withCredentials: true });
                setDetail(response.data.data.internshipDto);
            } catch (error) {
                alert('Chyba při načítání dat:' + { error });
            }
        };

        fetchData();
    }, [internshipId]);

    const ResolveState = (state: number | undefined) => {
        switch (state) {
            case 0: return "Vytvořeno";
            case 1: return "Publikováno";
            case 2: return "Vybráno"
            case 3: return "Uzavřeno";
            case 4: return "Zrušeno"
        }
    }

    const handleExportDownload = async (internship: InternshipDetail | null) => {
        if (!internship || !internship.id) return;

        try {
            const response = await axios.post(
                '/api/v1/internship/get/export',
                { internshipId: internship.id },
                {
                    withCredentials: true,
                    responseType: 'blob',
                }
            );

            const sanitizedName = internship.name
                ?.replace(/[^a-z0-9_\\-]/gi, '_')
                ?.toLowerCase()
                ?.substring(0, 50); 

            const disposition = response.headers['content-disposition'];
            let fileName = `export_${sanitizedName}.html`; 

            if (disposition && disposition.includes('filename=')) {
                const match = disposition.match(/filename="?([^"]+)"?/);
                if (match?.[1]) {
                    const extension = match[1].substring(match[1].lastIndexOf('.')) || '.html';
                    fileName = `export_${internship.id}_${sanitizedName || 'internship'}${extension}`;
                }
            }

            const blob = new Blob([response.data]);
            const url = window.URL.createObjectURL(blob);
            const link = document.createElement('a');
            link.href = url;
            link.setAttribute('download', fileName);
            document.body.appendChild(link);
            link.click();
            link.remove();
            window.URL.revokeObjectURL(url);
        } catch (error) {
            console.error('Chyba při stahování souboru:', error);
        }
    };

    return (
        <div className="content-height-scalable">
            <h1 className="title-custom">{detail?.name}</h1>

            {detail?.state !== undefined && detail.state === 3 && (
                <div className="flex-end">
                    <button className="button" onClick={(e) => { e.stopPropagation(); handleExportDownload(detail); }}>
                        Vygenerovat report
                    </button>
                </div>
            )}
            
            <div className="row">
                <div className="col-md-12">
                    <div className="input-container">
                        <h3 className="input-title">Název firmy</h3>
                        <input type="text" defaultValue={detail?.companyName} placeholder="Název firmy" className="input" disabled={true} />
                    </div>
                </div>
            </div>
            <div className="row">
                <div className="col-md-6">
                    <div className="input-container">
                        <h3 className="input-title">Kategorie</h3>
                        <input type="text" defaultValue={detail?.category?.codeName} placeholder="Kategorie" className="input" disabled={true} />
                    </div>
                </div>
                <div className="col-md-6">
                    <div className="input-container">
                        <h3 className="input-title">Stav</h3>
                        <input type="text" defaultValue={ResolveState(detail?.state)} placeholder="Kategorie" className="input" disabled={true} />
                    </div>
                </div>
                <div className="row">
                    <div className="col-md-12">
                        <div className="input-container">
                            <h3 className="input-title">Popis</h3>
                            <textarea className="description" defaultValue={detail?.description} placeholder="Popis" disabled={true}></textarea>
                        </div>
                    </div>
                </div>
                <div className="row">
                    <div className="col-md-6">
                        <h3 className="subtitle">Učitelé</h3>
                        <div className="col-md-12">
                            <div className="scrollable-container">
                                {detail?.teachers.map((teacher, index) => (
                                    <div key={index} className="link-container">
                                        <input
                                            type="text"
                                            placeholder="Jméno a příjmení"
                                            defaultValue={teacher.fullName}
                                            disabled={true}
                                        />
                                    </div>
                                ))}
                            </div>
                        </div>
                    </div>
                    <div className="col-md-6">
                        <h3 className="subtitle">Firemní osoby</h3>
                        <div className="col-md-12">
                            <div className="scrollable-container">
                                {detail?.companyRelatives.map((relative, index) => (
                                    <div key={index} className="link-container">
                                        <input
                                            type="text"
                                            placeholder="Jméno a příjmení"
                                            defaultValue={relative.fullName}
                                            disabled={true}
                                        />
                                    </div>
                                ))}
                            </div>
                        </div>
                    </div>
                </div>
                <div className="row">
                    <div className="col-md-6">
                        <h3 className="subtitle">Odkazy</h3>
                        <div className="scrollable-container-links">
                            {detail?.links.map((link, index) => (
                                <div key={index} className="link-container">
                                    <a href={link.url} target="_blank" rel="noopener noreferrer">{link.name}</a>
                                </div>
                            ))}
                        </div>
                    </div>
                </div>
            </div>
        </div>
    )
}

export default InternshipDetailForm