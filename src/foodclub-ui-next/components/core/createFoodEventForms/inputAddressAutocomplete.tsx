"use client";

import { Input } from "@/components/ui/input"
import { Label } from "@/components/ui/label"
import { useEffect, useState } from "react"
import { AddressAutocompleteItem } from "@/types/address-autocomplete.type"
import { Popover, PopoverAnchor, PopoverContent, PopoverTrigger } from "@/components/ui/popover"
import { LocationDetails } from "./whenWhereForm";

interface InputAddressAutocompleteProps extends React.InputHTMLAttributes<HTMLInputElement> {
    labelText: string,
    id: string,
    setFormLocationDetailsAction: (locationDetails: LocationDetails) => void
}



export default function InputAddressAutocomplete({
    labelText: label,
    id,
    setFormLocationDetailsAction,
    ...props
}: InputAddressAutocompleteProps) {
    const [inputSearchQuery, setInputSearchQuery] = useState('');
    const [autocompleteContent, setAutocompleteContent] = useState<AddressAutocompleteItem[] | null>(null);
    const [popoverOpen, setPopoverOpen] = useState<boolean>(false);
    const [kommuner, setKommuner] = useState<any[] | null>(null);

    function handleInputChange(e: React.ChangeEvent<HTMLInputElement>) {
        const inpVal = e.target.value;
        setInputSearchQuery(inpVal);

        if (!inpVal) {
            setAutocompleteContent(null);
            return;
        }

        fetch(`https://api.dataforsyningen.dk/adresser/autocomplete?q=${encodeURIComponent(inpVal)}`)
            .then((res) => res.json())
            .then((data: AddressAutocompleteItem[]) => {
                setAutocompleteContent(data);
                setPopoverOpen(true);
            })
            .catch(() => setAutocompleteContent(null));
    }

    useEffect(() => {
        let cancelled = false;
        fetch("https://api.dataforsyningen.dk/kommuner")
            .then(r => r.json())
            .then(data => { if (!cancelled) setKommuner(data) })
            .catch(() => { if (!cancelled) setKommuner([]) });
        return () => { cancelled = true };
    }, []);

    function getRegionByMunicipality(kode?: string|null) {
        if (!kode || !Array.isArray(kommuner)) return "Unknown";
        const kommune = kommuner.find((k: { kode: string; }) => k.kode === kode);
        return kommune?.region?.navn ?? "Unknown";
    }


    return (
        <div className="grid gap-2">
            <Label
                htmlFor={id}
                className="text-xs font-bold text-muted-foreground"
            >
                {label}
            </Label>

            <Popover open={popoverOpen} onOpenChange={setPopoverOpen} modal={false} >
                <PopoverAnchor asChild>
                    <Input
                        id={id}
                        className="h-10 bg-gray-50 text-sm font-normal text-muted-foreground font-['Poppins']"
                        value={inputSearchQuery}
                        onChange={handleInputChange}
                        onFocus={e => { if (inputSearchQuery) setPopoverOpen(true) }}
                        {...props}
                    />
                </PopoverAnchor>

                <PopoverContent
                    align="start"
                    onOpenAutoFocus={(e) => e.preventDefault()}
                    onCloseAutoFocus={(e) => e.preventDefault()}
                >
                    <div className="flex flex-col max-h-32 overflow-y-auto">
                        {autocompleteContent?.map(a =>
                        (
                            <p
                                key={a.adresse.id}
                                className="cursor-pointer p-2"
                                onClick={e => {
                                    const formatNullableStr = (str: string | null | undefined, separator: string) => str ? separator + str : "";
                                    const formData: LocationDetails = {
                                        streetNumber: `${a.adresse.vejnavn} ${a.adresse.husnr}${formatNullableStr(a.adresse?.etage, " ")}${formatNullableStr(a.adresse.dør, ". ")}`,
                                        city: a.adresse.postnrnavn,
                                        postalCode: a.adresse.postnr,
                                        region: getRegionByMunicipality(a.adresse.kommunekode)
                                    };
                                    setFormLocationDetailsAction(formData);
                                    setInputSearchQuery('');
                                    setPopoverOpen(false);
                                }}
                            >
                                {a.tekst}
                            </p>
                        )
                        )}
                    </div>
                </PopoverContent>

            </Popover>
        </div>
    )
}