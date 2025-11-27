"use client";

import { useEffect, useState } from "react";
import { useRouter } from "next/navigation";
import ComponentsWrapper from "../wrappers/componentsWrapper";
import CreateEventFormTextarea from "./createEventFormTextarea";
import { Label } from "@/components/ui/label";
import { Check } from "lucide-react";
import { FormDietAllergens, FormWizardActionType, FormWizardStep, useFormWizardContext } from "./formWizardContext";
import MealCard from "../meal-card/meal-card";
import { EventOverviewDto } from "@/types/event-details.type";
import { parseJwt } from "@/lib/jwt-decoder.helper";
import { createEventDefaultData } from "@/services/mocks/createEventDefaultData";


export default function DietAllergensForm() {
    const [formState, formDispatch] = useFormWizardContext();
    const router = useRouter();
    const [username, setUsername] = useState<string>("");

    if (!formState.basics || !formState.whenWhere || !formState.priceCapacity) {
        throw new Error("Previous form steps must be completed before this step");
    }

    useEffect(() => {
        const token = localStorage.getItem("accessToken");
        if (token) {
            try {
                const decoded = parseJwt(token);
                setUsername(decoded.unique_name || "");
            } catch (err) {
                console.error("Failed to decode token:", err);
            }
        }
    }, []);

    const [selectedDietaryStyles, setSelectedDietaryStyles] = useState<string[]>(
        formState.dietAllergens?.dietaryPreferences?.length
            ? formState.dietAllergens.dietaryPreferences
            : [
                "Vegetarian",
                "Vegan"
            ]
    );

    const [selectedAllergens, setSelectedAllergens] = useState<string[]>(
        formState.dietAllergens?.allergens?.length
            ? formState.dietAllergens.allergens
            : [
                "Gluten",
                "Nuts"
            ]
    );

    const [notesForGuests, setNotesForGuests] = useState<string>(formState.dietAllergens?.guestsNotes ?? "");

    const dietaryStyles = [
        { id: "Vegetarian", label: "Vegetarian" },
        { id: "Vegan", label: "Vegan" },
        { id: "Pescatarian", label: "Pescatarian" },
        { id: "Halal-friendly", label: "Halal-friendly" },
        { id: "Kosher-friendly", label: "Kosher-friendly" },
    ];

    const allergens = [
        { id: "Gluten", label: "Gluten" },
        { id: "Nuts", label: "Nuts" },
        { id: "Dairy", label: "Dairy" },
        { id: "Eggs", label: "Eggs" },
        { id: "Soy", label: "Soy" },
        { id: "Shellfish", label: "Shellfish" },
        { id: "Sesame", label: "Sesame" },
        { id: "Mustard", label: "Mustard" },
    ];

    let eventDetail: EventOverviewDto = {
        ...createEventDefaultData,
        hostName: username,
        name: formState.whenWhere?.city 
            ? `${formState.basics?.title}, ${formState.whenWhere.city}`
            : formState.basics?.title,
        description: formState.basics?.description,
        imageThumbnail: formState.basics?.coverImage || createEventDefaultData.imageThumbnail,
        maxAllowedParticipants: formState.priceCapacity?.seatsAvailable,


        startDate: formState.whenWhere?.date,
        startTime: formState.whenWhere?.startTime,
        tags: selectedDietaryStyles,
        allergens: selectedAllergens,
        participantsCount: 0,

        price: formState.priceCapacity?.pricePerSeat,

    };

    const toggleDietaryStyle = (style: string) => {
        setSelectedDietaryStyles(prev =>
            prev.includes(style)
                ? prev.filter(s => s !== style)
                : [...prev, style]
        );
    };

    const toggleAllergen = (allergen: string) => {
        setSelectedAllergens(prev =>
            prev.includes(allergen)
                ? prev.filter(a => a !== allergen)
                : [...prev, allergen]
        );
    };

    function onSubmit(e: React.FormEvent) {
        e.preventDefault();

        const formDataObj: FormDietAllergens = {
            dietaryPreferences: selectedDietaryStyles,
            allergens: selectedAllergens,
            guestsNotes: notesForGuests || undefined,
        };

        formDispatch({
            type: FormWizardActionType.Set,
            step: FormWizardStep.DietAllergensForm,
            value: { dietAllergens: formDataObj },
        });

        router.push("/createFoodEvent/step/5");
    }

    return (
        <div id="mainWrapper" className="grid grid-cols-1 md:grid-cols-[1fr_auto] gap-10">
            <form id="dietAllergensForm" className="contents" onSubmit={onSubmit}>
                <ComponentsWrapper id="dietAllergensFormWrapper">

                    <div className="grid gap-2">
                        <Label className="text-xs font-bold text-muted-foreground">
                            Dietary style (select all that apply)
                        </Label>
                        <div className="flex flex-wrap gap-2">
                            {dietaryStyles.map((style) => {
                                const isSelected = selectedDietaryStyles.includes(style.id);
                                return (
                                    <button
                                        key={style.id}
                                        type="button"
                                        onClick={() => toggleDietaryStyle(style.id)}
                                        className={`inline-flex items-center gap-1.5 px-4 py-1.5 rounded-full text-sm font-medium transition-colors ${isSelected
                                            ? "bg-emerald-100 text-emerald-800 border border-emerald-600"
                                            : "bg-gray-100 text-gray-600 border border-gray-300"
                                            }`}
                                    >
                                        {<Check className={`h-4 w-4 ${isSelected? "flex" : "hidden"}`} />}
                                        {style.label}
                                    </button>
                                );
                            })}
                        </div>
                    </div>

                    <div className="grid gap-2">
                        <Label className="text-xs font-bold text-muted-foreground">
                            Allergens clearly labeled
                        </Label>
                        <div className="flex flex-wrap gap-2">
                            {allergens.map((allergen) => {
                                const isSelected = selectedAllergens.includes(allergen.id);
                                return (
                                    <button
                                        key={allergen.id}
                                        type="button"
                                        onClick={() => toggleAllergen(allergen.id)}
                                        className={`inline-flex items-center gap-1.5 px-4 py-1.5 rounded-full text-sm font-medium transition-colors ${isSelected
                                            ? "bg-orange-200 text-orange-900 border border-orange-400"
                                            : "bg-gray-100 text-gray-600 border border-gray-300"
                                            }`}
                                    >
                                        {<Check className={`h-4 w-4 ${isSelected? "flex" : "hidden"}`} />}
                                        {allergen.label}
                                    </button>
                                );
                            })}
                        </div>
                    </div>

                    <CreateEventFormTextarea
                        id="notesForGuests"
                        labelText="Notes for guests (optional)"
                        placeholder="e.g., Traces of nuts possible; soy-free substitutes used."
                        className="h-32 resize-none"
                        value={notesForGuests}
                        onChange={(e) => setNotesForGuests(e.target.value)}
                    />

                </ComponentsWrapper>




            </form>
            <div className="flex flex-col">
                <ComponentsWrapper id="livePreview">
                    <h3>Live Preview</h3>
                    <MealCard key={123} {...eventDetail} />
                </ComponentsWrapper>

                <button type="submit" form="dietAllergensForm" className="py-2 px-12 w-75 self-center border-1 cursor-pointer border-black text-white text-base font-bold font-['Poppins'] bg-primary-green rounded-xl hover:bg-muted hover:text-foreground transition-colors">Save & Continue</button>
            </div>

        </div>
    );
}