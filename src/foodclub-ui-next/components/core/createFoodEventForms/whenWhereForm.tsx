"use client";

import { useState } from "react";
import ComponentsWrapper from "../wrappers/componentsWrapper";
import OrangeWrapper from "../wrappers/orangeWrapper";
import CreateEventFormInput from "./createEventFormInput";
import InputAddressAutocomplete from "./inputAddressAutocomplete";
import LocationMiniMap from "./locationMiniMap";
import { FormBasics, FormWhenWhere, FormWizardActionType, FormWizardStep, useFormWizardContext } from "./formWizardContext";
import { useRouter } from "next/navigation";

export interface LocationDetails {
    streetNumber?: string;
    postalCode?: string;
    city?: string;
    region?: string;
}

export default function WhenWhereForm() {
    const [formState, formDispatch] = useFormWizardContext();
    const router = useRouter();

    // Initialize location details from context if available
    const [formLocationDetails, setFormLocationDetails] = useState<LocationDetails | null>(
        formState.whenWhere ? {
            streetNumber: formState.whenWhere.streetAddress,
            postalCode: formState.whenWhere.postalCode,
            city: formState.whenWhere.city,
            region: formState.whenWhere.region,
        } : null
    );

    function onSubmit(e: React.FormEvent) {
        e.preventDefault();
        const form = e.target as HTMLFormElement;
        const formData = new FormData(form);


        const formDataObj: FormWhenWhere = {
            date: formData.get("date") as string,
            startTime: formData.get("startTime") as string,
            endTime: formData.get("endTime") as string | undefined,

            streetAddress: formData.get("address") as string,
            postalCode: formData.get("postalCode") as string,
            city: formData.get("city") as string,
            region: formData.get("region") as string
        };

        formDispatch({ type: FormWizardActionType.Set, step: FormWizardStep.WhenWhereForm, value: { whenWhere: formDataObj } });

        router.push("/createFoodEvent/step/3");
    }

    return (
        // md:grid-cols-[2fr_1fr] 
        <div id="mainWrapper" className="grid grid-cols-1 md:grid-cols-[1fr_auto] gap-10">

            <form id="whenWhereForm" onSubmit={onSubmit} >
                <ComponentsWrapper id="whenWhereForm">

                    <div className="flex flex-col md:flex-row gap-5">
                        <CreateEventFormInput
                            id="date"
                            labelText="Date"
                            type="date"
                            defaultValue={formState.whenWhere?.date ?? new Date().toISOString().split('T')[0]}
                            required
                        />

                        <CreateEventFormInput
                            id="startTime"
                            labelText="Start time"
                            type="time"
                            defaultValue={formState.whenWhere?.startTime ?? "12:30"}
                            required
                        />

                        <CreateEventFormInput
                            id="endTime"
                            labelText="End time (optional)"
                            type="time"
                            defaultValue={formState.whenWhere?.endTime ?? ""}
                        />

                    </div>

                    <InputAddressAutocomplete
                        labelText={"Search your address"}
                        placeholder="e.g. Nørrebrostræde 123, 2. tv"
                        id={"addressAutocomplete"}
                        setFormLocationDetailsAction={setFormLocationDetails} />

                    <CreateEventFormInput
                        id="address"
                        labelText="Street address"
                        type="text"
                        placeholder="Street & number"
                        value={formLocationDetails?.streetNumber ?? formState.whenWhere?.streetAddress ?? ""}
                        onChange={e => setFormLocationDetails(prev => ({
                            ...prev,
                            streetNumber: e.target.value
                        }))}
                        required
                    />

                    <div className="flex">

                        <div className="flex flex-col md:flex-row gap-5 flex-1">
                            <CreateEventFormInput
                                id="postalCode"
                                labelText="Postal code"
                                type="text"
                                placeholder="2200"
                                value={formLocationDetails?.postalCode ?? formState.whenWhere?.postalCode ?? ""}
                                wrapperClassName="flex-1"
                                onChange={e => setFormLocationDetails(prev => ({
                                    ...prev,
                                    postalCode: e.target.value
                                }))}
                                required
                            />

                            <CreateEventFormInput
                                id="city"
                                labelText="City"
                                type="text"
                                placeholder="Copenhagen"
                                value={formLocationDetails?.city ?? formState.whenWhere?.city ?? ""}
                                wrapperClassName="flex-1"
                                onChange={e => setFormLocationDetails(prev => ({
                                    ...prev,
                                    city: e.target.value
                                }))}
                                required
                            />

                            <CreateEventFormInput
                                id="region"
                                labelText="Region"
                                type="text"
                                placeholder="Capital Region"
                                value={formLocationDetails?.region ?? formState.whenWhere?.region ?? ""}
                                wrapperClassName="flex-1"
                                onChange={e => setFormLocationDetails(prev => ({
                                    ...prev,
                                    region: e.target.value
                                }))}
                                required
                            />

                            <CreateEventFormInput
                                id="country"
                                labelText="Country"
                                type="text"
                                placeholder="Denmark"
                                value="Denmark"
                                wrapperClassName="flex-1"
                                disabled
                                required
                            />

                        </div>
                    </div>


                    <OrangeWrapper>
                        <p className="text-muted-gray text-xs font-bold">Your exact address is only shared with confirmed guests.</p>
                    </OrangeWrapper>

                    
                </ComponentsWrapper>

            </form>


            <div className="flex flex-col">
                <ComponentsWrapper id="locationPreview" className="w-100">
                    <h3>Location Preview</h3>
                    <LocationMiniMap 
                        address={formLocationDetails?.streetNumber}
                        city={formLocationDetails?.city}
                        postalCode={formLocationDetails?.postalCode}
                    />
                </ComponentsWrapper>

                <button type="submit" form="whenWhereForm" className="py-2 px-12 w-75 self-center border-1 cursor-pointer border-black text-white text-base font-bold font-['Poppins'] bg-primary-green rounded-xl hover:bg-muted hover:text-foreground transition-colors">Save & Continue</button>
            </div>

        </div>
    );
}