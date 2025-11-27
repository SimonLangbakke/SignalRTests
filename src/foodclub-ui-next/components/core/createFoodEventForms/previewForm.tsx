"use client";

import { CreateEventRequestDto } from "@Rameez349/events-api-sdk/dist/generated/model/createEventRequestDto";
import { postEvent } from "@/services/api/events-api.service";
import { useFormWizardContext } from "./formWizardContext";
import ComponentsWrapper from "../wrappers/componentsWrapper";
import OrangeWrapper from "../wrappers/orangeWrapper";
import MealCard from "../meal-card/meal-card";
import { EventOverviewDto } from "@/types/event-details.type";
import { parse } from 'date-fns';
import { useEffect, useState } from 'react';
import { parseJwt } from "@/lib/jwt-decoder.helper";
import { createEventDefaultData } from "@/services/mocks/createEventDefaultData";
import { useRouter } from "next/navigation";


export default function PreviewForm() {
    const [error, setError] = useState<string | null>(null);
    const [loading, setLoading] = useState(false);
    const [username, setUsername] = useState<string>("");
    const [token, setToken] = useState<string | null>(null);
    const router = useRouter();

    // TODO: refactor, move to a module 
    useEffect(() => {
        const token = localStorage.getItem("accessToken");
        if (token) {
            try {
                const decoded = parseJwt(token);
                setUsername(decoded.unique_name || "");
                setToken(token);
            } catch (err) {
                console.error("Failed to decode token:", err);
            }
        }
    }, []);

    const [formState, formDispatch] = useFormWizardContext();

    // TODO: handle these errors properly
    if (!formState.basics || !formState.whenWhere || !formState.priceCapacity || !formState.dietAllergens) {
        throw new Error("Previous form steps must be completed before this step");
    }

    let eventDetail: EventOverviewDto = {
        ...createEventDefaultData,

        hostName: username,
        name: formState.whenWhere?.city 
            ? `${formState.basics?.title}, ${formState.whenWhere.city}`
            : formState.basics?.title,
        description: formState.basics?.description,
        imageThumbnail: formState.basics?.coverImage || createEventDefaultData.imageThumbnail,
        startDate: formState.whenWhere?.date,
        startTime: formState.whenWhere?.startTime,
        tags: formState.dietAllergens?.dietaryPreferences,
        allergens: formState.dietAllergens?.allergens,
        participantsCount: 0,
        maxAllowedParticipants: formState.priceCapacity?.seatsAvailable,
        price: formState.priceCapacity?.pricePerSeat,
    };

    async function onSubmit(e: React.FormEvent) {
        e.preventDefault();
        setError(null);
        setLoading(true);

        const startDatetimeStr = formState.whenWhere?.date + " " + formState.whenWhere?.startTime;
        const endDatetimeStr = formState.whenWhere?.date + " " + formState.whenWhere?.endTime;

        // parse as local time
        const eventStartDateTime = parse(startDatetimeStr, "yyyy-MM-dd HH:mm", new Date());
        const eventEndDateTime = formState.whenWhere?.endTime ? parse(endDatetimeStr, "yyyy-MM-dd HH:mm", new Date()) : undefined;
        const reservationEndDateTime = new Date(eventStartDateTime)
        reservationEndDateTime.setHours(reservationEndDateTime.getHours() - 1);

        // TODO: check if basics are null
        const createEventReq: CreateEventRequestDto = {
            name: formState.basics?.title ?? "",
            description: formState.basics?.description ?? "",
            pricePerSeat: formState.priceCapacity?.pricePerSeat ?? 0,
            maxAllowedParticipants: formState.priceCapacity?.seatsAvailable ?? 0,
            minAllowedAge: 0,
            maxAllowedAge: 99,
            startDate: eventStartDateTime.toISOString(),
            reservationEndDate: reservationEndDateTime.toISOString(),
            endDate: eventEndDateTime ? eventEndDateTime.toISOString() : undefined,
            imageThumbnail: formState.basics?.coverImage || "https://i0.wp.com/blog.themalamarket.com/wp-content/uploads/2024/06/Vegetarian-pulled-noodles-lead-more-sat.jpg?resize=1200%2C900&ssl=1",
            isPublic: true,
            eventFoodDetails: {
                name: formState.basics?.title ?? "",
                dietaryStyles: formState.dietAllergens?.dietaryPreferences ?? [],
                allergens: formState.dietAllergens?.allergens ?? [],
            },
            images: [],
            streetAddress: formState.whenWhere?.streetAddress ?? "",
            postalCode: formState.whenWhere?.postalCode ?? "",
            city: formState.whenWhere?.city ?? "",
            region: formState.whenWhere?.region ?? "",
        };

        const errMsg = "Failed to create event. Please try again or contact support.";
        try {
            const res = await postEvent(createEventReq, token ?? "");
            console.log("Event created result:", res);

            if (!res || res.status < 200 || res.status >= 300) {
                setError(errMsg);
            } else {
                router.push("/userProfile");
            }
        } catch (err) {
            setError(errMsg);
        }
    }

    return (
        <div className="flex justify-center">

            <form id="livePreviewForm" className="contents" onSubmit={onSubmit}>
                <div className="flex flex-col md:flex-row gap-8 items-start">
                    {/* Left Column: Preview Card */}
                    <ComponentsWrapper id="livePreview">
                        <h3>Live Preview</h3>
                        <MealCard key={123} {...eventDetail} />
                    </ComponentsWrapper>

                    {/* Right Column: Actions */}
                    <div className="flex flex-col gap-4 w-full md:w-64 mt-8">
                        <OrangeWrapper>
                            <p className="text-xs font-semibold text-gray-800">
                                By publishing, you agree to our Host Guidelines and confirm allergen accuracy.
                            </p>
                        </OrangeWrapper>

                        {error && (
                            <p className="text-sm text-red-600 text-center" role="alert">
                                {error}
                            </p>
                        )}
                        
                        <button
                            type="button"
                            disabled={loading}
                            className="w-full py-2 border-1 cursor-pointer border-black text-black text-base font-bold font-['Poppins'] bg-white rounded-xl hover:bg-gray-50 transition-colors disabled:opacity-50 disabled:cursor-not-allowed"
                        >
                            Save as Draft
                        </button>
                        
                        <button
                            type="submit"
                            disabled={loading}
                            className="w-full py-2 border-1 cursor-pointer border-black text-white text-base font-bold font-['Poppins'] bg-primary-green rounded-xl hover:bg-muted hover:text-foreground transition-colors disabled:opacity-50 disabled:cursor-not-allowed"
                        >
                            {loading ? "Publishing..." : "Publish"}
                        </button>
                    </div>
                </div>
            </form>

        </div>
    );
}