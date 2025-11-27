"use client";

import { useEffect, useState } from "react";

interface LocationMiniMapProps {
    address?: string;
    city?: string;
    postalCode?: string;
}

export default function LocationMiniMap({ address, city, postalCode }: LocationMiniMapProps) {
    const [mapUrl, setMapUrl] = useState<string>("");

    useEffect(() => {
        // Construct the full address for geocoding
        const fullAddress = [address, postalCode, city].filter(Boolean).join(", ");
        
        if (!fullAddress) {
            setMapUrl("");
            return;
        }

        // Use OpenStreetMap Nominatim to geocode the address
        fetch(`https://nominatim.openstreetmap.org/search?format=json&q=${encodeURIComponent(fullAddress)}, Denmark`)
            .then(res => res.json())
            .then(data => {
                if (data && data.length > 0) {
                    const { lat, lon } = data[0];
                    // Create an OpenStreetMap embed URL
                    const url = `https://www.openstreetmap.org/export/embed.html?bbox=${parseFloat(lon) - 0.01},${parseFloat(lat) - 0.01},${parseFloat(lon) + 0.01},${parseFloat(lat) + 0.01}&layer=mapnik&marker=${lat},${lon}`;
                    setMapUrl(url);
                }
            })
            .catch(err => {
                console.error("Error geocoding address:", err);
                setMapUrl("");
            });
    }, [address, city, postalCode]);

    if (!mapUrl) {
        return (
            <div className="w-full h-64 bg-gray-100 rounded-lg flex items-center justify-center text-gray-500">
                <p className="text-sm">Enter an address to see the location preview</p>
            </div>
        );
    }

    return (
        <div className="w-full h-64 rounded-lg overflow-hidden border border-gray-200">
            <iframe
                width="100%"
                height="100%"
                frameBorder="0"
                scrolling="no"
                src={mapUrl}
                title="Location Preview Map"
            />
        </div>
    );
}
