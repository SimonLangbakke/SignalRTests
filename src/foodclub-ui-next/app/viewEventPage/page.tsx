"use client";

import {useRouter, useSearchParams} from "next/navigation";
import {useEffect, useState} from "react";
import { EventDetails } from "@/types/event-details.type";
import { UserDetails } from "@/types/user-details.type";

import { parseJwt } from "@/lib/jwt-decoder.helper";
import {getEventById, withdrawFromEvent} from "@/services/api/events-api.service";
import { signUpForEvent } from "@/services/api/events-api.service";
import {getUserById} from "@/services/api/user-api.service";



export default function EventDetailsPage() {
    
    const searchParams = useSearchParams();
    const id = searchParams.get('id');
    const router = useRouter();
    
    const [event, setEvent] = useState<EventDetails | null>(null);
    const [host, setHost] = useState<UserDetails | null>(null);
    const [user, setUser] = useState<UserDetails | null>(null);
    //const [foodDetails, setFoods] = useState<EventFoodDetails | null>(null);

    // helper methods
    const getInitials = (name?: string) => {
        if (!name) return "";
        return name
            .split(" ")
            .filter(Boolean)
            .map(word => word[0])
            .join("")
            .toLowerCase()
    }
    const ingredients = event?.eventFoodDetails.ingredients;
    const ingredientsArray = Array.isArray(ingredients) 
        ? ingredients 
        : typeof ingredients === "string" 
            ? ingredients.split(",").map((i) => i.trim())
            : [];
    
    useEffect(() => {
        if (!id) return;
        
        const token = localStorage.getItem("accessToken");
        if (!token) {
            router.replace("/");
            return;
        }
        
        try {
            (async () => {
                try {
                    const eventData = await getEventById(id as string, token);
                    setEvent(eventData);
                    const hostData = await getUserById(event!.userId)
                    setHost(hostData);
                    const decoded = parseJwt(token);
                    const userData = await getUserById(decoded.sub)
                    setUser(userData);
                } catch (err) {
                    console.error("Failed to fetch event details:", err);
                }
            })();
        } catch (err) {
            console.error("Invalid token:", err);
        }
    }, [id, router]);
    
    const getReserveButtonProps = () => {
        if (!user || !event) return { text: "Reserve a seat", disabled: true, reason: "Log in required"}

        const today = new Date();
        const birthday = new Date(user.birthday);
        let age = today.getFullYear() - birthday.getFullYear();
        const monthDiff = today.getMonth() - birthday.getMonth();
        const dayDiff = today.getDate() - birthday.getDate();
        if (monthDiff < 0 || (monthDiff === 0 && dayDiff < 0)) age--;
        
        const isParticipating = event.eventParticipants.some(p => p.userId == user.id)
        const spotsLeft = event.maxAllowedParticipants - event.eventParticipants.length
        const correctAge = event.minAllowedAge <= age && age <= event.maxAllowedAge
        
        
        if (spotsLeft <= 0) return { text: "Unable to sign up", disabled: true, reason: "No more space" };
        if (!correctAge) return { text: "Unable to sign up", disabled: true, reason: "Age constraints" };
        if (isParticipating) return { text: "Cancel reservation", disabled: false}
        
        return { text: "Reserve a seat", disabled: false}
    }
    
    const reserveButtonProps = getReserveButtonProps();
    
    const handleReserveSeat = async () => {
        if (!user || !event) return;
        
        try {
            const token = localStorage.getItem("accessToken");
            if (!token) return;

            if (event.eventParticipants.every(p => p.userId != user.id)) {
                await signUpForEvent(event.eventId, token)
            } else {
                await withdrawFromEvent(event.eventId, token)
            }

            // Refresh everything =>
            const newToken = localStorage.getItem("accessToken");
            const updatedEvent = await getEventById(event.eventId, newToken!);
            setEvent(updatedEvent);
        } catch (err) {
            console.error("Failed to update event participant", err);
        }
    }
    

    return (
        <main className="max-w-6xl mx-auto px-4 py-10">

            {/* Top section */}
            <div className="grid grid-cols-1 lg:grid-cols-3 gap-10">

                {/* Image placeholder */}
                <div className="lg:col-span-2">
                    <div className="w-full h-80 bg-amber-100 rounded-xl">
                        {event?.imageThumbnail}
                    </div> 
                </div>

                {/* Side card */}
                <div className="bg-white border rounded-xl p-6 h-fit shadow-sm">
                    <div className="text-sm text-gray-600">
                        {event?.startDate}
                    </div>
                    <div className="text-sm text-gray-600 mb-4">
                        {event?.eventAddress}
                    </div>

                    <div className="mb-4">
                        <div className="flex items-center justify-between text-sm mb-1">
                            <span>
                                {event?.eventParticipants.length} / {event?.maxAllowedParticipants} seats left
                            </span>
                        </div>

                        <div className="w-full bg-gray-200 h-2 rounded-full">
                            <div
                                className="bg-green-700 h-2 rounded-full"
                                style={{ 
                                    width: `${
                                        event && event.maxAllowedParticipants > 0 
                                            ? (event.eventParticipants.length / event.maxAllowedParticipants) * 100
                                            : 0
                                    }` 
                                }}
                            />
                        </div>
                    </div>

                    <div className="text-gray-800 text-lg font-semibold mb-6">
                        Price per seat <br />
                        <span className="text-green-800 text-2xl font-bold">
                            DKK event?.price (migrate) 
                        </span>
                    </div>

                    <button 
                        onClick={handleReserveSeat}
                        disabled={reserveButtonProps.disabled}
                        className="w-full py-3 bg-green-800 text-white rounded-full font-medium">
                        {reserveButtonProps.text}     
                    </button>
                    {reserveButtonProps.reason && <p className="text-sm text-red-600 mt-1">{reserveButtonProps.reason}</p>}
                </div>
            </div>

            {/* Meal content */}
            <div className="mt-10">
                <h1 className="text-3xl font-extrabold">
                    {event?.eventFoodDetails.name}
                </h1>

                {/* Host */}
                <div className="flex items-center gap-3 mt-4">
                    <div className="w-10 h-10 bg-green-200 rounded-full flex items-center justify-center font-bold">
                        {getInitials(host?.name)}
                    </div>
                    <div className="flex flex-col">
            <span className="font-medium"> 
                {host?.name} • <span className="text-green-700">
                     host.verified (migrate)
                </span>
            </span>
                        <span className="text-sm text-orange-600 font-semibold">★ host.score (migrate) </span>
                    </div>
                    {ingredientsArray.map((ingredient) => (
                        <span className="bg-green-100 text-green-700 text-xs px-3 py-1 rounded-full">
                        {ingredient}
                        </span>    
                    ))};
                </div>

                {/* About */}
                <div className="mt-6">
                    <h2 className="font-semibold text-lg">About the meal</h2>
                    <p className="mt-2 text-gray-700 leading-relaxed max-w-xl">
                        {event?.description}
                    </p>
                </div>

                {/* Location + Reviews */}
                <div className="grid grid-cols-1 md:grid-cols-2 gap-10 mt-10">

                    {/* Map */}
                    <div className="bg-blue-100 h-64 rounded-xl flex items-center justify-center text-gray-500">
                        Map placeholder (TODO)
                    </div>

                    {/* Reviews */}
                    <div>
                        <h2 className="font-semibold text-lg mb-4">Reviews</h2>

                        <div className="space-y-4">
                            {event?.eventReviews.length ? (
                                event?.eventReviews.map((review) => (
                                    <div
                                        key={review.reviewId}
                                        className="flex items-start gap-3 p-4 bg-white rounded-xl border shadows-sm"
                                    >
                                        <div className="w-10 h-10 bg-green-200 rounded-full flex items-center justify-center font-bold">
                                            {getInitials(review.username)}
                                        </div>
                                        
                                        <div>
                                            <div className="font-medium">
                                                {review.username} - ★ {review.stars} 
                                            </div>
                                            <p className="text-sm text-gray-600">{review.comment}</p>
                                        </div>
                                    </div>
                            ))
                            ) : (
                                <p className="text-gray-500 text-sm">No reviews yet!</p>
                            )}
                        </div>
                    </div>
                </div>
            </div>
        </main>
    );
    
}